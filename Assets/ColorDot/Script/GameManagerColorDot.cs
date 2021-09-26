using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManagerColorDot : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabObstacle;
    [SerializeField] GameObject pointColor;
    [SerializeField] GameObject obstacleParent;
    public List<Color> ColorList;
    public int posYObstacle;
    public int posYChangColor;
    bool IsFinishSendData;
    [Header("Panel")]
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject panelPopUpError;
    [SerializeField] GameObject mainMenu;

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
        IsFinishSendData = false;
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
        yield return new WaitUntil(() => IsFinishSendData);
        panelGameOver.GetComponent<PanelGameOverManagement>().OpenGameOverBox();
        StopAllCoroutines();
    }
    void DestroyObstacle()
    {
        if (obstacleParent.transform.childCount>7)
        {
            Destroy(obstacleParent.transform.GetChild(0).gameObject);
        }
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        LeanTween.scale(mainMenu.transform.GetChild(0).gameObject, Vector3.one, .5f).setEase(LeanTweenType.easeOutCubic);
    }
    public void CloseMainMenu()
    {
        LeanTween.scale(mainMenu.transform.GetChild(0).gameObject, Vector3.zero, .3f).setEase(LeanTweenType.easeInCubic).setOnComplete(() => mainMenu.SetActive(false));
    }
    public void BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
                if (userInfo.highScore < ScoreManager.ScoreGame)
                {
                    StartCoroutine(UpdateScoreGame(ScoreManager.ScoreGame));
                    userInfo.highScore = ScoreManager.ScoreGame;
                    panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = ScoreManager.ScoreGame.ToString();

                }
                else
                {
                    StartCoroutine(UpdateScoreGame(userInfo.highScore));
                    panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = userInfo.highScore.ToString();
                }
            }
            else
            {
                Debug.Log("Sucess Update");
                IsFinishSendData = true;

            }
        }

    }
    #endregion
}
