using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandAndBrushMover : MonoBehaviourSingleton<HandAndBrushMover>
{
    [SerializeField]
    Transform m_ToothbrushPositionerParent;
    [SerializeField]
    Transform m_ToothbrushBrushingChild;
    [SerializeField]
	float m_SpeedModifier = 1;
	[SerializeField]
	float animationDuration = 1;
    [SerializeField]
    float m_brushingMotionWidth = 0.1f;
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

	
    void Awake(){
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
        Vector3 targetPos = targetTooth.transform.position;
        //cache previous values if target is the same
        bool isTargetTheSame = false;
        Vector3 brushingUpDir = m_ToothbrushBrushingChild.forward;//TODO: figure out depending on how hand is held
        

        if(Vector3.Distance(m_TargetPosLastFrame, targetPos) < 0.01f)
            isTargetTheSame = true;

        if(!isTargetTheSame){
            // calculate keyframe blended rotation
            #region Get Rotation From nearby Keyframes
                Quaternion targetRot = targetTooth.GetClosestBlendedKeyframeRotation(2,false);
            #endregion

            // LerpToTarget pos and rot to target
            StartCoroutine(LerpToTarget(
                m_ToothbrushPositionerParent,
                m_ToothbrushPositionerParent.position,
                targetPos,
                m_ToothbrushPositionerParent.rotation,
                targetRot,
                m_BrushTravelPosAnimCurve,
                m_BrushTravelRotAnimCurve,
                0, 0, 0,
                animationDuration, m_SpeedModifier,
                ()=>{BrushTooth1x_Toggled(m_ToothbrushBrushingChild,brushingUpDir);}
            ));
        }
        else{
            // brush teeth 1x
            BrushTooth1x_Toggled(m_ToothbrushBrushingChild, brushingUpDir);
        }

        m_TargetPosLastFrame = targetPos;
	}
	
    /// <summary>
    /// Does one brush action per call (per key press).
    /// So It will do a brush up action then next time brush down then next up again etc.
    /// </summary>
    /// <param name="brushingMoveCurve"></param>
    /// <param name="upDir"></param>
    void BrushTooth1x_Toggled(Transform brushTr, Vector3 upDir){
        StartCoroutine(LerpToTarget(
            brushTr,
            brushTr.position,
            brushTr.position + m_brushingMotionWidth * upDir,
            brushTr.rotation,
            brushTr.rotation,
            m_BrushTravelPosAnimCurve,
            m_BrushTravelRotAnimCurve,
            0, 0, 0,
            animationDuration, m_SpeedModifier,
            ()=>{
                StartCoroutine(LerpToTarget(
                    brushTr,
                    brushTr.position,
                    brushTr.position - m_brushingMotionWidth * upDir,
                    brushTr.rotation,
                    brushTr.rotation,
                    m_BrushTravelPosAnimCurve,
                    m_BrushTravelRotAnimCurve,
                    0, 0, 0,
                    animationDuration, m_SpeedModifier,
                    ()=>{}
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
		
        lerpT = lerpT + (Time.deltaTime/(duration))*speedMod;
        if(lerpT<1.0f){
            lerpVPos = moveCurve.Evaluate(lerpT);
            lerpVRot = rotCurve.Evaluate(lerpT);
            obj.position = Vector3.Lerp(startPos, targetPos, lerpVPos);
            obj.rotation = Quaternion.Lerp(startRot, targetRot, lerpVPos);
            yield return new WaitForEndOfFrame();
        }

        obj.position = targetPos;
        obj.rotation = targetRot;

        //now we've reached the destination, now we can brush 1x
        doAfterReachedTarget();
	}
}
