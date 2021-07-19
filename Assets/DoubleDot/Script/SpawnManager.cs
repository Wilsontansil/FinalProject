using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Camera MainCamera;

    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject points;

    float time;
    float maxTime;

    DotDouble GM;


    private void Awake()
    {
        GM = FindObjectOfType<DotDouble>();
        MainCamera = FindObjectOfType<Camera>();
    }
    private void Start()
    {

        maxTime = 2f;
        time = 0;
        //screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));

    }

    private void Update()
    {
        if (GM.state == DotDoubleState.game)
        {
            if (time > maxTime)
            {
                SpawnObstacle();
                time = 0;
            }
            time += Time.deltaTime;
        }


    }

    void SpawnObstacle()
    {
        GameObject parent = new GameObject();
        GameObject point = Instantiate(points);
        point.transform.SetParent(parent.transform);
        point.transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(-3f, -1f));
        GameObject gb = Instantiate(obstacle);
        gb.transform.SetParent(parent.transform);
        parent.transform.SetParent(transform);
        parent.transform.localPosition = Vector3.zero;
        parent.AddComponent<MovingObstacleDoubleDot>();
        //gb.transform.localPosition = new Vector3(-2f, 0, 0);
        if (Random.Range(0, 2) == 0)
        {
            //if (Random.Range(0,2)==0)
            //{
            //    gb.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 90);
            //}
            gb.transform.localPosition = new Vector3(-2f, 0, 0);
            gb.GetComponent<MovingObject>().state = StateObs.Left;
        }
        else
        {
            //if (Random.Range(0, 2) == 0)
            //{
            //    gb.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 90);
            //}
            gb.transform.localPosition = new Vector3(2f, 0, 0);
            gb.GetComponent<MovingObject>().state = StateObs.Right;
        }

    }

}
