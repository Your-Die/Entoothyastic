using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSaver : MonoBehaviourSingleton<ScoreSaver>
{

    public float scoreOverall;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void resetscore() {
        scoreOverall = 0;
    }
    public void SetScore(float amount) {
        scoreOverall += amount;
    }

}
