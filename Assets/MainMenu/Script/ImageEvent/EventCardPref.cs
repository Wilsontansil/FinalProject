using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
public class EventCardPref : MonoBehaviour
{
    public Image eventIMG;
    public TextMeshProUGUI txtEvent;
    public TextMeshProUGUI txtTitle;
    public Button btnLink;
    public bool finishTakeImage;

    private void Awake()
    {
        finishTakeImage = false;
    }
    public void ExitObject()
    {
        gameObject.GetComponentInParent<EventParent>().EventCount -= 1;
        LeanTween.moveLocalY(gameObject, 700, .5f).setEase(LeanTweenType.easeInCubic).setOnComplete(DestoryGameObject);
    }
    private void DestoryGameObject()
    {
        gameObject.GetComponentInParent<EventParent>().CheckEvent();
        Destroy(gameObject);

    }
    public void SetLink(bool HaveLink,string link)
    {
        if (!HaveLink)
        {
            btnLink.gameObject.SetActive(false);
        }
        else
        {
            btnLink.onClick.AddListener(() => Application.OpenURL(link));
        }
    }
    public IEnumerator GetImageEvent(string url)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.LogError("ERROR");
            }
            else
            {
                DownloadHandlerTexture dhd = unityWebRequest.downloadHandler as DownloadHandlerTexture;
                Texture2D texture = dhd.texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector3(.5f, .5f),256);
                eventIMG.sprite = sprite;
            }
        }
        finishTakeImage = true;
    }
}
