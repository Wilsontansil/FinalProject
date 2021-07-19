using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static int ScoreGame;
    public TextMeshProUGUI txtScore;


    public void RestartGameScore()
    {
        ScoreGame = 0;
        txtScore.text = 0.ToString();
    }
    public void AddScore(int Point)
    {
        ScoreGame += Point;
        txtScore.text = ScoreGame.ToString();
    }
}
