using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    CanvasGroup cg;
    bool damagePopup;


    [SerializeField]
    float speed;
    [SerializeField]
    float fadeOutDuration;
    [SerializeField]
    float fadeInDuration;
    [SerializeField]
    float maxAlpha;
    [SerializeField]
    float stayTime;

    public void Initialize(string text)
    {
        this.text.text = text;
        cg.alpha = 0;
        LeanTween.alphaCanvas(cg,maxAlpha,fadeInDuration).setOnComplete(StayTime);
    }

    public void OnFadeIn()
    {
        LeanTween.alphaCanvas(cg, 0, fadeOutDuration).setOnComplete(OnComplete);
    }
    public void StayTime()
    {
        StartCoroutine(Stay());
    }

    IEnumerator Stay()
    {
        yield return new WaitForSecondsRealtime(stayTime);
        OnFadeIn();
    }

    public void OnComplete()
    {
        Destroy(gameObject);
    }

    public void Update()
    {
        transform.Translate(Vector2.up*speed);
    }
}
