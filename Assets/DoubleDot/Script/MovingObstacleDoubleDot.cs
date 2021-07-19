using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacleDoubleDot : MonoBehaviour
{
    DotDouble GM;

    private void Awake()
    {
        GM = FindObjectOfType<DotDouble>();
    }
    void Update()
    {
        if (GM.state == DotDoubleState.game)
        {
            transform.Translate(Vector3.down * 2 * Time.deltaTime);
        }

    }
}
