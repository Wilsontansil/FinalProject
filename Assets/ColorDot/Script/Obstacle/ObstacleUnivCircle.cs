using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleUnivCircle : MonoBehaviour
{
    [SerializeField] GameObject objectUpL;
    [SerializeField] GameObject objectUpR;
    [SerializeField] GameObject objectDownL;
    [SerializeField] GameObject objectDownR;
    GameManagerColorDot GMColorDot;
    float speed = 10;
    private void Awake()
    {
        GMColorDot = FindObjectOfType<GameManagerColorDot>();
    }
    private void Start()
    {
        SetColor(Color.Lerp(Color.gray,Color.blue,.3f), Color.Lerp(Color.gray, Color.blue, .3f), Color.Lerp(Color.gray, Color.blue, .3f), Color.Lerp(Color.gray, Color.blue, .3f));
    }
    private void Update()
    {
        transform.Rotate(0, 0, speed * 5 * Time.deltaTime);
        SetColor(Color.Lerp(Color.gray, Color.blue, .3f), Color.Lerp(Color.gray, Color.blue, .3f), Color.Lerp(Color.gray, Color.blue, .3f), Color.Lerp(Color.gray, Color.blue, .3f));
    }
    void SetColor(Color UpL, Color UpR, Color DownL, Color DownR)
    {
        objectUpL.GetComponent<SpriteRenderer>().color = UpL;
        objectUpL.name = UpL.ToString();
        objectUpR.GetComponent<SpriteRenderer>().color = UpR;
        objectUpR.name = UpR.ToString();
        objectDownL.GetComponent<SpriteRenderer>().color = DownL;
        objectDownL.name = DownL.ToString();
        objectDownR.GetComponent<SpriteRenderer>().color = DownR;
        objectDownR.name = DownR.ToString();
    }



}

