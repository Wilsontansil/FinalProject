using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class UserInformationScript : MonoBehaviour
{
    public int _UserID;
    public string _UserName;
    public string _UserClass;
    public string _UserMajors;

    [Header("SP Information")]
    public string imei;
    public int highScore;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
