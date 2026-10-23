using System;
using System.Collections;
using DialogueSystem;
using UnityEngine;
using UnityEngine.Video;

public class DialogueEventManager : Singleton<DialogueEventManager>
{
    [Serializable]
    private class Items
    {
        public GameObject[] findItems;
        public GameObject[] hiddenItems;
    }
    [Header("Player Object")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerFollower SubPlayer;

    [Header("Items")]
    [SerializeField] private Items prologueItems;
    [SerializeField] private Items chpater1Items;
    [SerializeField] private Items chpater2Items;
    [SerializeField] private Items chpater3Items;
    private bool isRunningEvent;
    public bool IsRunningEvent
    {
        get { return isRunningEvent; }
    }

    [Header("설명 영상")]
    [SerializeField] private GameObject guideVideoPanel;
    [SerializeField] private VideoPlayer guideVideoPlayer;
    [SerializeField] private VideoClip[] guideVideoClips;


    public IEnumerator ExecuteEvent(int dialogueId, int lineIndex, DialogueEventTiming timing)
    {
        DialogueEventData[] events =
            DialogueEventStaticData.Events;

        foreach (DialogueEventData eventData in events)
        {
            if (eventData.dialogueId != dialogueId ||
                eventData.lineIndex != lineIndex ||
                eventData.timing != timing)
            {
                continue;
            }

            yield return StartCoroutine(
                RunEvent(eventData)
            );
        }
    }

    private IEnumerator RunEvent(DialogueEventData eventData)
    {
        isRunningEvent = true;

        float duration = eventData.duration;

        switch (eventData.eventType)
        {
            case DialogueEventType.FadeOut:
                yield return StartCoroutine(
                    FadeManager.Instance.FadeOut(duration)
                );
                break;

            case DialogueEventType.FadeIn:
                yield return StartCoroutine(
                    FadeManager.Instance.FadeIn(duration)
                );
                break;

            case DialogueEventType.FadeOutIn:
                yield return StartCoroutine(
                    FadeManager.Instance.FadeOut(duration)
                );

                yield return StartCoroutine(
                    FadeManager.Instance.FadeIn(duration)
                );
                break;
            case DialogueEventType.SpawnItem:
                switch (DialogueManager.Instance.GetPresentChapter())
                {
                    case 0:
                        FindItem(eventData.item, prologueItems);
                        break;
                    case 1:
                        FindItem(eventData.item, chpater1Items);
                        break;
                    case 2:
                        FindItem(eventData.item, chpater2Items);
                        break;
                    case 3:
                        // PrologueManager.Instance.CheckWasAction(objectData);
                        break;
                    case 4:
                        // PrologueManager.Instance.CheckWasAction(objectData);
                        break;
                }
                break;
            case DialogueEventType.HiddenItem:
                switch (DialogueManager.Instance.GetPresentChapter())
                {
                    case 0:
                        HiddenItem(eventData.item, prologueItems);
                        break;
                    case 1:
                        HiddenItem(eventData.item, chpater1Items);
                        break;
                    case 2:
                        HiddenItem(eventData.item, chpater2Items);
                        break;
                    case 3:
                        // PrologueManager.Instance.CheckWasAction(objectData);
                        break;
                    case 4:
                        // PrologueManager.Instance.CheckWasAction(objectData);
                        break;
                }
                break;

            case DialogueEventType.SetMartActive:
                yield return StartCoroutine(PrologueManager.Instance.SetMartNPCActive(true));
                break;
            case DialogueEventType.ShowVideo:
                ShowVideo(eventData.videoIndex);
                break;
            case DialogueEventType.HideVideo:
                HideVideo();
                break;
            case DialogueEventType.TeleportPlayer:
                yield return StartCoroutine(TeleportPlayer(eventData.teleportTarget, duration));
                break;
        }

        isRunningEvent = false;
    }

    private void ShowVideo(int videoIndex)
    {
        Debug.Log($"[ShowVideo 호출] index={videoIndex}", this);
        if (guideVideoPanel == null || guideVideoPlayer == null)
        {
            Debug.LogError("영상 패널과 VideoPlayer를 연결해주세요.", this);
            return;
        }

        if (guideVideoClips == null ||
            videoIndex < 0 ||
            videoIndex >= guideVideoClips.Length ||
            guideVideoClips[videoIndex] == null)
        {
            Debug.LogError($"영상 번호를 확인해주세요: {videoIndex}", this);
            return;
        }

        guideVideoPlayer.playOnAwake = false;

        guideVideoPanel.SetActive(true);

        // 이전 영상을 멈추고 선택한 영상을 처음부터 재생
        guideVideoPlayer.Stop();
        guideVideoPlayer.source = VideoSource.VideoClip;
        guideVideoPlayer.clip = guideVideoClips[videoIndex];
        Debug.Log(
        $"[클립 대입 완료] 대상={guideVideoPlayer.name}, 클립={guideVideoPlayer.clip?.name ?? "None"}",
        guideVideoPlayer
    );
        guideVideoPlayer.isLooping = true;
        guideVideoPlayer.Play();
    }

    private void HideVideo()
    {
        if (guideVideoPlayer != null)
        {
            guideVideoPlayer.Stop();
        }

        if (guideVideoPanel != null)
        {
            guideVideoPanel.SetActive(false);
        }
    }

    private void FindItem(DialogueItem item, Items items)
    {
        string targetName;

        switch (item)
        {
            case DialogueItem.TeddyBear:
                targetName = "TeddyBear";
                break;

            case DialogueItem.Book:
                targetName = "Book";
                break;

            case DialogueItem.Book2:
                targetName = "Book2";
                break;

            case DialogueItem.Sweatsuit:
                targetName = "Sweatsuit";
                break;

            case DialogueItem.BookPherkaOutline:
                targetName = "BookPherkaOutline";
                break;

            case DialogueItem.GymPherkaOutline:
                targetName = "GymPherkaOutline";
                break;

            default:
                return;
        }

        foreach (GameObject obj in items.findItems)
        {
            if (obj == null || obj.name != targetName)
                continue;

            obj.SetActive(true);
            return;
        }

        Debug.LogWarning($"{targetName}을(를) 등록된 배열에서 찾지 못했습니다.");
    }
    private void HiddenItem(DialogueItem item, Items items)
    {
        string targetName;

        switch (item)
        {
            case DialogueItem.Book:
                targetName = "Book";
                break;

            default:
                return;
        }

        foreach (GameObject obj in items.hiddenItems)
        {
            if (obj == null || obj.name != targetName)
                continue;

            obj.SetActive(false);
            return;
        }

        Debug.LogWarning($"{targetName}을(를) 등록된 배열에서 찾지 못했습니다.");
    }
    private IEnumerator TeleportPlayer(DialogueTeleportTarget teleportTarget, float duration)
    {
        if (player == null)
        {
            Debug.LogError(
                "DialogueEventManager의 Player가 지정되지 않았습니다."
            );

            yield break;
        }

        if (FadeManager.Instance == null)
        {
            Debug.LogError(
                "FadeManager.Instance가 존재하지 않습니다."
            );

            yield break;
        }

        Vector3 targetPosition;


        // 텔레포트 할 위치가 생길때마다 case를 추가
        switch (teleportTarget)
        {
            case DialogueTeleportTarget.FirstGoToWork:
                if (PrologueManager.Instance == null)
                {
                    yield break;
                }

                targetPosition =
                    PrologueManager.Instance.GetFirstGoToWork();
                break;
            case DialogueTeleportTarget.SecondGoToWork:
                if (PrologueManager.Instance == null)
                {
                    yield break;
                }

                targetPosition =
                    PrologueManager.Instance.GetSecondGoToWork();

                break;

            case DialogueTeleportTarget.GoToHome:
                if (PrologueManager.Instance == null)
                {
                    yield break;
                }

                targetPosition =
                    PrologueManager.Instance.GetGoToHome();

                break;

            case DialogueTeleportTarget.GoToMainStreet:
                if (GameManager.Instance == null) yield break;

                targetPosition = GameManager.Instance.GetMainStreetMap();
                break;

            case DialogueTeleportTarget.GoChapter1Map:
                if (GameManager.Instance == null) yield break;

                targetPosition = GameManager.Instance.GetChapter1Map();
                break;

            case DialogueTeleportTarget.GoChapter2Map:
                if (GameManager.Instance == null) yield break;

                targetPosition = GameManager.Instance.GetChapter2Map();
                break;

            default:
                Debug.LogWarning(
                    "순간이동 목적지가 지정되지 않았습니다."
                );

                yield break;
        }

        FadeManager.Instance.fadeData.isFading = true;

        yield return StartCoroutine(
            FadeManager.Instance.FadeOut(
                duration
            )
        );

        SetPlayerPosition(
            targetPosition
        );

        yield return StartCoroutine(
            FadeManager.Instance.FadeIn(
                duration
            )
        );

        FadeManager.Instance.fadeData.isFading = false;
    }

    private void SetPlayerPosition(Vector3 position)
    {
        if (player == null)
            return;

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            playerRb.position = position;
            playerRb.velocity = Vector2.zero;
        }

        player.transform.position = position;

        // 따라가기 시작한 동료만 플레이어와 같은 위치로 이동
        if (SubPlayer != null && SubPlayer.isActiveAndEnabled)
        {
            SubPlayer.TeleportWithPlayer(position);
        }

        Physics2D.SyncTransforms();
    }
}