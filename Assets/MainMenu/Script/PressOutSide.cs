using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressOutSide : MonoBehaviour
{
    [SerializeField] GameObject[] panelMain;//close
    [SerializeField] GameObject[] PanelChild;//if toouch outside then exit
    [SerializeField] GameObject[] panelExecption;
    [SerializeField] Camera Camera;

    GameManagerMainMenu GM;
    private void Awake()
    {
        GM = FindObjectOfType<GameManagerMainMenu>();
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PanelChild.Length; i++)
        {
            if (PanelChild[i].activeInHierarchy)
            {
                CloseIfClickedOutside(PanelChild[i]);
            }

        }
    }

    void CloseIfClickedOutside(GameObject panel)
    {
        if (Input.GetMouseButton(0))
        {
            for (int i = 0; i < panelExecption.Length; i++)
            {
                if (panelExecption[i].activeInHierarchy)
                {
                    return;
                }
            }
            RectTransform rectTransform = panel.GetComponent<RectTransform>();
            Canvas canvas = GetComponent<Canvas>();
            Camera camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera;
            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, camera))
            {
                GM.CheckOpenedPanel();
            }
        }
    }
}
