using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float Xend;
    public float Xstart;
    Vector3 startM;

    GameManagerDotRun GM;
    private void Start()
    {
        GM = FindObjectOfType<GameManagerDotRun>();
    }
    void Update()
    {
        if (GM.gameState == GameStateDotRun.inPlayGame)
        {
            transform.Translate(Vector2.down * GM._GameSpeed / 4 * Time.deltaTime);
            if (GetComponent<Transform>().position.y < Xend)
            {
                Vector2 pos = new Vector2(0, Xstart);
                GetComponent<Transform>().position = startM;
            }
        }

    }
}
