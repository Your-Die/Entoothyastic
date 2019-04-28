using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
public class ToothInfo : MonoBehaviour
{
    public Transform BrushTarget;
    public bool BrushAfterMoving = true;
    public float brushStrokeLengthModifier = 1;
    public float brushTravelSpeedhModifier = 1;
    public float brushStrokeSpeedhModifier = 1;

    public bool useOverrideBrushingAnimDirection;
    public Vector3 OverrideBrushingAnimDirWS = Vector3.forward;

    public bool UseRandomOffsetToTarget = false;
    public Vector3 RandomLocalPosOffset;

    //public string lettersNeeded="";
    //[Range(0,1)]
    //public float dirtiness=0;
    [SerializeField]
    float flipYAxis = 1;
    

    //[SerializeField]
    //HandAndBrushMover m_mover;
    SortedDictionary<float, int> m_SortedDistancesAndIndexes;
    List<Transform> m_keyframeSource;
    float m_maxDistance;

    [System.Serializable]
    public class Event : UnityEvent<ToothInfo> { }
    [FormerlySerializedAs("OnInputSuccess")] public Event OnSuccessfullyBrushed_1x;

    [System.Serializable]
    public class Event2 : UnityEvent<ToothInfo> { }
    [FormerlySerializedAs("OnInputSuccess")] public Event OnStartedToBrush_1x;

    [System.Serializable]
    public class Event3 : UnityEvent<ToothInfo> { }
    [FormerlySerializedAs("OnInputSuccess")] public Event OnStartedToPokeOnBrushingFail;
    

    // Start is called before the first frame update
    void Start()
    {
        if(BrushTarget==null)
            foreach(Transform ch in transform){
                if(ch.name.ToLower().Contains("text")){
                    BrushTarget = ch;
                }
            }

        #region Distance Calculation
        if(Vector3.Dot(flipYAxis * transform.up, Vector3.up) > 0){
            m_keyframeSource = HandAndBrushMover.Instance.RotationKeyframes_BottomMouth;
        }
        else{
            m_keyframeSource = HandAndBrushMover.Instance.RotationKeyframes_TopMouth;
        }
        m_SortedDistancesAndIndexes = new SortedDictionary<float, int>();
        #endregion


    }

    public void OnBrushedInstanceComplete(){
        OnSuccessfullyBrushed_1x?.Invoke(this);
    }

    public void OnStartingToBrush(){
        AudioManager.instance.Play("Toothbrush");
        OnStartedToBrush_1x?.Invoke(this);
    }

    
    public void OnStartingToPokeOnBrushingFail(){
        AudioManager.instance.Play("Vocal8");
        OnStartedToPokeOnBrushingFail?.Invoke(this);
    }

    

    public Quaternion GetClosestBlendedKeyframeRotation(int numberOfClosestKeyframes = 2, bool forceRecalculate = false){
        Quaternion targetRot = Quaternion.identity;
        
        #region Distance Calculation
        if(forceRecalculate || m_SortedDistancesAndIndexes.Count == 0){
            m_maxDistance = 0;
            m_SortedDistancesAndIndexes = new SortedDictionary<float, int>();
            for(int i=0; i<m_keyframeSource.Count; i++){
                float dist = Vector3.Distance(BrushTarget.position, m_keyframeSource[i].position);
                
                if(m_SortedDistancesAndIndexes.ContainsKey(dist)){
                    Debug.Log(dist+"; i: "+i);
                    m_SortedDistancesAndIndexes.Add(dist+001f*i, i);
                }
                else
                {
                    m_SortedDistancesAndIndexes.Add(dist, i);
                }

                if(dist>m_maxDistance)
                    m_maxDistance = dist;
            }
        }
        #endregion

        //List<Quaternion> rotations = new List<Quaternion>();
        Quaternion rot = Quaternion.identity;
        int c = 0;
        //for(int i = 0; i< numberOfClosestKeyframes; i++){
        foreach(KeyValuePair<float, int> entry in m_SortedDistancesAndIndexes){
            if(c>=numberOfClosestKeyframes)
                break;
            c++;
            //rotations.Add(m_keyframeSource[m_SortedDistancesAndIndexes[0]].rotation);
            //Debug.Log(entry.Key+", "+entry.Value);
            m_keyframeSource[entry.Value].gameObject.SetActive(true);
            StartCoroutine(DisableAfterTime(3,m_keyframeSource[entry.Value].gameObject));

            rot = Quaternion.Lerp(
                m_keyframeSource[entry.Value].rotation,
                rot,
                entry.Key/m_maxDistance
                );
        }
        return rot;
    }

    IEnumerator DisableAfterTime(float time, GameObject objToDisable){
        yield return new WaitForSeconds(time);
        objToDisable.SetActive(false);
    }
}
