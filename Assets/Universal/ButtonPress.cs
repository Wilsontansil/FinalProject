using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public void Press()
    {

        LeanTween.scale(gameObject, new Vector3(.9f, .9f, .9f), .05f).setEase(LeanTweenType.easeInBack).setOnComplete(Unpress);
    }

    public void Unpress()
    {
        LeanTween.scale(gameObject, new Vector3(1.05f, 1.05f, 1.05f), .1f).setEase(LeanTweenType.easeOutBounce).setOnComplete(Finish);
    }
    public void Finish()
    {
        gameObject.transform.localScale = Vector3.one;
    }
}
