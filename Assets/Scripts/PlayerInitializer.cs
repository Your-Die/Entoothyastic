using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializePlayerScoreEntry(TextMeshProUGUI name){
        GameManager.Instance.playerName = name.text;
        GameManager.Instance.scoreOverall = 0f;
    }

    public void InitializePlayerScoreEntry(Text name){
        GameManager.Instance.playerName = name.text;
        GameManager.Instance.scoreOverall = 0f;
    }


}
