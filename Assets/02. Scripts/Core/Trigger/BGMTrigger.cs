using UnityEngine;

public class BGMTrigger : MonoBehaviour
{
    [Header("지역 BGM")]
    [SerializeField] private AudioClip clip;

    [Header("페이드 시간")]
    [SerializeField] private float fadeDuration = 1.5f;

    private bool wasRunningCutScene;
    private bool isPlayerInside;

    private void Start()
    {
        wasRunningCutScene =
            GameManager.Instance.gameData.isRunningCutScene;
    }

    private void Update()
    {
        bool isRunningCutScene =
            GameManager.Instance.gameData.isRunningCutScene;

        // 컷신이 방금 끝났고 플레이어가 아직 지역 안에 있음
        if (wasRunningCutScene &&
            !isRunningCutScene &&
            isPlayerInside)
        {
            // 컷신 BGM을 줄인 뒤 지역 BGM 재생
            SoundManager.Instance.FadeToBGM(
                clip,
                fadeDuration
            );
        }

        wasRunningCutScene = isRunningCutScene;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInside = true;

        // 컷신 중에는 지역 BGM을 재생하지 않음
        if (GameManager.Instance.gameData.isRunningCutScene)
            return;

        SoundManager.Instance.FadeToBGM(
            clip,
            fadeDuration
        );
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInside = false;

        // 컷신 중에는 컷신 BGM을 끄지 않음
        if (GameManager.Instance == null || GameManager.Instance.gameData.isRunningCutScene)
            return;

        //nullpointexception 예외처리
        if(SoundManager.Instance == null) return;
        
        // 지역에서 빠져나갈 때 서서히 정지
        SoundManager.Instance.FadeOutBGM(fadeDuration);
    }
}