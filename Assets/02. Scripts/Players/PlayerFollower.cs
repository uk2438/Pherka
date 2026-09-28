using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [Header("따라갈 플레이어")]
    [SerializeField] private Transform player;

    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float followDistance = 0.7f;
    [SerializeField] private float recordSpacing = 0.1f;

    [Header("애니메이션")]
    [SerializeField] private Animator anim;

    private readonly List<Vector3> positionHistory =
        new List<Vector3>();

    private Vector3 lastRecordedPosition;

    private void Awake()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    private void OnEnable()
    {
        positionHistory.Clear();

        if (player == null)
            return;

        lastRecordedPosition = player.position;
        positionHistory.Add(player.position);
    }

    private void OnDisable()
    {
        if (anim != null)
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        RecordPlayerPosition();
        FollowPlayerPath();
    }

    private void RecordPlayerPosition()
    {
        float spacing = Mathf.Max(recordSpacing, 0.01f);

        while (Vector3.Distance(lastRecordedPosition, player.position) >= spacing)
        {
            lastRecordedPosition = Vector3.MoveTowards(
                lastRecordedPosition,
                player.position,
                spacing
            );

            positionHistory.Insert(0, lastRecordedPosition);
        }

        int maximumHistory = Mathf.CeilToInt(followDistance / spacing) + 10;

        if (positionHistory.Count > maximumHistory)
        {
            positionHistory.RemoveRange(
                maximumHistory,
                positionHistory.Count - maximumHistory
            );
        }
    }

    private void FollowPlayerPath()
    {
        int targetIndex = Mathf.RoundToInt(
            followDistance / Mathf.Max(recordSpacing, 0.01f)
        );

        if (positionHistory.Count <= targetIndex)
        {
            SetMovingAnimation(false);
            return;
        }

        Vector3 targetPosition =
            positionHistory[targetIndex];

        Vector3 difference =
            targetPosition - transform.position;

        // 목표 위치에 도착한 경우
        if (difference.sqrMagnitude <= 0.0001f)
        {
            transform.position = targetPosition;

            SetMovingAnimation(false);
            return;
        }

        Vector3 previousPosition =
            transform.position;

        Vector3 nextPosition =
            Vector3.MoveTowards(
                previousPosition,
                targetPosition,
                moveSpeed * Time.fixedDeltaTime
            );

        transform.position = nextPosition;

        Vector3 moveDirection =
            nextPosition - previousPosition;

        UpdateDirectionAnimation(moveDirection);
    }

    private void UpdateDirectionAnimation(Vector3 moveDirection)
    {
        if (anim == null)
            return;

        if (moveDirection.sqrMagnitude <= 0.000001f)
        {
            SetMovingAnimation(false);
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Mathf.Abs(moveDirection.x) >
            Mathf.Abs(moveDirection.y))
        {
            horizontal =
                moveDirection.x > 0 ? 1 : -1;
        }
        else
        {
            vertical =
                moveDirection.y > 0 ? 1 : -1;
        }

        // 방향을 먼저 변경
        anim.SetFloat(
            "hAxisRaw",
            horizontal
        );

        anim.SetFloat(
            "vAxisRaw",
            vertical
        );

        // 방향 변경 이후 이동 상태 설정
        SetMovingAnimation(true);
    }

    private void SetMovingAnimation(bool isMoving)
    {
        if (anim == null)
            return;

        anim.SetBool(
            "isMoving",
            isMoving
        );
    }

    public void TeleportWithPlayer(Vector3 newPlayerPosition)
    {
        Rigidbody2D followerRb = GetComponent<Rigidbody2D>();

        if (followerRb != null)
        {
            followerRb.position = newPlayerPosition;
            followerRb.velocity = Vector2.zero;
        }

        transform.position = newPlayerPosition;

        positionHistory.Clear();
        lastRecordedPosition = newPlayerPosition;
        positionHistory.Add(newPlayerPosition);

        SetMovingAnimation(false);
    }
}