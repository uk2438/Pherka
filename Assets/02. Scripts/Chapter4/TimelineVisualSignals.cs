using System.Collections;
using UnityEngine;

public class TimelineVisualSignals : MonoBehaviour
{

    public void SpriteFadeIn(GameObject obj)
    {
        StartCoroutine(FadeIn(obj));
    }

    public void SpriteFadeOut(GameObject obj)
    {
        StartCoroutine(FadeOut(obj));
    }
    private IEnumerator FadeIn(GameObject obj, float duration = 2f)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Color color = sr.color;

        // 처음에는 투명하게 설정
        color.a = 0f;
        sr.color = color;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Clamp01(elapsed / duration);
            sr.color = color;

            yield return null;
        }

        // 마지막 값은 정확히 1로 설정
        color.a = 1f;
        sr.color = color;
    }

    private IEnumerator FadeOut(GameObject obj, float duration = 2f)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Color color = sr.color;

        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            sr.color = color;

            yield return null;
        }

        color.a = 0f;
        sr.color = color;
    }
}
