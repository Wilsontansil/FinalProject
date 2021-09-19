using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum State
{
    checkConnection,
    connectionReady,
    downlaodFile,
    prepare,
    finish
}
public class LoadingManager : MonoBehaviour
{
    UserInformationScript userInfo;
    [SerializeField] TextMeshProUGUI txtLoaading;
    [SerializeField] TextMeshProUGUI txtPercentage;
    [SerializeField] Slider sliderLoading;
    [SerializeField] GameObject popUpError;
    //[SerializeField] string userImeiSmartPhone;
    //[SerializeField] string userImeiServer;

    public State state;
    private void Awake()
    {
        userInfo = FindObjectOfType<UserInformationScript>();
        sliderLoading.value = 0;
        txtLoaading.text = "Checking Connection...";
        state = State.checkConnection;
        CheckLogIN();

    }

    //bool CheckConenction()
    //{
    //    bool connect = false;
    //    if (Application.internetReachability == NetworkReachability.NotReachable)
    //    {
    //        connect = false;
    //        Debug.Log("Error. Check internet connection!");
    //    }
    //    else
    //    {
    //        connect = true;
    //    }
    //    return connect;
    //}
    private void Update()
    {
        txtPercentage.text = Mathf.Round(sliderLoading.value * 100) + "%";
        if (state == State.connectionReady)
        {
            if (sliderLoading.value < .3f)
            {
                sliderLoading.value += Time.deltaTime;
            }
        }
        else if (state == State.downlaodFile)
        {
            if (sliderLoading.value >= .3f && sliderLoading.value <= .5f)
            {
                sliderLoading.value += Time.deltaTime;
            }
        }
        else if (state == State.prepare)
        {
            sliderLoading.value += Time.deltaTime;
        }

        if (sliderLoading.value >= 1)
        {
            state = State.finish;
            sliderLoading.value = 1;
            txtPercentage.text =  "100%";
            GetConnection();
        }
    }
    private void Start()
    {
        StartCoroutine(NetworkConnection.CheckImei(PlayerPrefs.GetInt("UserID")));
        StartCoroutine(CoundownDonnection());
        Debug.Log(PlayerPrefs.GetInt("UserID") + " " + CheckLogIN());
    }
    IEnumerator CoundownDonnection()
    {
        yield return new WaitForSeconds(1f);
        if (NetworkConnection.CheckConnection())
        {
            state = State.connectionReady;
            txtLoaading.text = "Connection is ready...";
            yield return new WaitForSeconds(1f);
            StartCoroutine(DownloadAsset());
        }
        else
        {
            LeanTween.scale(popUpError, Vector2.one, .5f).setEase(LeanTweenType.easeInOutCirc);
            //popUpError.GetComponent<PanelLostConnection>().OpenMessage();
        }
    }
    IEnumerator DownloadAsset()
    {
        yield return new WaitForSeconds(1f);
        state = State.downlaodFile;
        txtLoaading.text = "Downloading File...";
        yield return new WaitForSeconds(2f);

        if (NetworkConnection.CheckConnection())
        {
            state = State.prepare;
            txtLoaading.text = "Preparing Games...";
        }
        else
        {
            LeanTween.scale(popUpError, Vector2.one, .5f).setEase(LeanTweenType.easeInOutCirc);
            //popUpError.GetComponent<PanelLostConnection>().OpenMessage();
        }

    }
    void GetConnection()
    {
        if (NetworkConnection.CheckConnection())
        {
            txtLoaading.text = "Time To Play";
            if (userInfo.imei != NetworkConnection.ImeiServer || PlayerPrefs.GetString("UserName") == "")
            {
                SceneManager.LoadScene("LogIn");
            }
            else
            {
                userInfo._UserID = PlayerPrefs.GetInt("UserID");
                userInfo._UserName = PlayerPrefs.GetString("UserName");
                userInfo._UserClass = PlayerPrefs.GetString("UserClass");
                userInfo._UserMajors = PlayerPrefs.GetString("UserMajors");
                PlayerPrefs.SetInt("ColorDotHighScore", 0);
                PlayerPrefs.SetInt("DotRunHighScore", 0);
                PlayerPrefs.SetInt("MatchFruitHighScore", 0);
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
    public void Retry()
    {
        StopAllCoroutines();
        LeanTween.scale(popUpError, Vector2.zero, .5f).setEase(LeanTweenType.easeInOutCirc);
        txtLoaading.text = "Checking Connection...";
        state = State.checkConnection;
        StartCoroutine(CoundownDonnection());
    }
    string CheckLogIN()
    {
        string imei = SystemInfo.deviceUniqueIdentifier;
        userInfo.imei = imei;
        return imei;
    }
    public void QuitApp()
    {
        Application.Quit();
    }


}
