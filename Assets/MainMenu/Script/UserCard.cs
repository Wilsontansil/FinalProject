using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class UserCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtUserName;
    [SerializeField] TextMeshProUGUI txtNameMajors;
    [SerializeField] TextMeshProUGUI txtUserClass;
    [SerializeField] TextMeshProUGUI txtMatchPlayedColorDot;
    [SerializeField] TextMeshProUGUI txtMatchPlayedDotRun;
    [SerializeField] TextMeshProUGUI txtMatchPlayedMatchFruit;
    [SerializeField] TextMeshProUGUI txtMatchPlayedDoubleDot;
    [SerializeField] TextMeshProUGUI txtMatchPlayedFeedIt;

    UserInformationScript userInfo;

    private void Awake()
    {
        userInfo = FindObjectOfType<UserInformationScript>();
    }
    private void Start()
    {
        txtUserName.text = userInfo._UserName;
        txtNameMajors.text = userInfo._UserMajors;
        txtUserClass.text = userInfo._UserClass;
        StartCoroutine(GetMatchPlay());
    }

    IEnumerator GetMatchPlay()
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", userInfo._UserID);

        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName + "/GetPlayerMatch.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);

            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                ProcessJsonDataScore(www.downloadHandler.text);
            }
        }
        StopCoroutine(GetMatchPlay());
    }
    void ProcessJsonDataScore(string url)
    {

        MatchPlayed MatchPlayed = JsonUtility.FromJson<MatchPlayed>(url);
        txtMatchPlayedColorDot.text = MatchPlayed.ColorDot.ToString();
        txtMatchPlayedDotRun.text = MatchPlayed.DotRun.ToString();
        txtMatchPlayedMatchFruit.text = MatchPlayed.MatchFruit.ToString();
        txtMatchPlayedDoubleDot.text = MatchPlayed.DoubleDot.ToString();
        txtMatchPlayedFeedIt.text = MatchPlayed.FeedIt.ToString();
    }

}
