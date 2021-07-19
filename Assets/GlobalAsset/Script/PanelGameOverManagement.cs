using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelGameOverManagement : MonoBehaviour
{
    [SerializeField] GameObject BoxGameOver;
    public TextMeshProUGUI txtHighScore;
    public TextMeshProUGUI txtScoreBoard;
    public void OpenGameOverBox()
    {
        LeanTween.scale(BoxGameOver, Vector3.one, .8f).setEase(LeanTweenType.easeOutCubic);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
