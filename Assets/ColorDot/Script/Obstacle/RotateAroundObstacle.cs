using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObstacle : MonoBehaviour
{
    public List<GameObject> target;
    public List<Transform> dotMove;
    float startPoint;
    private void Awake()
    {
        startPoint = 0;
    }
    private void Start()
    {
        foreach (var item in target)
        {
            LeanTween.move(item, new Vector3[] { new Vector3(1, 1, 0f), new Vector3(1, -1, 0f), new Vector3(-1, -1, 0f), new Vector3(-1, 1, 0f) }, 1.5f).setDelay(startPoint).setEase(LeanTweenType.easeOutQuad).setLoopPingPong();
            startPoint += .8f;
        }
        //LTBezierPath ltPath = new LTBezierPath(new Vector3[] { new Vector3(1, 1, 0f), new Vector3(1, -1, 0f), new Vector3(-1, -1, 0f), new Vector3(-1, 1f, 0f) });

        //LeanTween.move(target, ltPath, 1).setEase(LeanTweenType.easeInOutQuad); // animate
        //Vector3 pt = ltPath.point(0.6f); // retrieve a point along the path
    }
    private void Update()
    {
        //if (target.transform.position == dotMove[startPoint].position)
        //{
        //    if (startPoint == dotMove.Count-1)
        //    {
        //        startPoint = 0;
        //    }
        //    else
        //    {
        //        startPoint += 1;
        //    }
        //}
        //else
        //{
        //    target.transform.position = Vector2.MoveTowards(target.transform.position, dotMove[startPoint].position,5* Time.deltaTime);
        //    //LeanTween.moveLocal(target, dotMove[startPoint].position, 1f).setHasInitialized(false);
        //    //target.transform.position = Vector3.Lerp(target.transform.position, dotMove[startPoint].transform.position, 5* Time.deltaTime);
        //}

    }


}
