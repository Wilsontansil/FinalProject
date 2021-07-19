using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EventParent : MonoBehaviour
{
    public int EventCount=0;
    public GameObject dontShow;


    public void CheckEvent()
    {
        if (EventCount == 0)
        {
            gameObject.SetActive(false);
            if (dontShow.GetComponent<Toggle>().isOn)
            {
                PlayerPrefs.SetInt("DontShow", 1);
            }
            else
            {
                PlayerPrefs.SetInt("DontShow", 0);
            }
        }
    }
}
