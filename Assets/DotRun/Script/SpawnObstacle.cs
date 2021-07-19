using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour
{
    [SerializeField] List<GameObject> obstacle;
    [SerializeField] List<GameObject> point;
    //public bool canMoveGame =true;
    GameManagerDotRun gameManagerDotRun;
    DotRunController dotRunColor;


    public List<Color> colorObstacle;
    private void Awake()
    {
        gameManagerDotRun = FindObjectOfType<GameManagerDotRun>();
        dotRunColor = FindObjectOfType<DotRunController>();
        colorObstacle = new List<Color>();
    }
    private void Update()
    {
        if (gameManagerDotRun.gameState == GameStateDotRun.inPlayGame)
        {
            if (gameManagerDotRun._timeMain > gameManagerDotRun.timeSpawnObstacle)
            {
                InstantiateObstcale();
                InstantiatePoint();
                gameManagerDotRun.timeSpawnObstacle += gameManagerDotRun.timeAdditionObstcle;
            }
            //if (gameManagerDotRun._timeMain> gameManagerDotRun.timeSpawnPoint)
            //{
            //    InstantiatePoint();
            //    gameManagerDotRun.timeSpawnPoint = gameManagerDotRun.timeSpawnObstacle + gameManagerDotRun.timeAdditionPoint;
            //}

        }

    }

     void InstantiatePoint()
    {
        //GameObject parent = new GameObject();
        //parent.AddComponent<ObstacleManager>();
        //parent.transform.SetParent(gameObject.transform);
        //parent.transform.localPosition = Vector3.zero;
        GameObject gb = Instantiate(point[0]);
        gb.transform.SetParent(gameObject.transform);
        gb.AddComponent<ObstacleManager>();
        gb.transform.localPosition = new Vector3(Random.Range(-1.5f, 1.5f), -3f, 0);
        gb.transform.localScale = Vector3.one;
        gb.GetComponentInChildren<SpriteRenderer>().color = colorObstacle[Random.Range(0,colorObstacle.Count)];
        Destroy(gb, 10f);

    }
    void InstantiateObstcale()
    {
        colorObstacle.Clear();
        GameObject parent = new GameObject();
        parent.AddComponent<ObstacleManager>();
        parent.transform.SetParent(gameObject.transform);
        parent.transform.localPosition = Vector3.zero;
        Destroy(parent, 10f);
        List<int> randNum = new List<int>();
        randNum = GenerateRandomNumber(gameManagerDotRun.colorGame.Count);
        colorObstacle = checkColor(randNum);
        float posX = -1.6f;
        for (int i = 0; i < 5; i++)
        {
            int randCount = Random.Range(4,8);
            float up = 0;
            for (int x = 0; x < randCount; x++)
            {
                SpawnChildObstacle(posX, up, parent, colorObstacle[i]);
                up += .3f;

                //if (i==0)
                //{
                //    SpawnChildObstacle(-1.6f,up, parent, gameManagerDotRun.colorGame[randNum[i]]);
                //    up += .3f;
                //}
                //else if(i==1)
                //{
                //    SpawnChildObstacle(-.8f,up, parent, gameManagerDotRun.colorGame[randNum[i]]);
                //    up += .3f;
                //}
                //else if (i==2)
                //{
                //    SpawnChildObstacle(0,up, parent, gameManagerDotRun.colorGame[randNum[i]]);
                //    up += .3f;
                //}
                //else if (i == 3)
                //{
                //    SpawnChildObstacle(.8f,up, parent, gameManagerDotRun.colorGame[randNum[i]]);
                //    up += .3f;
                //}
                //else if(i == 4)
                //{
                //    SpawnChildObstacle(1.6f,up, parent, gameManagerDotRun.colorGame[randNum[i]]);
                //    up += .3f;
                //}
            }
            posX += .8f;

        }

    }

    void SpawnChildObstacle(float posX,float posY,GameObject parent,Color color)
    {
        GameObject gb = Instantiate(obstacle[0]);
        gb.transform.SetParent(parent.transform);
        gb.transform.localPosition = new Vector3(posX, posY, 0);
        gb.GetComponent<SpriteRenderer>().color = color;
    }

    List<int> GenerateRandomNumber(int countMax)
    {
        List<int> listRandom = new List<int>();
        while (listRandom.Count<countMax)
        {
            int x = Random.Range(0, countMax);
            while (!listRandom.Contains(x))
            {
                listRandom.Add(x);
            }
        }
        return listRandom;
    }

    List<Color> checkColor(List<int> x)
    {
        List<Color> color = new List<Color>();
        for (int i = 0; i < gameManagerDotRun.colorGame.Count; i++)
        {
            color.Add(gameManagerDotRun.colorGame[i]);
        }
        List<Color> final = new List<Color>();
        for (int i = 0; i < color.Count; i++)
        {
            if (color[i] != dotRunColor.sr.color)
            {
                final.Add(color[i]);
                //color.RemoveAt(i);

                //color.rem(color.Count);
            }
        }
        return final;
    }
}
