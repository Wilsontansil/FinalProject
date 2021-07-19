using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObstacle : MonoBehaviour
{
    DotDouble GM;

    private void Awake()
    {
        GM = FindObjectOfType<DotDouble>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.CompareTag("DotDouble"))
        {
            //GM.state = DotDoubleState.gameOver;
            GM.UpdateScore();
            GM.GetPoint(transform);
            Destroy(gameObject);
        }
    }
}
