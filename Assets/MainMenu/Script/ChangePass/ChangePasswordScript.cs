using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ChangePasswordScript : MonoBehaviour
{
    [SerializeField] TMP_InputField txtCurrentPass;
    [SerializeField] TMP_InputField txtNewPass;
    [SerializeField] TMP_InputField txtConfirmPass;

    [SerializeField] TextMeshProUGUI txtErorr;
    [SerializeField] Button btnConfirm;
    [SerializeField] GameObject circleLoading;
    UserInformationScript userInfo;
    GameManagerMainMenu gm;
    private void Awake()
    {
        gm = FindObjectOfType<GameManagerMainMenu>();
        userInfo = FindObjectOfType<UserInformationScript>();
    }
    private void Start()
    {
        circleLoading.SetActive(false);
        txtErorr.text = "";
    }
    public void ChangePassword()
    {
        txtErorr.text = "";
        if (txtConfirmPass.text == "" || txtCurrentPass.text == "" || txtNewPass.text == "")
        {
            txtErorr.text = "Please Input your Passsword";
            txtErorr.color = Color.red;
        }
        else
        {
            if (txtNewPass.text != txtConfirmPass.text)
            {
                txtErorr.text = "Make Sure you confirm Password is similar with new Password";
                txtErorr.color = Color.red;
            }
            else
            {
                gm.isOpenPanel = false;
                circleLoading.SetActive(true);
                btnConfirm.interactable = false;
                StartCoroutine(ChangePass(userInfo._UserID, txtCurrentPass.text, txtConfirmPass.text));
            }
        }

    }

    IEnumerator ChangePass(int UserID,string current,string newPass)
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("UserPassword", current);
        form.AddField("UserNewPass", newPass);

        using (UnityWebRequest www = UnityWebRequest.Post(ServerGame.serverName + "/Update.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error "+ www.error);
                txtErorr.text = www.downloadHandler.text;
                txtErorr.color = Color.red;
            }
            else
            {
                if (www.downloadHandler.text=="1")
                {
                    txtErorr.text = "Update Successfull";
                    txtErorr.color = Color.green;
                }
                else
                {
                    txtErorr.text = "Make Sure your current Password is Correct";
                    txtErorr.color = Color.red;
                }

                Debug.Log("No Error "+www.downloadHandler.text);
            }
        }

        txtConfirmPass.text = "";
        txtCurrentPass.text = "";
        txtNewPass.text = "";
        circleLoading.SetActive(false);
        yield return new WaitForSeconds(1f);
        txtErorr.text = "";
        btnConfirm.interactable = true;
        gm.isOpenPanel = true;
    }
}
