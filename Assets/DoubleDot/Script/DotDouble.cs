using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public enum DotDoubleState
{
    startGame,
    game,
    gameOver

}
public class DotDouble : MonoBehaviour
{

    float rotataion;
    int speed;
    public bool Rotate;
    float screenBounds;
    int score;
    bool isFinishSendData;
    public DotDoubleState state;
    [SerializeField] GameObject txtPress;
    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] GameObject panelPopUpError;
    [SerializeField] GameObject panelGameOver;
    UserInformationScript userInfo;

    [Header("Game")]
    [SerializeField] GameObject explode;
    [SerializeField] GameObject getPoint;
    [Header("Sound Setting")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clipMusicStart;
    [SerializeField] AudioClip clipMusicGo;
    [SerializeField] AudioClip clipPoint;

    private void Awake()
    {
        userInfo = FindObjectOfType<UserInformationScript>();
    }
    void Start()
    {
        isFinishSendData = false;
        audioSource.clip = clipMusicStart;
        audioSource.Play();
        Rotate = false;
        score = 0;
        txtScore.text = score.ToString();
        state = DotDoubleState.startGame;
        screenBounds = Screen.width;
    }

    void Update()
    {
        if (state == DotDoubleState.startGame && Input.GetMouseButtonDown(0))
        {
            state = DotDoubleState.game;
            audioSource.clip = clipMusicGo;
            audioSource.Play();
            txtPress.SetActive(false);
        }
        if (state == DotDoubleState.game)
        {
            if (Input.GetMouseButton(0))
            {
                Debug.LogError(Input.mousePosition + " " + screenBounds / 2);
                if (Input.mousePosition.x > screenBounds / 2)
                {
                    Debug.LogError("Right");
                    rotataion += 300 * Time.deltaTime;
                }
                else
                {
                    Debug.LogError("Left");
                    rotataion -= 300 * Time.deltaTime;

                }
                gameObject.transform.rotation = Quaternion.Euler(0, 0, rotataion);
            }

            //if (Input.GetMouseButtonDown(0))
            //{
            //    Rotate = !Rotate;
            //}
            //if (Rotate)
            //{
            //    rotataion += 200 * Time.deltaTime;

            //}
            //else
            //{
            //    rotataion -= 200 * Time.deltaTime;
            //}

        }


    }

    public void UpdateScore()
    {
        score += 10;
        txtScore.text = score.ToString();
    }

    public void GameOver(Transform pos)
    {
        if (state==DotDoubleState.game)
        {
            LeanTween.move(transform.GetChild(0).gameObject, pos.position, .1f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.move(transform.GetChild(1).gameObject, pos.position, .1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => Explode());
            state = DotDoubleState.gameOver;
        }

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
        GameOver(transform);
    }



    IEnumerator CountDownGameOver()
    {

        if (userInfo.highScore < score)
        {
            StartCoroutine(UpdateScoreGame(score));
            userInfo.highScore = score;
            panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = score.ToString();

        }
        else
        {
            StartCoroutine(UpdateScoreGame(userInfo.highScore));
            panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = userInfo.highScore.ToString();
        }
        panelGameOver.GetComponent<PanelGameOverManagement>().txtScoreBoard.text = score.ToString();
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

        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName + "/UpdateScoreDoubleDot.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);

                if (userInfo.highScore < score)
                {
                    StartCoroutine(UpdateScoreGame(score));
                    userInfo.highScore = score;
                    panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = score.ToString();

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

    void Explode()
    {
        GameObject gb = Instantiate(explode);
        gb.transform.position = transform.GetChild(0).position;
        GameObject gb1 = Instantiate(explode);
        gb1.transform.position = transform.GetChild(1).position;
        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetChild(1).gameObject);
    }

    public void GetPoint(Transform pos)
    {
        GameObject gb = Instantiate(getPoint);
        gb.transform.position = pos.position;
        Destroy(gb, 5f);
        LeanAudio.play(clipPoint);
    }

}
