using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    void Update()
    {
        if (player != null)
        {
            if (player.position.y > transform.position.y)
            {

                transform.position = new Vector3(player.position.x, player.position.y, -10);
            }

        }

    }
}
