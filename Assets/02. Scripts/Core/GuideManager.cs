using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GuideManager : Singleton<GuideManager>
{
    [Header("조작법 이미지 UI")]
    [SerializeField] private Image[] guideImages;

    private bool canInput;

    public bool IsShowing { get; private set; }

    void Update()
    {
        if (!IsShowing || !canInput)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CloseGuide();
        }
    }

    public IEnumerator ShowGuide(int index)
    {
        if (IsShowing)
            yield break;

        if (guideImages == null)
        {
            Debug.LogError("TutorialGuideManager의 Guide Image가 지정되지 않았습니다.");

            yield break;
        }

        IsShowing = true;
        canInput = false;

        // 배경 이미지 활성화
        FadeManager.Instance.SetGuideBackgroundActive(
            true
        );

        // 조작법 이미지 활성화
        guideImages[index].gameObject.SetActive(true);

        /*
         * 조작법 이벤트를 실행시킨 Space 입력이
         * 곧바로 이미지를 닫지 않도록 한 프레임 대기
         */
        yield return null;

        canInput = true;

        // Space를 눌러 CloseGuide가 호출될 때까지 대기
        yield return new WaitUntil(() => !IsShowing);
    }

    public void CloseGuide()
    {
        if (!IsShowing)
            return;

        IsShowing = false;
        canInput = false;

        if (guideImages != null)
        {
            foreach (Image guideImage in guideImages)
                guideImage.gameObject.SetActive(false);
        }

        FadeManager.Instance.SetGuideBackgroundActive(false);
    }

    public void ShowGuideFromSignal(int index)
    {
        StartCoroutine(ShowGuide(index));
    }
}