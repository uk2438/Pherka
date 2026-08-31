using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeManager : Singleton<FadeManager>
{
    public FadeData fadeData = new FadeData();
    private Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    // 외부에서 호출할 함수 (예: StartCoroutine(FadeManager.Instance.FadeIn()))
    public IEnumerator FadeOut(float fadeDuration) // 화면이 검게 변함 (Alpha 0 -> 1)
    {
        fadeData.isFading = true;
        float elapsed = 0f;
        Color c = fadeData.fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            // alpha 값을 0에서 1까지 fadeDuration 동안 보간
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            fadeData.fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        // 마지막에 확실하게 1로 고정
        fadeData.fadeImage.color = new Color(c.r, c.g, c.b, 1f);
    }

    public IEnumerator FadeOutImage(Sprite image, float fadeDuration)
    {
        fadeData.isFading = true;
        float elapsed = 0f;
        img.sprite = image;
        Color c = img.color;

        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed/fadeDuration);
            img.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
    }

    public IEnumerator FadeIn(float fadeDuration) // 화면이 다시 밝아짐 (Alpha 1 -> 0)
    {
        float elapsed = 0f;
        Color c = fadeData.fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            // alpha 값을 1에서 0까지 fadeDuration 동안 보간
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            fadeData.fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadeData.fadeImage.color = new Color(c.r, c.g, c.b, 0f);
        fadeData.isFading = false;
    }

    public IEnumerator FadeInImage(Sprite image, float fadeDuration)
    {
        float elapsed = 0f;
        img.sprite = image;
        Color c = img.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            // alpha 값을 1에서 0까지 fadeDuration 동안 보간
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            img.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadeData.fadeImage.color = new Color(c.r, c.g, c.b, 0f);
        fadeData.isFading = false;
    }

    public void SetGuideBackgroundActive(bool active)
{
    if (fadeData.guideBackground == null)
    {
        Debug.LogWarning(
            "FadeManager의 Background가 지정되지 않았습니다."
        );

        return;
    }

    fadeData.guideBackground .gameObject.SetActive(active);
}
    public void StartFadeOut(float fadeDuration)
    {
        StartCoroutine(FadeOut(fadeDuration));
    }
    public void StartFadeIn(float fadeDuration)
    {
        StartCoroutine(FadeIn(fadeDuration));
    }

    public void StartFadeOutImage(Sprite image)
    {
        StartCoroutine(FadeOutImage(image, 1f));
    }
    
    public void StartFadeInImage(Sprite image)
    {
        StartCoroutine(FadeInImage(image, 1f));
    }

}
