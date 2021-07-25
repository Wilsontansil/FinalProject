using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public enum GameStateDotRun
{
    inPlayGame,
    inGameOver
}
public class GameManagerDotRun : MonoBehaviour
{
    public TextMeshProUGUI txtPoint;
    public TextMeshProUGUI txtScore;
    public List<Color> colorGame;

    [Header("Game Manager")]
    public float _GameSpeed;
    public float _timeMain;
    float _timeSecond;
    public float timeSpawnObstacle;
    //public float timeSpawnPoint;
    public float timeAdditionObstcle;
    //public float timeAdditionPoint;
    public GameStateDotRun gameState;
    bool isFinishSendData;
    UserInformationScript userInfo;
    [Header("Panel")]
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject panelPopUpError;

    private void Awake()
    {
        SetColorGame();
        userInfo = FindObjectOfType<UserInformationScript>();
    }
    private void Start()
    {
        setGameRestart();

    }
    void setGameRestart()
    {
        isFinishSendData = false;
        _GameSpeed = 2;
        _timeMain = 0;
        _timeSecond = 0;
        timeSpawnObstacle = 4;
        //timeSpawnPoint = 4;
        timeAdditionObstcle = timeSpawnObstacle;
        //timeAdditionPoint = timeSpawnPoint;
    }
    void SetColorGame()
    {
        colorGame = new List<Color>();
        colorGame.Add(Color.green);
        colorGame.Add(Color.yellow);
        colorGame.Add(Color.blue);
        colorGame.Add(Color.cyan);
        colorGame.Add(Color.magenta);
        colorGame.Add(Color.red);

    }
    private void Update()
    {
        if (gameState== GameStateDotRun.inPlayGame)
        {
            if (Mathf.RoundToInt(_timeMain) % 10 == 0 && _timeMain > 20)
            {
                _GameSpeed = Mathf.RoundToInt(_timeMain) / 10;
            }

            if (_timeSecond > 20)
            {
                timeAdditionObstcle -= .5f;
                //timeAdditionPoint -= .5f;
                _timeSecond = 0;
            }
            _timeMain += Time.deltaTime;
            _timeSecond += Time.deltaTime;
        }

        txtPoint.text = DotRunController.point.ToString();
        txtScore.text = DotRunController.score.ToString();
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

        if (userInfo.highScore < DotRunController.score)
        {
            //PlayerPrefs.SetInt("ColorDotHighScore", ScoreManager.ScoreGame);
            StartCoroutine(UpdateScoreGame(DotRunController.score));
            userInfo.highScore = DotRunController.score;
            panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = DotRunController.score.ToString();

        }
        else
        {
            StartCoroutine(UpdateScoreGame(userInfo.highScore));
            panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = userInfo.highScore.ToString();
        }
        panelGameOver.GetComponent<PanelGameOverManagement>().txtScoreBoard.text = DotRunController.score.ToString();
        panelGameOver.SetActive(true);
        yield return new WaitUntil(() => isFinishSendData);
        panelGameOver.GetComponent<PanelGameOverManagement>().OpenGameOverBox();
        StopAllCoroutines();
    }

    IEnumerator UpdateScoreGame(int newScore)
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", userInfo._UserID);
        form.AddField("ScoreGame", newScore);

        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName+"/UpdateScoreDotRun.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (userInfo.highScore < DotRunController.score)
                {
                    StartCoroutine(UpdateScoreGame(DotRunController.score));
                    userInfo.highScore = DotRunController.score;
                    panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = DotRunController.score.ToString();

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

                isFinishSendData = true;
            }
        }

    }
}
