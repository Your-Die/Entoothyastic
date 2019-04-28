using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class HandAndBrushMover : MonoBehaviourSingleton<HandAndBrushMover>
{
    [SerializeField]
    Transform m_ToothbrushPositionerParent;
    [SerializeField]
    Transform m_ToothbrushBrushingChild;
    [SerializeField]
	float m_SpeedModifierMove = 1.4f;
    [SerializeField]
	float m_SpeedModifierBrush = 4;
	[SerializeField]
	float animationDuration = 1;
    [SerializeField]
    float m_BrushStrokeLength = 0.2f;
    [SerializeField]
    Transform m_RotationKeyframes_TopMouth_Parent;
    [SerializeField]
    Transform m_RotationKeyframes_BottomMouth_Parent;
	//[SerializeField]
    [HideInInspector]
	public List<Transform> RotationKeyframes_TopMouth;
	//[SerializeField]
    [HideInInspector]
	public List<Transform> RotationKeyframes_BottomMouth;
    
	[SerializeField]
	AnimationCurve m_BrushTravelPosAnimCurve;
    [SerializeField]
	AnimationCurve m_BrushTravelRotAnimCurve;
	[SerializeField]
	AnimationCurve m_BrushingAnimCurve;
	

    Vector3 m_TargetPosLastFrame;

    Coroutine[] m_coroutines;

	
    void Awake(){
        m_coroutines = new Coroutine[4];
        RotationKeyframes_TopMouth = new List<Transform>();
        RotationKeyframes_BottomMouth = new List<Transform>();
        foreach(Transform ch in m_RotationKeyframes_TopMouth_Parent){
            RotationKeyframes_TopMouth.Add(ch);
            ch.gameObject.SetActive(false);
        }
        foreach(Transform ch in m_RotationKeyframes_BottomMouth_Parent){
            RotationKeyframes_BottomMouth.Add(ch);
            ch.gameObject.SetActive(false);
        }
	}
	
	void Update(){
	
	}

    /// <summary>
    /// If the new target is different from the previous target, we move to it and then do 1x brush
    /// If the new target is the same as the old target, we just do a 1x brush.
    /// So you need to spam the same command multiple times to brush the tooth more than once.
    /// </summary>
    /// <param name="targetPos"></param>
	public void MoveToTargetAndBrush(ToothInfo targetTooth){
        #region ResetPrevCoroutines
        for(int i=0; i< m_coroutines.Length; i++){
            if(m_coroutines[i]!=null)
                StopCoroutine(m_coroutines[i]);
        }
        m_ToothbrushBrushingChild.localPosition = Vector3.zero;
        m_ToothbrushBrushingChild.localRotation = Quaternion.identity;
        #endregion

        
        Vector3 targetPos = targetTooth.BrushTarget.position + m_BrushStrokeLength * targetTooth.brushStrokeLengthModifier * targetTooth.BrushTarget.up;
        if(targetTooth.UseRandomOffsetToTarget){
            targetPos += new Vector3(
                UnityEngine.Random.Range(-targetTooth.RandomLocalPosOffset.x, targetTooth.RandomLocalPosOffset.x),
                UnityEngine.Random.Range(-targetTooth.RandomLocalPosOffset.y, targetTooth.RandomLocalPosOffset.y),
                UnityEngine.Random.Range(-targetTooth.RandomLocalPosOffset.z, targetTooth.RandomLocalPosOffset.z)
            );
        }
        //cache previous values if target is the same
        bool isTargetTheSame = false;
        //Vector3 brushingUpDir = m_ToothbrushBrushingChild.right;//TODO: figure out depending on how hand is held
        

        if(Vector3.Distance(m_TargetPosLastFrame, targetPos) < 0.01f)
            isTargetTheSame = true;

        if(!isTargetTheSame){
            // calculate keyframe blended rotation
            #region Get Rotation From nearby Keyframes
                Quaternion targetRot = targetTooth.GetClosestBlendedKeyframeRotation(2,false);
            #endregion

            // LerpToTarget pos and rot to target
            m_coroutines[0] = StartCoroutine(LerpToTarget(
                m_ToothbrushPositionerParent,
                m_ToothbrushPositionerParent.position,
                targetPos,
                m_ToothbrushPositionerParent.rotation,
                targetRot,
                m_BrushTravelPosAnimCurve,
                m_BrushTravelRotAnimCurve,
                0, 0, 0,
                animationDuration, m_SpeedModifierMove,
                ()=>{
                    //if(targetTooth.BrushAfterMoving == true)
                        BrushTooth1x_Toggled(m_ToothbrushBrushingChild, targetTooth);
                    }
            ));
        }
        else{
            // brush teeth 1x
            //if(targetTooth.BrushAfterMoving == true)
                BrushTooth1x_Toggled(m_ToothbrushBrushingChild, targetTooth);
        }

        m_TargetPosLastFrame = targetPos;
	}
	
    /// <summary>
    /// Does one brush action per call (per key press).
    /// So It will do a brush up action then next time brush down then next up again etc.
    /// </summary>
    /// <param name="brushingMoveCurve"></param>
    /// <param name="upDir"></param>
    void BrushTooth1x_Toggled(Transform brushTr, ToothInfo targetTooth){
        float dir;
        if(Vector3.Dot(targetTooth.BrushTarget.up, m_ToothbrushBrushingChild.forward) > 0)
            dir = -1;
        else
            dir = 1;

        m_coroutines[1] = StartCoroutine(LerpToTarget(
            brushTr,
            brushTr.position,
            brushTr.position + dir * m_BrushStrokeLength * targetTooth.brushStrokeLengthModifier * m_ToothbrushBrushingChild.forward * 2,
            brushTr.rotation,
            brushTr.rotation,
            m_BrushTravelPosAnimCurve,
            m_BrushTravelRotAnimCurve,
            0, 0, 0,
            animationDuration, targetTooth.BrushAfterMoving? m_SpeedModifierBrush : 0,
            ()=>{
                m_coroutines[2] = StartCoroutine(LerpToTarget(
                    brushTr,
                    brushTr.position,
                    brushTr.position - dir * m_BrushStrokeLength * targetTooth.brushStrokeLengthModifier * m_ToothbrushBrushingChild.forward * 2,
                    brushTr.rotation,
                    brushTr.rotation,
                    m_BrushTravelPosAnimCurve,
                    m_BrushTravelRotAnimCurve,
                    0, 0, 0,
                    animationDuration, targetTooth.BrushAfterMoving? m_SpeedModifierBrush : 0,
                    ()=>{ 
                        // m_coroutines[3] = StartCoroutine(LerpToTarget(
                        //     brushTr,
                        //     brushTr.position,
                        //     brushTr.position + dir * m_brushingMotionWidth * targetTooth.brushStrokeLengthModifier * m_ToothbrushBrushingChild.forward * 2,
                        //     brushTr.rotation,
                        //     brushTr.rotation,
                        //     m_BrushTravelPosAnimCurve,
                        //     m_BrushTravelRotAnimCurve,
                        //     0, 0, 0,
                        //     animationDuration, targetTooth.BrushAfterMoving? m_SpeedModifierBrush : 0,
                        //     ()=>{ 
                                targetTooth.OnBrushedInstanceComplete();
                                //}
                        //));
                    }
                ));
            }
        ));
    }

	IEnumerator LerpToTarget(
		Transform obj, 
		Vector3 startPos, Vector3 targetPos, 
		Quaternion startRot, Quaternion targetRot,
		AnimationCurve moveCurve, AnimationCurve rotCurve,
		float lerpT, float lerpVPos, float lerpVRot,
		float duration, float speedMod,//, float travelDistance
        Action doAfterReachedTarget
		){
		
        while(lerpT<1.0f){
            lerpVPos = moveCurve.Evaluate(lerpT);
            lerpVRot = rotCurve.Evaluate(lerpT);
            obj.position = Vector3.Lerp(startPos, targetPos, lerpVPos);
            obj.rotation = Quaternion.Lerp(startRot, targetRot, lerpVPos);
            lerpT = lerpT + (Time.deltaTime/(duration))*speedMod;
            yield return new WaitForEndOfFrame();
        }

        obj.position = targetPos;
        obj.rotation = targetRot;

        //now we've reached the destination, now we can brush 1x
        doAfterReachedTarget();
	}
}
