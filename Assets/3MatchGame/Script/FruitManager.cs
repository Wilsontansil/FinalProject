using System.Collections.Generic;
using UnityEngine;

public enum levelObject
{
    L1,
    L2
}
public class FruitManager : MonoBehaviour
{
    public Vector3 Scale;
    public levelObject levels;
    public bool isMatch=false;
    public List<GameObject> collisionObject;
    public Transform matchObject;
    bool canDestroy;
    //public bool isMainDestroy;
    GameManagerMatch GM;
    private void Awake()
    {
        GM = FindObjectOfType<GameManagerMatch>();
        collisionObject = new List<GameObject>();
        canDestroy = true;
        //isMainDestroy = false;
        //addPos = new Vector3(0, .3f, 0);
    }
    void Start()
    {
        isMatch = false;
    }
    private void Update()
    {
        CheckCombo();
        //if (isMatch)
        //{
        //    //DestroySelf();
        //    if (canDestroy)
        //    {
        //        //AnimationDestroy();
        //        canDestroy = false;
        //    }

        //}
    }
    void CheckCombo()
    {
        if (collisionObject.Count>1)
        {
            if (canDestroy)
            {
                isMatch = true;
                GM.fruitInGame.Add(gameObject);
                //isMainDestroy = true;
                //matchObject = gameObject.transform;
                for (int i = 0; i < collisionObject.Count; i++)
                {
                    collisionObject[i].GetComponent<FruitManager>().isMatch = true;
                    //collisionObject[i].GetComponent<FruitManager>().matchObject = matchObject;
                    GM.fruitInGame.Add(collisionObject[i]);
                    collisionObject[i].GetComponent<FruitManager>().canDestroy = false;
                }
                //if (levels == levelObject.L1)
                //{
                //    if (isMainDestroy)
                //    {
                //        GM.instantiateBall(gameObject.transform.position, GM.jelly[Random.Range(0, GM.jelly.Count)]);
                //    }
                //}

                collisionObject.Clear();
                canDestroy = false;
            }

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(gameObject.transform.tag))
        {
            if (!collisionObject.Contains(collision.gameObject))
            {
                collisionObject.Add(collision.gameObject);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(gameObject.transform.tag))
        {
            if (!collisionObject.Contains(collision.gameObject))
            {
                collisionObject.Add(collision.gameObject);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(gameObject.transform.tag))
        {
            //collision.gameObject.GetComponent<FruitManager>().isMatch = false;
            collisionObject.Remove(collision.gameObject);
        }


    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void AnimationDestroy(Transform mainObject)
    {
        if (levels == levelObject.L2)
        {
            LeanTween.move(gameObject, mainObject, .2f).setDelay(.1f);
            LeanTween.scale(gameObject, Vector2.zero, .3f).setDelay(.15f).setOnComplete(DestroySelf);
        }
        else
        {
            LeanTween.move(gameObject, mainObject, .2f);
            LeanTween.scale(gameObject, Vector2.zero, .3f).setDelay(.1f).setOnComplete(DestroySelf);
        }


    }
}
