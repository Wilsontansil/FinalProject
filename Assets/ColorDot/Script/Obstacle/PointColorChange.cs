﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointColorChange : MonoBehaviour
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
        SetPointColor(GMColorDot.ColorList[3], GMColorDot.ColorList[0], GMColorDot.ColorList[1], GMColorDot.ColorList[2]);
    }
    private void Update()
    {
        transform.Rotate(0, 0, speed * 10 * Time.deltaTime);
    }
    public void SetPointColor(Color UpL, Color UpR, Color DownL, Color DownR)
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
