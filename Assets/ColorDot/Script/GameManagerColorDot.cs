﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManagerColorDot : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabObstacle;
    [SerializeField] GameObject pointColor;
    [SerializeField] GameObject obstacleParent;
    public List<Color> ColorList;
    public int posYObstacle;
    public int posYChangColor;

    [Header("Panel")]
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject panelPopUpError;

    [Header("Game Manager")]
    ScoreManager scoreManager;
    UserInformationScript userInfo;
    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        userInfo = FindObjectOfType<UserInformationScript>();
        posYChangColor = 2;
        posYObstacle = 0;
    }

    void colorManager()
    {
        ColorList = new List<Color>();
        ColorList.Add(Color.yellow);
        ColorList.Add(Color.cyan);
        ColorList.Add(Color.red);
        ColorList.Add(Color.green);
    }

    private void Start()
    {
        scoreManager.RestartGameScore();
        colorManager();
        instantiateObstacle();
        instantiateObstacle();
        instantiatePoint();
        instantiatePoint();
    }
    public void instantiateObstacle()
    {
        int x = Random.Range(0, prefabObstacle.Count);
        GameObject gb = Instantiate(prefabObstacle[x]);
        gb.transform.SetParent(obstacleParent.transform);
        gb.transform.localPosition = new Vector3(0, posYObstacle, 0);
        //gb.GetComponentInChildren<ObstacleColorManager>().SetColor(ColorList[0], ColorList[1], ColorList[2], ColorList[3]);
        posYObstacle += 4;
        DestroyObstacle();
    }

    public void instantiatePoint()
    {
        GameObject gb = Instantiate(pointColor);
        gb.transform.SetParent(obstacleParent.transform);
        gb.transform.localPosition = new Vector3(0, posYChangColor, 0);
        //gb.GetComponentInChildren<ObstacleColorManager>().SetPointColor(ColorList[2], ColorList[3], ColorList[0], ColorList[1]);
        posYChangColor += 4;
        DestroyObstacle();
    }

    public void GameOver()
    {
        if (NetworkConnection.CheckConnection())
        {
            StartCoroutine(CountDownGameOver());
        }
        else
        {
            panelPopUpError.SetActive(true);
            panelPopUpError.GetComponent<PanelLostConnection>().SetMessage("Please Check Your Internet Connection", "Retry", "Quit");
            panelPopUpError.GetComponent<PanelLostConnection>().btnOk.onClick.AddListener(Retry);
            panelPopUpError.GetComponent<PanelLostConnection>().btnCancel.onClick.AddListener(() => Application.Quit());
            LeanTween.scale(panelPopUpError.transform.GetChild(0).gameObject, Vector2.one, .5f).setEase(LeanTweenType.easeInOutExpo);
        }

    }

    void Retry()
    {
        LeanTween.scale(panelPopUpError.transform.GetChild(0).gameObject, Vector2.zero, .5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(retry);
    }
    void retry()
    {
        panelPopUpError.SetActive(false);
        GameOver();
    }



    IEnumerator CountDownGameOver()
    {
        yield return new WaitForSeconds(2f);
        if (userInfo.highScore < ScoreManager.ScoreGame)
        {
            //PlayerPrefs.SetInt("ColorDotHighScore", ScoreManager.ScoreGame);
            StartCoroutine(UpdateScoreGame(ScoreManager.ScoreGame));
            userInfo.highScore = ScoreManager.ScoreGame;
            panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = ScoreManager.ScoreGame.ToString();

        }
        else
        {
            StartCoroutine(UpdateScoreGame(userInfo.highScore));
            panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = userInfo.highScore.ToString();
        }
        panelGameOver.GetComponent<PanelGameOverManagement>().txtScoreBoard.text = ScoreManager.ScoreGame.ToString();
        panelGameOver.SetActive(true);
        panelGameOver.GetComponent<PanelGameOverManagement>().OpenGameOverBox();
        StopCoroutine(CountDownGameOver());
    }
    void DestroyObstacle()
    {
        if (obstacleParent.transform.childCount>7)
        {
            Destroy(obstacleParent.transform.GetChild(0).gameObject);
        }
    }

    #region WWW Web
    IEnumerator UpdateScoreGame(int newScore)
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", userInfo._UserID);
        form.AddField("ScoreGame", newScore);

        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName+"/UpdateScoreDot.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);

            }
            else
            {
                Debug.Log("Sucess Update");


            }
        }
        StopAllCoroutines();
    }
    #endregion
}
