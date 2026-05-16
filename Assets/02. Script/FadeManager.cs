using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeManager : Singleton<FadeManager>
{
    public Image fadeImage;
    public bool isFading = false;

    // 외부에서 호출할 함수 (예: StartCoroutine(FadeManager.Instance.FadeIn()))
    public IEnumerator FadeOut(float fadeDuration) // 화면이 검게 변함 (Alpha 0 -> 1)
    {
        isFading = true;
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            // alpha 값을 0에서 1까지 fadeDuration 동안 보간
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        // 마지막에 확실하게 1로 고정
        fadeImage.color = new Color(c.r, c.g, c.b, 1f);
    }

    public IEnumerator FadeIn(float fadeDuration) // 화면이 다시 밝아짐 (Alpha 1 -> 0)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            // alpha 값을 1에서 0까지 fadeDuration 동안 보간
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, 0f);
        isFading = false;
    }

    public void StartFadeOut(float fadeDuration)
    {
        StartCoroutine(FadeOut(fadeDuration));
    }
        public void StartFadeIn(float fadeDuration)
    {
        StartCoroutine(FadeIn(fadeDuration));
    }

}
