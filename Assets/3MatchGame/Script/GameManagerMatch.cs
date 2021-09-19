using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class GameManagerMatch : MonoBehaviour
{
    private Camera MainCamera;
    Vector2 screenBounds;
    [SerializeField] GameObject gbs;
    [SerializeField] GameObject explode;
    [SerializeField] GameObject explodeV2;
    [SerializeField] List<GameObject> ball;
    [SerializeField] List<GameObject> jelly;
    int a = 0;
    int repeat = 0;
    int move;
    [SerializeField] GameObject posNextFruit;
    [SerializeField] GameObject panelPopUpError;
    GameObject nextFruit;
    GameObject playFruit;
    public List<GameObject> fruitInGame;
    bool isPress;
    bool canMove;
    bool isFinsihSendData;
    bool gameover;
    int scoreGame;
    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] TextMeshProUGUI txtMove;
    [SerializeField] GameObject panelGameOver;
    UserInformationScript userInfo;
    [Header("GameSound")]
    [SerializeField] AudioClip clipSqueeze;
    [SerializeField] AudioClip clipFall;
    [SerializeField] AudioClip clipMatch;

    private void Awake()
    {
        userInfo = FindObjectOfType<UserInformationScript>();
        MainCamera = FindObjectOfType<Camera>();
        fruitInGame = new List<GameObject>();
    }
    void Start()
    {
        scoreGame = 0;
        move = 50;
        txtMove.text = move.ToString();
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        insr();
        ins();
        StartCoroutine(GenerateNextMove());
        InvokeRepeating("GenerateRandom", 1, .5f);
        canMove = false;
        isPress = false;
        isFinsihSendData = false;
        gameover = false;
    }

    private void Update()
    {
        CheckMatch();
        if (!gameover)
        {
            if (move > 0)
            {
                if (canMove)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (playFruit != null)
                        {
                            LeanTween.scale(playFruit, playFruit.transform.localScale + new Vector3(.05f, .05f, .05f), .1f).setEase(LeanTweenType.easeInOutElastic).setOnComplete(PopUpAnimation);
                            LeanAudio.play(clipSqueeze);
                        }
                    }
                    if (Input.GetMouseButton(0))
                    {
                        if (playFruit != null)
                        {
                            isPress = true;
                            Vector3 tmp = playFruit.transform.position;
                            tmp = new Vector3(Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, -screenBounds.x + .5f, screenBounds.x - .5f), playFruit.transform.position.y);
                            playFruit.transform.position = tmp;
                        }


                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (isPress)
                        {
                            playFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            LeanAudio.play(clipFall);
                            MoveBall();
                            move -= 1;
                            txtMove.text = move.ToString();
                            canMove = false;
                        }
                    }
                }
            }
            else
            {
                GameOver();
                gameover = true;
            }
        }



    }
    public void instantiateBall(Vector3 x ,GameObject pref)
    {
        a++;
        GameObject gb = Instantiate(pref);
        gb.transform.localScale = Vector3.zero;
        gb.name = "Ball " + a;
        gb.transform.position = new Vector3(x.x,x.y,x.z);
        LeanTween.scale(gb, gb.GetComponent<FruitManager>().Scale, .2f).setEase(LeanTweenType.easeInElastic);
        //fruitInGame.Add(gb);
    }
    void PopUpAnimation()
    {
        LeanTween.scale(playFruit, playFruit.GetComponent<FruitManager>().Scale, .1f);
    }
    void ins()
    {
        GameObject gb = Instantiate(gbs);
        gb.transform.position = new Vector3(screenBounds.x, 0);
    }
    void insr()
    {
        GameObject gb = Instantiate(gbs);
        gb.transform.position = new Vector3(-screenBounds.x, 0);
    }
    IEnumerator GenerateNextMove()
    {
        yield return new WaitForSeconds(.1f);
        nextFruit = Instantiate(ball[Random.Range(0,ball.Count)]);
        nextFruit.transform.localScale = Vector3.zero;
        LeanTween.scale(nextFruit, nextFruit.GetComponent<FruitManager>().Scale, .2f).setEase(LeanTweenType.easeInElastic);
        nextFruit.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(posNextFruit.transform.position).x, Camera.main.ScreenToWorldPoint(posNextFruit.transform.position).y,0);
        nextFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        canMove = true;
        StopCoroutine(GenerateNextMove());
    }
    void CheckMatch()
    {
        if (fruitInGame.Count >2)
        {
            if (fruitInGame[1].GetComponent<FruitManager>().levels == levelObject.L1)
            {
                UpdateScore(3);
                Explode(fruitInGame[1].transform,explode);
                instantiateBall(fruitInGame[1].transform.position, jelly[Random.Range(0, jelly.Count)]);
            }
            else
            {
                Explode(fruitInGame[1].transform, explodeV2);
                UpdateScore(10);
            }
            for (int i = 0; i < fruitInGame.Count; i++)
            {
                fruitInGame[i].GetComponent<FruitManager>().AnimationDestroy(fruitInGame[1].transform);
                LeanAudio.play(clipMatch);
            }
            fruitInGame.Clear();
        }
    }
    void Explode(Transform transform,GameObject explodes)
    {
        GameObject gb = Instantiate(explodes);
        gb.transform.position = transform.position;
        gb.transform.localScale = explode.transform.localScale;
        Destroy(gb, 2f);
    }
    void UpdateScore(int score)
    {
        scoreGame += score;
        txtScore.text = scoreGame.ToString();
    }
    void GenerateRandom()
    {
        if (repeat>15)
        {
            CancelInvoke();
            canMove = true;
            MoveBall();
        }
        else
        {
            if (repeat % 3 == 0)
            {
                instantiateBall(new Vector3(Random.Range(-screenBounds.x+.5f, screenBounds.x -.5f), 3, 0), jelly[Random.Range(0,jelly.Count)]);
            }
            else
            {
                instantiateBall(new Vector3(Random.Range(-screenBounds.x +.5f, screenBounds.x -.5f), 3, 0), ball[Random.Range(0,ball.Count)]);
            }
            repeat++;
        }
    }
    void MoveBall()
    {
        if (nextFruit!=null)
        {
            LeanTween.move(nextFruit, new Vector3(0, 3, 0), .5f).setEase(LeanTweenType.easeInOutCubic).setDelay(1f).setOnComplete(FinishMove);
        }

    }

    void FinishMove()
    {
        playFruit = nextFruit;
        nextFruit = null;
        if (move>1)
        {
            StartCoroutine(GenerateNextMove());
        }
        else
        {
            canMove = true;
        }

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

        if (userInfo.highScore < scoreGame)
        {
            //PlayerPrefs.SetInt("ColorDotHighScore", ScoreManager.ScoreGame);
            StartCoroutine(UpdateScoreGame(scoreGame));
            userInfo.highScore = scoreGame;
            panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = scoreGame.ToString();

        }
        else
        {
            StartCoroutine(UpdateScoreGame(userInfo.highScore));
            panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = userInfo.highScore.ToString();
        }
        panelGameOver.GetComponent<PanelGameOverManagement>().txtScoreBoard.text = scoreGame.ToString();
        panelGameOver.SetActive(true);

        yield return new WaitUntil(()=>isFinsihSendData);
        panelGameOver.GetComponent<PanelGameOverManagement>().OpenGameOverBox();
        StopAllCoroutines();
    }

    IEnumerator UpdateScoreGame(int newScore)
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", userInfo._UserID);
        form.AddField("ScoreGame", newScore);

        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName + "/UpdateScoreMatch3.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (userInfo.highScore < scoreGame)
                {
                    StartCoroutine(UpdateScoreGame(scoreGame));
                    userInfo.highScore = scoreGame;
                    panelGameOver.GetComponent<PanelGameOverManagement>().txtHighScore.text = scoreGame.ToString();

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
                isFinsihSendData = true;

            }
        }

    }
}
