using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

//enum PanelState{
//    mainPanel,
//    panelSetting,
//    panelLeaderboard,
//    panelUserId,
//    panelChangePass,
//    panelLoading,
//    panelEvent
//}

enum GameState {
    ColorDot,
    DotRun,
    Match3,
    DoubleDot,
    Hit3
}

public class GameManagerMainMenu : MonoBehaviour
{
    UserInformationScript userInfo;
    //EventCardPanel eventCardPanel;
    [Header("User Information")]
    [SerializeField] TextMeshProUGUI txtUserName;
    [SerializeField] TextMeshProUGUI txtUserClass;
    [SerializeField] TextMeshProUGUI txtUserClassIDCard;
    [SerializeField] TextMeshProUGUI txtUserMajors;
    [SerializeField] TextMeshProUGUI txtUserInitial;

    [Header("Event Manager")]
    [SerializeField] GameObject EventCard;
    [SerializeField]List<string> linkImageEvent;
    [SerializeField]List<string> eventText;
    [SerializeField] List<string> eventTitle;
    [SerializeField] List<string> linkEvent;
    //[SerializeField]List<Sprite> imageEvent;
    [SerializeField] Transform eventCardParent;
    [SerializeField] GameObject eventCardPref;
    //bool finishRetrieveData;
    [SerializeField] Transform Eventparent;

    [Header("Game Panel")]
    //List<GameObject> listPanel;
    //PanelState panelState;
    GameState gameState;
    [SerializeField] GameObject panelMain;
    [SerializeField] GameObject panelSetting;
    [SerializeField] GameObject panelLeaderboard;
    [SerializeField] GameObject panelUserId;
    [SerializeField] GameObject panelLoading;
    [SerializeField] GameObject panelPopUpError;
    [SerializeField] GameObject panelChangePass;
    [SerializeField] GameObject panelEvent;
    [SerializeField] GameObject panelChat;

    [Header("Sound")]
    [SerializeField] AudioSource backSoundAudio;
    [SerializeField] AudioClip click;

    [Header("Toggle Settting")]
    [SerializeField] ToggleSetting toggleSound;

    [Header("ManagerUI")]
    public GameObject userNameUI;
    public GameObject settingButton;
    public GameObject buttonColorDot;
    public GameObject buttonDotRun;
    public GameObject buttonMatchFruit;
    public GameObject buttonDotDouble;
    public GameObject buttonHit3;
    public GameObject buttonEvent;
    public GameObject buttonOff;
    public GameObject buttonChat;
    public Transform buttonSettingParent;
    public Transform buttonDotDoubleParent;
    public Transform buttonHit3Parent;
    public Transform buttonEventParent;
    public Transform buttonChatParent;
    [Header("LeaderCard")]
    [SerializeField] GameObject cardLeaderBoard;
    [SerializeField] GameObject parentCard;
    [SerializeField] GameObject loadingCircle;
    public bool isOpenPanel;
    private void Awake()
    {
        userInfo = FindObjectOfType<UserInformationScript>();
        //SetAllPanel();
        linkImageEvent = new List<string>();
        eventText = new List<string>();
        eventTitle = new List<string>();
        linkEvent = new List<string>();
        //imageEvent = new List<Sprite>();
        //finishRetrieveData = false;
        //eventCardPanel = FindObjectOfType<EventCardPanel>();
    }
    private void Start()
    {
        //panelState = PanelState.mainPanel;
        OnLeanUserName();
        OnLeanSetting();
        OnleanbuttonColorDot();
        OnleanbuttonDotRun();
        OnleanbuttonMatchFruit();
        OnleanbuttonDotDouble();
        OnleanbuttonHit3();
        OnleanbuttonEvent();
        OnleanbuttonOff();
        OnleanbuttonChat();
        SetUserInfo();
        //if (PlayerPrefs.GetInt("DontShow")==0)
        //{
        StartCoroutine(GetImageLink());
        //}
        //else
        //{
        //    Eventparent.GetComponent<EventParent>().CheckEvent();
        //}
        isOpenPanel = false;
    }
    void SetUserInfo()
    {
        txtUserName.text = "Hi, " + userInfo._UserName;
        txtUserClass.text = userInfo._UserClass;
        txtUserClassIDCard.text = userInfo._UserClass;
        txtUserMajors.text = userInfo._UserMajors;
        txtUserInitial.text = GetInitialName();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("get panel closeed");
            if (isOpenPanel)
            {
                Debug.Log("is open " + isOpenPanel);
                CheckOpenedPanel();
                isOpenPanel = false;
            }

        }
        //if (Eventparent.transform.childCount>0)
        //{
        //    if (CheckLoadingEvent())
        //    {
        //        loadingEvent.SetActive(false);
        //    }
        //    else
        //    {
        //        loadingEvent.SetActive(true);
        //    }
        //}
        //else if(finishRetrieveData)
        //{
        //    loadingEvent.SetActive(false);
        //}
    }
    //bool CheckLoadingEvent()
    //{
    //    bool result = false;
    //    for (int i = 0; i < Eventparent.transform.childCount; i++)
    //    {
    //        if (Eventparent.transform.GetChild(i).GetComponent<EventCardPref>().finishTakeImage)
    //        {
    //            result = true;
    //        }
    //        else
    //        {
    //            result = false;
    //            break;
    //        }

    //    }
    //    return result;
    //}
    string GetInitialName()
    {
        string init = "";
        char[] chars = userInfo._UserName.ToCharArray();

        init = chars[0].ToString();

        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i].ToString() == " ")
            {
                init += chars[i + 1];
            }
        }
        return init;
    }
    #region panelSetting
    //void SetAllPanel()
    //{
    //    listPanel = new List<GameObject>();
    //    listPanel.Add(panelMain);
    //    listPanel.Add(panelSetting);
    //}
    //bool MainMenePanelSetting()
    //{
    //    if (listPanel.TrueForAll(X => X.activeInHierarchy == false))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    public void CheckOpenedPanel()
    {
        if (panelChat.activeInHierarchy)
        {
            OffPressLeanPanel(panelChat);
        }else if (panelEvent.activeInHierarchy)
        {
            OffPressLeanPanel(panelEvent);
        }
        else if (panelChangePass.activeInHierarchy)
        {
            OffPressLeanPanel(panelChangePass);
        }
        else if (panelUserId.activeInHierarchy)
        {
            OffPressLeanPanel(panelUserId);
        }
        else if (panelLeaderboard.activeInHierarchy)
        {
            OffPressLeanPanel(panelLeaderboard);
        }
        else if (panelSetting.activeInHierarchy)
        {
            OffPressLeanPanel(panelSetting);
        }
        //if (panelState == PanelState.panelEvent)
        //{
        //    OffPressLeanPanel(panelEvent);
        //}
        //else if (panelState == PanelState.panelChangePass)
        //{
        //    OffPressLeanPanel(panelChangePass);
        //}
        //else if (panelState == PanelState.panelUserId)
        //{
        //    OffPressLeanPanel(panelUserId);
        //}
        //else if (panelState == PanelState.panelLeaderboard)
        //{
        //    OffPressLeanPanel(panelLeaderboard);
        //}
        //else if (panelState == PanelState.panelSetting)
        //{
        //    OffPressLeanPanel(panelSetting);
        //}
        //panelState = PanelState.mainPanel;
        //if (panelSetting.activeInHierarchy)
        //{
        //    OffPressLeanPanelSetting();
        //}
        //if (MainMenePanelSetting())
        //{
        //    panelMain.SetActive(true);
        //}
    }

    //void OpenPanelSetting()
    //{
    //    foreach (GameObject item in listPanel)
    //    {
    //        item.SetActive(false);
    //    }
    //    panelSetting.SetActive(true);
    //}
    //void CloseSetting()
    //{
    //    panelSetting.SetActive(false);
    //}
    #endregion
    #region AnimationLean

    //public void OnPressLeanPanelSetting()
    //{
    //    ////LeanTween.scale(settingButton, new Vector3(.8f, .8f, .8f), .1f).setEase(LeanTweenType.easeSpring).setOnComplete(OnCompleteLeanPanelSetting);\
    //    OnLeanOpenPanelSetting();
    //}
    void OffPressLeanPanel(GameObject panel)
    {
        LeanTween.moveLocalY(panel, 700, .5f).setEase(LeanTweenType.easeInCubic).setOnComplete(()=> panel.SetActive(false));

    }
    //void OffPressLeanPanelLeaderboard()
    //{
    //    LeanTween.moveLocalY(panelLeaderboard, 700, .5f).setEase(LeanTweenType.easeInCubic);
    //}
    //void OffPressLeanPanelUserId()
    //{
    //    LeanTween.moveLocalY(panelUserId, 700, .5f).setEase(LeanTweenType.easeInCubic);
    //}
    //void OnCompleteLeanPanelSetting()
    //{
    //    LeanTween.scale(settingButton, Vector3.one, .1f).setEase(LeanTweenType.easeSpring).setOnComplete(OnLeanOpenPanelSetting);
    //}
    public void OnLeanOpenPanelChangePass()
    {
        //OpenPanelSetting();
        isOpenPanel = false;
        panelChangePass.SetActive(true);
        //panelState = PanelState.panelChangePass;
        LeanTween.moveLocalY(panelChangePass, 0, .8f).setEase(LeanTweenType.easeOutCubic).setOnComplete(FinishSetPassPanel);

    }
    void FinishSetPassPanel()
    {
        OffPressLeanPanel(panelSetting);
        isOpenPanel = true;
    }
    public void OnLeanOpenPanelSetting()
    {
        //CheckOpenedPanel();
        //OpenPanelSetting();
        //panelState = PanelState.panelSetting;
        panelSetting.SetActive(true);
        LeanTween.moveLocalY(panelSetting, 0, .8f).setEase(LeanTweenType.easeOutCubic).setOnComplete(()=>isOpenPanel=true);
    }
    public void OnLeanOpenPanelChat()
    {
        //CheckOpenedPanel();
        //OpenPanelSetting();
        //panelState = PanelState.panelSetting;
        panelChat.SetActive(true);
        LeanTween.moveLocalY(panelChat, 0, .8f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => isOpenPanel = true);
    }
    void OnLeanOpenPanelLeaderboard()
    {
        //CheckOpenedPanel();
        panelLeaderboard.SetActive(true);
        //panelState = PanelState.panelLeaderboard;
        LeanTween.moveLocalY(panelLeaderboard, 0, .8f).setDelay(.1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(()=>isOpenPanel=true);
    }
    public void OnLeanOpenPanelUserID()
    {
        //CheckOpenedPanel();
        panelUserId.SetActive(true);
        //panelState = PanelState.panelUserId;
        LeanTween.moveLocalY(panelUserId, 0, .8f).setDelay(.1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(()=>isOpenPanel=true);
    }
    public void OnLeanOpenPanelLoading()
    {
        //CheckOpenedPanel();
        panelLoading.SetActive(true);
        //panelState = PanelState.panelLoading;
        LeanTween.moveLocalY(panelLoading, 0, .8f).setDelay(.1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(()=>isOpenPanel=true);
    }
    public void OnLeanOpenEventPanel()
    {
        CheckOpenedPanel();
        panelEvent.SetActive(true);
        StartCoroutine(GetImageLink());
        PanelEventOpen();
    }
    void PanelEventOpen()
    {
        if (eventCardParent.childCount != 0)
        {
            foreach (Transform item in eventCardParent)
            {
                Destroy(item.gameObject);
            }
        }
        for (int i = 0; i < linkImageEvent.Count; i++)
        {
            InstantiateEventCard(linkImageEvent[i], eventTitle[i],linkEvent[i]);
        }
        //panelState = PanelState.panelEvent;
        LeanTween.moveLocalY(panelEvent, 0, .8f).setDelay(.1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => isOpenPanel = true);
    }
    public void OnPressLeanGameColorDot()
    {
        loadingCircle.SetActive(true);
        ClearCard();
        StartCoroutine(GetLeaderboardColorDot());
        gameState = GameState.ColorDot;
        OnLeanOpenPanelLeaderboard();
        //LeanTween.scale(buttonColorDot, new Vector3(.8f, .8f, .8f), .1f).setEase(LeanTweenType.easeSpring).setOnComplete(OnCompleteLeanGameColorDot);
    }
    //void OnCompleteLeanGameColorDot()
    //{
    //    LeanTween.scale(buttonColorDot, Vector3.one, .1f).setEase(LeanTweenType.easeSpring).setOnComplete(OnLeanOpenPanelLeaderboard);
    //}

    public void OnPressLeanGameDotRun()
    {
        loadingCircle.SetActive(true);
        ClearCard();
        StartCoroutine(GetLeaderboardDotRun());
        gameState = GameState.DotRun;
        OnLeanOpenPanelLeaderboard();
        //LeanTween.scale(buttonDotRun, new Vector3(.8f, .8f, .8f), .1f).setEase(LeanTweenType.easeSpring).setOnComplete(OnCompleteLeanGameDotRun);
    }
    //void OnCompleteLeanGameDotRun()
    //{
    //    LeanTween.scale(buttonDotRun, Vector3.one, .1f).setEase(LeanTweenType.easeSpring).setOnComplete(OnLeanOpenPanelLeaderboard);
    //}

    public void OnPressLeanGameMatchFruit()
    {
        loadingCircle.SetActive(true);
        ClearCard();
        StartCoroutine(GetLeaderboardMatchFruit());
        gameState = GameState.Match3;
        OnLeanOpenPanelLeaderboard();
        //LeanTween.scale(buttonMatchFruit, new Vector3(.8f, .8f, .8f), .1f).setEase(LeanTweenType.easeSpring).setOnComplete(OnCompleteLeanGameMatchFruit);
    }

    public void OnPressLeanGameDoubleDot()
    {
        loadingCircle.SetActive(true);
        ClearCard();
        StartCoroutine(GetLeaderboardDoubleDot());
        gameState = GameState.DoubleDot;
        OnLeanOpenPanelLeaderboard();
        //LeanTween.scale(buttonMatchFruit, new Vector3(.8f, .8f, .8f), .1f).setEase(LeanTweenType.easeSpring).setOnComplete(OnCompleteLeanGameMatchFruit);
    }

    public void OnPressLeanGameHit3()
    {
        loadingCircle.SetActive(true);
        ClearCard();
        StartCoroutine(GetLeaderboardFeedIt());
        gameState = GameState.Hit3;
        OnLeanOpenPanelLeaderboard();
        //LeanTween.scale(buttonMatchFruit, new Vector3(.8f, .8f, .8f), .1f).setEase(LeanTweenType.easeSpring).setOnComplete(OnCompleteLeanGameMatchFruit);
    }
    //void OnCompleteLeanGameMatchFruit()
    //{
    //    LeanTween.scale(buttonMatchFruit, Vector3.one, .1f).setEase(LeanTweenType.easeSpring).setOnComplete(OnLeanOpenPanelLeaderboard);
    //}

    void OnLeanUserName()
    {
        LeanTween.moveX(userNameUI, 10, 1f).setEase(LeanTweenType.easeOutCubic);
    }
    void OnLeanSetting()
    {
        LeanTween.move(settingButton, buttonSettingParent, 1.5f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.alpha(settingButton.GetComponent<RectTransform>(), 1, 1);
    }
    void OnleanbuttonColorDot()
    {
        LeanTween.moveLocalY(buttonColorDot, -70, 1f).setEase(LeanTweenType.easeSpring);
    }
    void OnleanbuttonDotRun()
    {
        LeanTween.moveLocalY(buttonDotRun, -80, 1f).setEase(LeanTweenType.easeSpring).setDelay(.15f);
    }
    void OnleanbuttonMatchFruit()
    {
        LeanTween.moveLocalY(buttonMatchFruit, -190, 1f).setEase(LeanTweenType.easeSpring).setDelay(.2f);
    }
    void OnleanbuttonDotDouble()
    {
        LeanTween.move(buttonDotDouble, buttonDotDoubleParent, 1f).setEase(LeanTweenType.easeSpring).setDelay(.3f);
    }
    void OnleanbuttonHit3()
    {
        LeanTween.move(buttonHit3, buttonHit3Parent, 1f).setEase(LeanTweenType.easeSpring).setDelay(.4f);
    }
    void OnleanbuttonEvent()
    {
        LeanTween.move(buttonEvent, buttonEventParent, 1f).setEase(LeanTweenType.easeOutExpo).setDelay(.6f);
    }
    void OnleanbuttonOff()
    {
        LeanTween.scale(buttonOff, Vector2.one, .5f).setEase(LeanTweenType.easeOutQuad).setDelay(.6f);
    }
    void OnleanbuttonChat()
    {
        LeanTween.move(buttonChat, buttonChatParent, .5f).setEase(LeanTweenType.easeOutQuad).setDelay(.6f);
    }
    #endregion
    public void LogOut()
    {
        StartCoroutine(UpdateStatusLogIn(0));
    }
    IEnumerator UpdateStatusLogIn(int status)
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", userInfo._UserID);
        form.AddField("UserImei", status);

        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName+"/UpdateStatusLogIn.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);

            }
            else
            {
                CheckOpenedPanel();
                Debug.Log("Sucess Log LogOut");
                PlayerPrefs.DeleteAll();
                Application.Quit();
            }
        }
        StopAllCoroutines();
    }

    IEnumerator GetLeaderboardColorDot()
    {
        UnityWebRequest www = UnityWebRequest.Get(ServerGame.serverName+ "/GetLeaderBoardColorDot.php");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.LogError("Errror Connection");
            StartCoroutine(GetLeaderboardColorDot());
        }
        else
        {
            if (www.isDone)
            {
                Debug.Log(www.downloadHandler.text);
                ProcessJsonData(www.downloadHandler.text);
                StopCoroutine(GetLeaderboardColorDot());
            }
        }

    }

    IEnumerator GetLeaderboardDotRun()
    {
        UnityWebRequest www = UnityWebRequest.Get(ServerGame.serverName+"/GetLeaderBoardDotRun.php");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.LogError("Errror Connection");
            StartCoroutine(GetLeaderboardDotRun());
        }
        else
        {
            if (www.isDone)
            {
                Debug.Log(www.downloadHandler.text);
                ProcessJsonData(www.downloadHandler.text);
                StopCoroutine(GetLeaderboardColorDot());
            }
        }

    }

    IEnumerator GetLeaderboardMatchFruit()
    {
        UnityWebRequest www = UnityWebRequest.Get(ServerGame.serverName+"/GetLeaderBoardMatchFruit.php");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.LogError("Errror Connection");
            StartCoroutine(GetLeaderboardMatchFruit());
        }
        else
        {
            if (www.isDone)
            {
                ProcessJsonData(www.downloadHandler.text);
                StopCoroutine(GetLeaderboardMatchFruit());
            }
        }

    }
    IEnumerator GetLeaderboardDoubleDot()
    {
        UnityWebRequest www = UnityWebRequest.Get(ServerGame.serverName + "/GetLeaderBoardDoubleDot.php");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.LogError("Errror Connection");
            StartCoroutine(GetLeaderboardDoubleDot());
        }
        else
        {
            if (www.isDone)
            {
                ProcessJsonData(www.downloadHandler.text);
                StopCoroutine(GetLeaderboardDoubleDot());
            }
        }

    }

    IEnumerator GetLeaderboardFeedIt()
    {
        UnityWebRequest www = UnityWebRequest.Get(ServerGame.serverName + "/GetLeaderboardFeedIt.php");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.LogError("Errror Connection");
            StartCoroutine(GetLeaderboardFeedIt());
        }
        else
        {
            if (www.isDone)
            {
                ProcessJsonData(www.downloadHandler.text);
                StopCoroutine(GetLeaderboardFeedIt());
            }
        }

    }
    IEnumerator GetImageLink()
    {
        UnityWebRequest www = UnityWebRequest.Get(ServerGame.serverName+"/GetImageEvent.php");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.LogError("Errror Connection");
            StartCoroutine(GetImageLink());
        }
        else
        {
            if (www.isDone)
            {
                Debug.Log(www.downloadHandler.text);
                ProcessJsonDataEvent(www.downloadHandler.text);
                StopCoroutine(GetImageLink());
            }
        }
        //for (int i = 0; i < linkEvent.Count; i++)
        //{
        //    StartCoroutine(GetImageEvent(linkEvent[i]));
        //}
        if (linkImageEvent.Count==0)
        {
            Eventparent.GetComponent<EventParent>().CheckEvent();
        }
        if (PlayerPrefs.GetInt("DontShow")==0)
        {
            StartCoroutine(InstantiateEventPopUp());
        }
        else
        {
            Eventparent.GetComponent<EventParent>().CheckEvent();
        }

        //StartCoroutine(setImage());



    }
    //IEnumerator setImage()
    //{
    //    yield return new WaitForSeconds(1f);
    //    for (int i = 0; i < linkEvent.Count; i++)
    //    {
    //        InstantiateEventCard(imageEvent[i], eventTitle[i]);
    //    }
    //}
    IEnumerator InstantiateEventPopUp()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < linkImageEvent.Count; i++)
        {
            Debug.LogError(linkImageEvent[i]);
            InstantiateEventPopUp(linkImageEvent[i],eventText[i],eventTitle[i],linkEvent[i]);
        }
        Eventparent.GetComponent<EventParent>().dontShow.SetActive(true);
    }
    void ProcessJsonData(string url)
    {
        int x = 1;
        listClassUserLeaderboard listUser = JsonUtility.FromJson<listClassUserLeaderboard>(url);
        foreach (ScoreLeaderboard item in listUser.listUser)
        {
            Debug.Log(item.UserName+ " " + item.ScoreGame);
            InstantiiateLeaderBoard(x,item.UserName, item.ScoreGame,item.ClassName);
            if (item.UserName == userInfo._UserName)
            {
                userInfo.highScore = item.ScoreGame;
            }
            x += 1;
        }
        loadingCircle.SetActive(false);
    }
    void ProcessJsonDataEvent(string url)
    {
        linkImageEvent.Clear();
        eventTitle.Clear();
        eventText.Clear();
        listEventCard listCard = JsonUtility.FromJson<listEventCard>(url);
        foreach ( EventCard item in listCard.ListImage)
        {
            linkImageEvent.Add(ServerGame.serverName+"/ImageUpload/" + item.ImageLink);
            eventText.Add(item.ImageText);
            eventTitle.Add(item.ImageTitle);
            linkEvent.Add(item.LinkEvent);
        }
    }
    //public IEnumerator GetImageEvent(string url)
    //{
    //    using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
    //    {
    //        //Debug.LogError("https://musa7098.000webhostapp.com/ImageUpload/" + url);
    //        yield return unityWebRequest.SendWebRequest();
    //        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
    //        {
    //            Debug.LogError("ERROR");
    //        }
    //        else
    //        {
    //            DownloadHandlerTexture dhd = unityWebRequest.downloadHandler as DownloadHandlerTexture;
    //            Texture2D texture = dhd.texture;
    //            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector3(.5f, .5f));
    //            imageEvent.Add(sprite);
    //        }
    //    }
    //}
    void InstantiateEventPopUp(string url,string text,string title,string link)
    {
        GameObject gb = Instantiate(EventCard);
        gb.transform.SetParent(Eventparent);
        Eventparent.GetComponentInParent<EventParent>().EventCount += 1;
        gb.transform.localPosition = Vector3.zero;
        gb.transform.localScale = Vector3.one;
        gb.GetComponent<EventCardPref>().StartCoroutine(gb.GetComponent<EventCardPref>().GetImageEvent(url));
        gb.GetComponent<EventCardPref>().txtEvent.text = text;
        gb.GetComponent<EventCardPref>().txtTitle.text = title;
        if (link == "0")
        {
            gb.GetComponent<EventCardPref>().SetLink(false, link);
        }
        else
        {
            gb.GetComponent<EventCardPref>().SetLink(true, link);
        }

    }
    void InstantiateEventCard(string url,string title,string link)
    {
        GameObject gb = Instantiate(eventCardPref);
        gb.transform.SetParent(eventCardParent.transform);
        gb.GetComponent<EventCardPref>().StartCoroutine(gb.GetComponent<EventCardPref>().GetImageEvent(url));
        gb.GetComponent<EventCardPref>().txtTitle.text = title;
        gb.transform.localScale = Vector3.one;
        if (link == "0")
        {
            gb.GetComponent<EventCardPref>().SetLink(false, link);
        }
        else
        {
            gb.GetComponent<EventCardPref>().SetLink(true, link);
        }
    }
    void InstantiiateLeaderBoard(int rank,string userName,int score,string className)
    {
        GameObject gb = Instantiate(cardLeaderBoard);
        gb.transform.SetParent(parentCard.transform);
        gb.transform.position = Vector3.zero;
        gb.transform.localScale = Vector3.one;
        gb.GetComponent<CardPrefab>().txtUserName.text = userName;
        gb.GetComponent<CardPrefab>().txtClassName.text = className;
        gb.GetComponent<CardPrefab>().txtRank.text = rank.ToString();
        gb.GetComponent<CardPrefab>().txtScoreGame.text = score.ToString();
        if (rank == 1)
        {
            gb.GetComponent<Image>().color = Color.yellow;
        }
        else if(rank == 2)
        {
            gb.GetComponent<Image>().color = Color.green;
        }


    }

    void ClearCard()
    {
        foreach (Transform item in parentCard.transform)
        {
            Destroy(item.gameObject);
        }
    }
    #region SceneManagement
    public void StartGame()
    {
        OnLeanOpenPanelLoading();
        if (!NetworkConnection.CheckConnection())
        {
            StartCoroutine(CheckConnection());
        }
        else
        {
            StartCoroutine(NetworkConnection.CheckImei(userInfo._UserID));
            StartCoroutine(GameStart());
        }

    }
    public void ExitGame()
    {
        panelPopUpError.SetActive(true);
        panelPopUpError.GetComponent<PanelLostConnection>().SetMessage("Are You Sure to Quit the game", "Ok", "Back");
        panelPopUpError.GetComponent<PanelLostConnection>().btnOk.onClick.AddListener(()=>Application.Quit());
        panelPopUpError.GetComponent<PanelLostConnection>().btnCancel.onClick.AddListener(BackExit);
        LeanTween.scale(panelPopUpError.transform.GetChild(0).gameObject, Vector2.one, .5f).setEase(LeanTweenType.easeInOutExpo);
    }
    void BackExit()
    {
        LeanTween.scale(panelPopUpError.transform.GetChild(0).gameObject, Vector2.zero, .5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(()=>panelPopUpError.SetActive(false));
    }
    IEnumerator CheckConnection()
    {
        yield return new WaitForSeconds(2f);
        panelPopUpError.SetActive(true);
        panelPopUpError.GetComponent<PanelLostConnection>().SetMessage("Please Check Your Internet Connection", "Retry", "Quit");
        panelPopUpError.GetComponent<PanelLostConnection>().btnOk.onClick.AddListener(Retry);
        panelPopUpError.GetComponent<PanelLostConnection>().btnCancel.onClick.AddListener(() => Application.Quit());
        LeanTween.scale(panelPopUpError.transform.GetChild(0).gameObject, Vector2.one, .5f).setEase(LeanTweenType.easeInOutExpo);
    }
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2f);
        if (userInfo.imei != NetworkConnection.ImeiServer)
        {
            PlayerPrefs.DeleteAll();
            panelPopUpError.SetActive(true);
            panelPopUpError.GetComponent<PanelLostConnection>().SetMessage("There are another Device log in Using this user ID", "OK", "Quit");
            panelPopUpError.GetComponent<PanelLostConnection>().btnOk.onClick.AddListener(() => SceneManager.LoadScene("LogIn"));
            panelPopUpError.GetComponent<PanelLostConnection>().btnCancel.onClick.AddListener(() => Application.Quit());
            LeanTween.scale(panelPopUpError.transform.GetChild(0).gameObject, Vector2.one, .5f).setEase(LeanTweenType.easeInOutExpo);

        }
        else
        {
            SceneManager.LoadScene(gameState.ToString());
        }

    }
    void Retry()
    {
        LeanTween.scale(panelPopUpError.transform.GetChild(0).gameObject, Vector2.zero, .5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(retry);
    }
    void retry()
    {
        panelPopUpError.SetActive(false);
        StartGame();
    }
    #endregion

    public void SoundSetting()
    {
        if (toggleSound.isOn)
        {
            backSoundAudio.mute = false;
        }
        else
        {
            backSoundAudio.mute = true;
        }
    }

    public void ClickSound()
    {
        LeanAudio.play(click);
    }
    public void ShareInfo()
    {
        StartCoroutine(shareText());
    }
    IEnumerator shareText()
    {
        //yield return animShare;??
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("Subject goes here").SetText("Hi, I am " + userInfo._UserName + " from IT&B Campus " + Environment.NewLine
            + " Majors " + userInfo._UserMajors + Environment.NewLine +
            "https://duckduckgo.com/" + Environment.NewLine +
            "Share to your friend and get more reward")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
        //new NativeShare().SetText("Mainkan GameGG bersama teman anda" + Environment.NewLine
        //    + "untuk download appsnya dan pakai kode referral" + Environment.NewLine + ">>>>>LINK<<<<<");
        yield return new WaitForSeconds(.5f);
        StopCoroutine(shareText());

    }
}
