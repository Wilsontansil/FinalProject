using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    float speed =5;
    GameManagerDotRun GM;
    //public SpriteRenderer sr;
    //float timeSpan = 1;
    //float time;

    private void Awake()
    {
        GM = FindObjectOfType<GameManagerDotRun>();
        //sr = GetComponent<SpriteRenderer>();
    }
    //private void Start()
    //{
    //    //ChangeColorGameObject();
    //}
    void Update()
    {
        if (GM.gameState == GameStateDotRun.inPlayGame)
        {
            //if (time > timeSpan)
            //{
            //    //ChangeColorGameObject();
            //    time = 0;
            //}
            //time += Time.deltaTime;
            transform.Rotate(0, 0, speed * 5 * Time.deltaTime);
        }

    }

    //void ChangeColorGameObject()
    //{
    //    int rand = Random.Range(0, GM.colorGame.Count);
    //    while (GM.colorGame[rand] != sr.color)
    //    {
    //        sr.color = Color.Lerp(sr.color, GM.colorGame[rand], .5f);
    //    }
    //}
}
