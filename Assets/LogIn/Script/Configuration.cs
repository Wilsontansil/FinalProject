using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Configuration : MonoBehaviour
{

    UserInformationScript userInfo;
    [Header("UserInfoLogIn")]
    public TMP_InputField txtID;
    public TMP_InputField txtPassword;
    public GameObject panelLoading;
    public TextMeshProUGUI errorMessage;
    [Header("UserInterface")]
    [SerializeField] GameObject userInputID;
    [SerializeField] GameObject userInputPass;
    [SerializeField] GameObject popUpError;
    [Header("Timer Loading")]
    float timer;
    bool loading;


    private void Start()
    {
        //Debug.LogError(PlayerPrefs.GetString("loginUserID"));
        //if (PlayerPrefs.GetString("loginUserID")!= "")
        //{
        //    txtID.text = PlayerPrefs.GetString("loginUserID");
        //    txtPassword.text = PlayerPrefs.GetString("loginUserPass");
        //}
        timer = 0;
        loading = false;
    }
    private void Update()
    {
        if (loading)
        {
            if (timer>5)
            {
                timer = 0;
                panelLoading.SetActive(false);
                loading = false;
                StopAllCoroutines();

            }
            timer += Time.deltaTime;
        }
    }
    public void Awake()
    {
        userInfo = FindObjectOfType<UserInformationScript>();
    }


    IEnumerator LogIn()
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", txtID.text);
        form.AddField("UserPassword", txtPassword.text);

        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName+"/LogIn.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);

            }
            else
            {
                ProcessJsonData(www.downloadHandler.text);
            }
        }

        StopCoroutine(LogIn());
    }

    IEnumerator UpdateImei(string status)
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", txtID.text);
        form.AddField("UserImei", status);

        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName+"/UpdateStatusLogIn.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                errorMessage.text = "Failed";
                Debug.Log(www.error);

            }
            else
            {
                PlayerPrefs.SetInt("UserID", userInfo._UserID);
                PlayerPrefs.SetString("UserName", userInfo._UserName);
                PlayerPrefs.SetString("UserClass", userInfo._UserClass);
                PlayerPrefs.SetString("UserMajors", userInfo._UserMajors);
                errorMessage.text = "Success";
                //StartCoroutine(GetScore());
                SceneManager.LoadScene("MainMenu");

            }
        }
    }

    //IEnumerator GetScore()
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("UserID", userInfo._UserID);

    //    using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName+"/GetScoreColorDot.php", form))
    //    {

    //        yield return www.SendWebRequest();
    //        if (www.isNetworkError || www.isHttpError)
    //        {
    //            Debug.Log(www.error);

    //        }
    //        else
    //        {
    //            ProcessJsonDataScore(www.downloadHandler.text);
    //        }
    //    }
    //    SceneManager.LoadScene("MainMenu");
    //    StopCoroutine(GetScore());
    //}
    public void LogInBtn()
    {
        if (txtID.text == "" || txtPassword.text == "")
        {
            errorMessage.text = "Please Input User ID or Password";
        }
        else
        {
            if (NetworkConnection.CheckConnection())
            {
                panelLoading.SetActive(true);
                StartCoroutine(LogIn());
                loading = true;
            }
            else
            {
                popUpError.SetActive(true);
                LeanTween.scale(popUpError.transform.GetChild(0).gameObject, Vector2.one, .5f).setEase(LeanTweenType.easeInOutQuart);
            }
            //if (toggleRemember.isOn)
            //{
            //    PlayerPrefs.SetString("loginUserID",txtID.text);
            //    PlayerPrefs.SetString("loginUserpass",txtPassword.text);
            //}

        }

    }

    public void Retry()
    {
        LeanTween.scale(popUpError.transform.GetChild(0).gameObject, Vector2.zero, .5f).setEase(LeanTweenType.easeInSine).setOnComplete(retry);
    }
    public void Exit()
    {
        Application.Quit();
    }
    void retry()
    {
        popUpError.SetActive(false);
        LogInBtn();

    }
    void ProcessJsonData(string url)
    {

        ClassUser user = JsonUtility.FromJson<ClassUser>(url);

        userInfo._UserID = user.UserID;
        userInfo._UserName = user.UserName;
        userInfo._UserClass = user.UserClass;
        userInfo._UserMajors = user.UserMajors;
        if (userInfo._UserName == "")
        {
            panelLoading.SetActive(false);
            errorMessage.text = "Wrong Password or User ID";
        }
        else
        {
            StartCoroutine(UpdateImei(SystemInfo.deviceUniqueIdentifier));
        }


    }

    //void ProcessJsonDataScore(string url)
    //{

    //    ClassScoreGame user = JsonUtility.FromJson<ClassScoreGame>(url);
    //    //userInfo._HighScoreColorDot = user.ScoreGame;
    //    PlayerPrefs.SetInt("ColorDotHighScore", user.ScoreGame);
    //    Debug.Log("User Score" + user.ScoreGame);
    //}

    #region UserInterface
    public void OnSelectInputUserID()
    {
        LeanTween.scaleX(userInputID, 1.1f, .05f).setEase(LeanTweenType.easeOutCubic).setOnComplete(ScaleYID);
    }
    void ScaleYID()
    {
        LeanTween.scaleY(userInputID, 1.1f, .01f).setEase(LeanTweenType.easeOutCubic);
    }
    public void OnDeSelectInputUserID()
    {
        LeanTween.scaleX(userInputID, 1, .05f).setEase(LeanTweenType.easeOutCubic).setOnComplete(ScaleInYID);
    }
    void ScaleInYID()
    {
        LeanTween.scaleY(userInputID, 1, .05f).setEase(LeanTweenType.easeOutCubic);
    }

    public void OnSelectInputUserPass()
    {
        LeanTween.scaleX(userInputPass, 1.1f, .05f).setEase(LeanTweenType.easeInOutBounce).setOnComplete(ScaleYPass);
    }
    void ScaleYPass()
    {
        LeanTween.scaleY(userInputPass, 1.1f, .01f).setEase(LeanTweenType.easeOutBounce);
    }
    public void OnDeSelectInputUserPass()
    {
        LeanTween.scaleX(userInputPass, 1, .1f).setEase(LeanTweenType.easeInOutBounce).setOnComplete(ScaleInYPass);
    }
    void ScaleInYPass()
    {
        LeanTween.scaleY(userInputPass, 1, .1f).setEase(LeanTweenType.easeOutBounce);
    }
    #endregion
}
