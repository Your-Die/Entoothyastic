using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothInfo : MonoBehaviour
{
    public string lettersNeeded="";
    [Range(0,1)]
    public float dirtiness=0;
    //[SerializeField]
    //HandAndBrushMover m_mover;
    SortedDictionary<float, int> m_SortedDistancesAndIndexes;
    List<Transform> m_keyframeSource;
    float m_maxDistance;

    // Start is called before the first frame update
    void Start()
    {

        #region Distance Calculation
        if(Vector3.Dot(transform.up, Vector3.up) > 0){
            m_keyframeSource = HandAndBrushMover.Instance.RotationKeyframes_BottomMouth;
        }
        else{
            m_keyframeSource = HandAndBrushMover.Instance.RotationKeyframes_TopMouth;
        }
        m_SortedDistancesAndIndexes = new SortedDictionary<float, int>();
        #endregion


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Quaternion GetClosestBlendedKeyframeRotation(int numberOfClosestKeyframes = 2, bool forceRecalculate = false){
        Quaternion targetRot = Quaternion.identity;
        
        #region Distance Calculation
        if(forceRecalculate || m_SortedDistancesAndIndexes.Count == 0){
            m_maxDistance = 0;
            m_SortedDistancesAndIndexes = new SortedDictionary<float, int>();
            for(int i=0; i<m_keyframeSource.Count; i++){
                float dist = Vector3.Distance(transform.position, m_keyframeSource[i].position);
                
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
