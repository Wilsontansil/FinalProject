using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    GameManagerDotRun GM;

    private void Awake()
    {
        GM = FindObjectOfType<GameManagerDotRun>();
        //colorObstacle = new List<Color>();
    }
    private void Update()
    {
        if (GM.gameState == GameStateDotRun.inPlayGame)
        {
            transform.Translate(Vector2.down * GM._GameSpeed * Time.deltaTime);
        }


    }
}
