using UnityEngine;

public class BGMTrigger : MonoBehaviour
{
    [Header("지역 BGM")]
    [SerializeField] public AudioClip clip;

    [Header("페이드 시간")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("컷신 중 지역 BGM을 멈출지")]
    [SerializeField] private bool isRunningCutscene = true;

    private bool wasRunningCutScene;
    private bool isPlayerInside;

    private void Start()
    {
        wasRunningCutScene = GameManager.Instance.gameData.isRunningCutScene;
    }

    private void Update()
    {
        bool isRunningCutScene = GameManager.Instance.gameData.isRunningCutScene;

        // 옵션이 true이고 컷신이 방금 시작됐다면 지역 BGM 정지
        if (isRunningCutscene &&
            !wasRunningCutScene &&
            isRunningCutScene &&
            isPlayerInside)
        {
            SoundManager.Instance.FadeOutBGM(fadeDuration);
        }

        // 옵션이 true이고 컷신이 방금 끝났다면 지역 BGM 재생
        if (isRunningCutscene &&
            wasRunningCutScene &&
            !isRunningCutScene &&
            isPlayerInside)
        {
            SoundManager.Instance.FadeToBGM(clip, fadeDuration);
        }

        wasRunningCutScene = isRunningCutScene;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        StartBGM();
    }

    public void StartBGM()
    {
        isPlayerInside = true;

        // 옵션이 true일 때만 컷신 중 지역 BGM 재생을 막음
        if (isRunningCutscene && GameManager.Instance.gameData.isRunningCutScene)
            return;

        SoundManager.Instance.FadeToBGM(clip, fadeDuration);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInside = false;

        if (GameManager.Instance == null ||
            (isRunningCutscene && GameManager.Instance.gameData.isRunningCutScene))
            return;

        if (SoundManager.Instance == null)
            return;

        SoundManager.Instance.FadeOutBGM(fadeDuration);
    }
}