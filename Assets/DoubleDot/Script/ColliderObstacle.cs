using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderObstacle : MonoBehaviour
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
            GM.GameOver(transform);
        }
    }
}
