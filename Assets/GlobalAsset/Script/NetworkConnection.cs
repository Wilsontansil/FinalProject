using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkConnection : MonoBehaviour
{
    public static string ImeiServer;

    public static bool CheckConnection()
    {
        bool connect = false;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            connect = false;
            Debug.Log("Error. Check internet connection!");
        }
        else
        {
            connect = true;
        }
        return connect;

    }

    public static IEnumerator CheckImei(int ID)
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID",ID);
        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName + "/GetImei.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                //errorMessage.text = "Failed";

                Debug.Log(www.error);

            }
            else
            {
                NetworkConnection.ImeiServer = www.downloadHandler.text;
                Debug.Log(NetworkConnection.ImeiServer);


            }
        }
    }
}
