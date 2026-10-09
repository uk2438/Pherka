using System;
using System.Collections;
using DialogueSystem;
using UnityEngine;

public class DialogueEventManager :
    Singleton<DialogueEventManager>
{
    [Header("Player Object")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerFollower SubPlayer;
    [SerializeField] private GameObject[] prologueItems;
    private bool isRunningEvent;

    public bool IsRunningEvent
    {
        get { return isRunningEvent; }
    }

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
                        FindItem(eventData.spawnItem, prologueItems);
                        break;
                    case 1:
                        break;
                    case 2:
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
            case DialogueEventType.ShowGuide0:
                yield return StartCoroutine(GuideManager.Instance.ShowGuide(0));
                break;
            case DialogueEventType.ShowGuide1:
                yield return StartCoroutine(GuideManager.Instance.ShowGuide(1));
                break;
            case DialogueEventType.TeleportPlayer:
                yield return StartCoroutine(TeleportPlayer(eventData.teleportTarget, duration));
                break;
        }

        isRunningEvent = false;
    }


    private void FindItem(DialogueItem item, GameObject[] items)
    {
        string targetName;

        switch (item)
        {
            case DialogueItem.TeddyBear:
                targetName = "TeddyBear";
                break;

            default:
                return;
        }

        foreach (GameObject obj in items)
        {
            if (obj == null || obj.name != targetName)
                continue;

            obj.SetActive(true);
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
                if(GameManager.Instance == null) yield break;

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