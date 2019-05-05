using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviourSingleton<GameManager>
{

    public float scoreOverall;
    public string playerName = "unnamed brusher";
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
    }

    public void resetscore() {
        scoreOverall = 0;
    }
    public void SetScore(float amount) {
        scoreOverall += amount;
    }




    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
