using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateObs {
    Right,
    Left

}

public class MovingObject : MonoBehaviour
{
    public StateObs state;
    //int speed=2;
    DotDouble GM;

    private void Awake()
    {
        GM = FindObjectOfType<DotDouble>();
    }
    private void Start()
    {
        if (state == StateObs.Left)
        {
            LeanTween.moveLocalX(gameObject, -1, 1.5f).setEase(LeanTweenType.easeInOutCubic).setLoopPingPong();
        }else if (state == StateObs.Right)
        {
            LeanTween.moveLocalX(gameObject, 1, 1.5f).setEase(LeanTweenType.easeInOutCubic).setLoopPingPong();
        }
    }
    //void Update()
    //{
    //    if (GM.state == DotDoubleState.game)
    //    {
    //        transform.Translate(Vector3.down * speed * Time.deltaTime);
    //    }

    //}

}
