using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public enum GameStateFeedIt
{
    ready,
    startGame,
    gameOver
}
public class GameManagerHit : MonoBehaviour
{
    [SerializeField] List<GameObject> dogObj;
    [SerializeField] List<GameObject> foodPref;
    [SerializeField] List<Button> buttonGame;
    [SerializeField] GameObject panelPopUpError;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject pressStart;
    [SerializeField] TextMeshProUGUI txtTime;
    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] GameObject popUpCookies;
    [SerializeField] GameObject popUpBones;
    [SerializeField] GameObject popUpFood;
    [SerializeField] GameObject combo3;
    [SerializeField] GameObject combo5;
    [SerializeField] GameObject combo10;
    [SerializeField] GameObject Nice;
    [SerializeField] GameObject Excellent;
    [SerializeField] GameObject mainMenu;

    [Header("Game Logic")]
    int combo;
    float timeGame;
    public float timeCombo;
    int scoreGame;
    bool isFinishSendData;
    public GameStateFeedIt gameState;

    bool canMove;
    float foodPosY;
    float foodSpace;
    List<GameObject> foodInGame;
    UserInformationScript userInfo;

    [Header("Sound")]
    [SerializeField] AudioClip clipEat;
    [SerializeField] AudioClip clipLose;
    [SerializeField] AudioClip clipNice;
    [SerializeField] AudioClip clipExcellent;
    private void Awake()
    {
        foodInGame = new List<GameObject>();
        foodSpace = 1;
        foodPosY = 1 - foodSpace;
        canMove = true;
        userInfo = FindObjectOfType<UserInformationScript>();
    }
    private void Start()
    {
        isFinishSendData = false;
        pressStart.SetActive(true);
        gameState = GameStateFeedIt.ready;
        timeGame = 30;
        timeCombo = .5f;
        txtTime.text = Mathf.Round(timeGame).ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            BTNPress(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            BTNPress(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            BTNPress(2);
        }
        if (gameState== GameStateFeedIt.ready)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pressStart.SetActive(false);

                for (int i = 0; i < 5; i++)
                {
                    InstantiateBall();
                }
                for (int i = 0; i < buttonGame.Count; i++)
                {
                    buttonGame[i].interactable = true;
                }
                gameState = GameStateFeedIt.startGame;
            }
        }
        if (gameState == GameStateFeedIt.startGame)
        {
            timeGame -= Time.deltaTime;
            if (timeCombo>0)
            {
                timeCombo -= Time.deltaTime;
            }
            if (timeCombo<=0)
            {
                combo = 0;
            }
            if (timeGame <=0)
            {
                timeGame = 0;
                gameState = GameStateFeedIt.gameOver;
                GameOver();
            }
            txtTime.text = Mathf.Round(timeGame).ToString();
        }

    }

    void PressDog(int num)
    {
        LeanTween.moveY(dogObj[num],3f, .1f).setOnComplete(()=>MoveBack(num));
    }

    void MoveBack(int x)
    {
        LeanTween.moveY(dogObj[x], 3.5f, .1f);
    }
    void InstantiateBall()
    {
        foodPosY -= foodSpace;
        int x = Random.Range(0, foodPref.Count);
        GameObject gb = Instantiate(foodPref[x]);
        gb.name = gb.GetComponent<FoodInformation>().ballPosition.ToString();
        gb.transform.SetParent(gameObject.transform);
        //gb.transform.localScale = new Vector3(.3f, .3f, .3f);
        gb.transform.localPosition = new Vector3(gb.transform.localPosition.x, foodPosY, gb.transform.localPosition.x);
        foodInGame.Add(gb);

    }
    IEnumerator DestroyBall()
    {
        canMove = false;
        yield return new WaitForSeconds(.05f);
        Destroy(foodInGame[0].gameObject);
        foodInGame.RemoveAt(0);
        InstantiateBall();
        foodPosY += foodSpace;
        BallPositionDown();
    }
    void BallPositionDown()
    {
        foreach (var item in foodInGame)
        {
            LeanTween.moveLocalY(item, item.transform.localPosition.y + foodSpace, .05f).setOnComplete(CheckMove);
        }
    }
    void CheckMove()
    {
        canMove = true;
    }
    public void BTNPress(int x)
    {
        if (gameState == GameStateFeedIt.startGame)
        {
            if (canMove)
            {
                PressDog(x);
                if (true)
                {
                    if (foodInGame[0].GetComponent<FoodInformation>().ballIndex == x)
                    {
                        if (foodInGame[0].GetComponent<FoodInformation>().ballPosition== BallPosition.bones)
                        {
                            instantiatePopUp(popUpBones, foodInGame[0].transform);
                        }
                        else if(foodInGame[0].GetComponent<FoodInformation>().ballPosition == BallPosition.cookies)
                        {
                            instantiatePopUp(popUpCookies, foodInGame[0].transform);
                        }
                        else if(foodInGame[0].GetComponent<FoodInformation>().ballPosition == BallPosition.food)
                        {
                            instantiatePopUp(popUpFood, foodInGame[0].transform);
                        }

                        StartCoroutine(DestroyBall());
                        LeanAudio.play(clipEat);
                        CheckCombo(true);
                    }
                    else
                    {
                        LeanAudio.play(clipLose);
                        CheckCombo(false);
                        canMove = false;
                        //foreach (var item in dogObj)
                        //{
                        //    item.GetComponent<Animator>().SetTrigger("Hurt");
                        //}
                        dogObj[x].GetComponent<Animator>().SetTrigger("Hurt");
                        //LeanAudio.play(missHammer);
                        LeanTween.moveY(gameObject, 2, .5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(() => BackAnimation(x));
                    }
                }
            }
        }


    }

    void BackAnimation(int x)
    {
        LeanTween.moveY(gameObject.gameObject, 3, .2f);
        //foreach (var item in dogObj)
        //{
        //    item.GetComponent<Animator>().SetTrigger("Idle");
        //}
        dogObj[x].GetComponent<Animator>().SetTrigger("Idle");
        canMove = true;
    }

    void UpdateScore(int add)
    {
        scoreGame += add;
        txtScore.text = scoreGame.ToString();
    }
    void CheckCombo(bool combos)
    {
        if (combos)
        {
            combo += 1;
            timeCombo = .5f;
            if (combo == 3)
            {
                Debug.LogError("Combo3");
                UpdateScore(10*3);
                instantiateCombo(combo3, foodInGame[0].transform.position);
            }
            else if (combo == 5)
            {
                Debug.LogError("Combo 5");
                UpdateScore(10*5);
                instantiateCombo(combo5, foodInGame[0].transform.position);
                LeanAudio.play(clipNice);
                instantiateFunEffect(Nice,Vector3.zero);
            }
            else if(combo == 10)
            {
                Debug.LogError("Combo 10");
                UpdateScore(10*10);
                instantiateCombo(combo10, foodInGame[0].transform.position);
                LeanAudio.play(clipExcellent);
                instantiateFunEffect(Excellent, Vector3.zero);
                combo = 0;
            }
            else
            {
                UpdateScore(10);
            }
        }
        else
        {
            combo = 0;
            timeCombo = .5f;
            UpdateScore(-5);
        }

    }
    void instantiatePopUp(GameObject popUp,Transform tf)
    {
        GameObject gb = Instantiate(popUp);
        gb.transform.position = tf.position;
        gb.transform.localScale = new Vector3(.6f, .6f);
        Destroy(gb,1f);
    }
    void instantiateCombo(GameObject combo, Vector3 pos)
    {
        GameObject gb = Instantiate(combo);
        gb.transform.position = pos;
        gb.transform.localScale = Vector3.zero;
        LeanTween.scale(gb, new Vector3(.4f, .4f), .3f).setEase(LeanTweenType.easeInOutElastic).setOnComplete(() => LeanTween.moveY(gb, transform.position.y + .5f, .1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => Destroy(gb, .2f)));
    }

    void instantiateFunEffect(GameObject combo, Vector3 pos)
    {
        GameObject gb = Instantiate(combo);
        gb.transform.position = pos;
        gb.transform.localScale = Vector3.zero;
        LeanTween.scale(gb, new Vector3(.4f, .4f), .3f).setEase(LeanTweenType.easeOutBounce).setOnComplete(() => LeanTween.scale(gb, new Vector3(.5f,.5f), .1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => Destroy(gb, .2f)));
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
        yield return new WaitUntil(() => isFinishSendData);
        panelGameOver.GetComponent<PanelGameOverManagement>().OpenGameOverBox();
        StopAllCoroutines();
    }

    IEnumerator UpdateScoreGame(int newScore)
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", userInfo._UserID);
        form.AddField("ScoreGame", newScore);

        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName + "/UpdateScoreFeedIt.php ", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);

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
            }
            else
            {
                Debug.Log("Sucess Update");
                isFinishSendData = true;

            }
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
}
