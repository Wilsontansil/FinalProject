using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleSetting : MonoBehaviour
{
    [SerializeField] Sprite OnSprite;
    [SerializeField] Sprite OffSprite;
    Image ImageButton;
    TextMeshProUGUI txtStatus;
    public bool isOn;

    private void Awake()
    {
        ImageButton = GetComponent<Image>();
        txtStatus = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void PressToggle()
    {
        isOn = !isOn;
        if (isOn)
        {
            ImageButton.sprite = OnSprite;
            txtStatus.text = "ON";
            txtStatus.color = Color.green;
        }
        else
        {
            ImageButton.sprite = OffSprite;
            txtStatus.text = "OFF";
            txtStatus.color = Color.red;
        }
        Debug.Log("Select");
    }

}
