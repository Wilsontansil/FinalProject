using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObstacleIndividual : MonoBehaviour
{
    int totalHit;

    private void Start()
    {
        //InstantiateText();
    }
    void InstantiateText()
    {
        GameObject gb = new GameObject();
        gb.AddComponent<Text>().text = "50";
        gb.transform.SetParent(gameObject.transform);
    }
    
}
