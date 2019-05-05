using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {
    //[SerializeField] private TMP_Text _scoreText;
    //float score = 0f;
    string playerName;
    const int numbOfHighscores = 5;

    char listSeperator = '|', userSeperator = '&';

    string[] scoreSaveList = new string[numbOfHighscores];
    string[] scoreNameList = new string[numbOfHighscores];
    float[] scorePointList = new float[numbOfHighscores];

    string scoreData;
    string tmpScoreData;

    [SerializeField] TMP_Text[] scoreNameGUI;
    [SerializeField] TMP_Text[] scorePointGUI;

    // public void AddToScore(int scoreIncrease) {

    //     score += scoreIncrease;
    //     _scoreText.text = score.ToString();
    // }


    public void UpdateGUIList() {
        for (int i = 0; i < numbOfHighscores; i++) {
            scoreNameGUI[i].text = scoreNameList[i];
            scorePointGUI[i].text = scorePointList[i].ToString();

        }
    }

    string ScoreDataEncoder() {
        string temp = "";
        for(int i = 0; i < numbOfHighscores-1; i++) {
            temp += scoreNameList[i] + userSeperator + scorePointList[i] + listSeperator;
        }
        temp += scoreNameList[numbOfHighscores-1] + userSeperator + scorePointList[numbOfHighscores - 1];
        Debug.Log(temp);
        return temp;
    }
    public void ZeroOutScores() {
        for (int i = 0; i < numbOfHighscores; i++) {
            scoreNameList[i] = "BillyBob";
            scorePointList[i] = 0f;
        }

        SaveHighscore();
        UpdateGUIList();
    }
    void ScoreDataDecoder(string data) {
        if (data == "") {
            ZeroOutScores();
            return;
        }
        Debug.Log("Loading scores: "+data);

        string[] tempUsers = data.Split(listSeperator);
        for (int i = 0; i < numbOfHighscores; i++) {
            string[] tempIndividual = tempUsers[i].Split(userSeperator);
            scoreNameList[i] = tempIndividual[0];
            float.TryParse(tempIndividual[1], out scorePointList[i]);
            Debug.Log("Parsed score: "+scoreNameList[i]+"; "+scorePointList[i]);
        }
    }

    public void SaveNewHighscore(string name) {
        playerName = name;
        List<(string, float)> scores = new List<(string, float)>();
        for (int i = 0; i < numbOfHighscores; i++) {
            scores.Add((scoreNameList[i], scorePointList[i]));
        }
        scores.Add((playerName, GameManager.Instance.scoreOverall));
        Debug.Log("Player score: "+playerName+"; "+ GameManager.Instance.scoreOverall);
        scores = scores.OrderBy(score => score.Item2).ToList();
        scores.Reverse();
        for (int i = 0; i < numbOfHighscores; i++) {
            scoreNameList[i] = scores[i].Item1;
            scorePointList[i] = scores[i].Item2;
        }

        SaveHighscore();
    }

    public void SaveHighscore() {
        PlayerPrefs.SetString("score", ScoreDataEncoder());
        PlayerPrefs.Save();

    }

    // Start is called before the first frame update
    void Start()
    {
        ScoreDataDecoder(PlayerPrefs.GetString("score"));
        SaveNewHighscore(GameManager.Instance.playerName);
        UpdateGUIList();

    }
    private void Update() {
        //UpdateGUIList();
        
    }

}
