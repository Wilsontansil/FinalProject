using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotAnimation : MonoBehaviour
{
    private void Start()
    {
        LeanTween.moveLocalY(gameObject, 15, 1f).setEase(LeanTweenType.easeInOutCubic).setLoopPingPong();
    }
}
