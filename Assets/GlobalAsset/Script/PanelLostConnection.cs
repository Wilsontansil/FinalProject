using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PanelLostConnection : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMessage;
    public Button btnOk;
    public Button btnCancel;
    //[SerializeField] TextMeshProUGUI textOk;
    //[SerializeField] TextMeshProUGUI textCancel;

    public void SetMessage(string message, string ok , string cancel)
    {
        textMessage.text = message;
        btnOk.GetComponentInChildren<TextMeshProUGUI>().text = ok;
        btnCancel.GetComponentInChildren<TextMeshProUGUI>().text = cancel;
    }
}
