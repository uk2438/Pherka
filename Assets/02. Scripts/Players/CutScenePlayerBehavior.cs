using UnityEngine;

public class CutScenePlayerBehavior : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator playerAnimator;

    [Header("컷신용 순간이동 위치")]
    [SerializeField] private Vector3[] positions;

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private void Awake()
    {
        if (playerAnimator == null && player != null)
        {
            playerAnimator =
                player.GetComponentInChildren<Animator>();
        }
    }

    public void TeleportSignal(int index)
    {
        if (player == null)
            return;

        if (index < 0 || index >= positions.Length)
        {
            Debug.LogError(
                $"잘못된 텔레포트 인덱스입니다: {index}",
                this
            );

            return;
        }

        Rigidbody2D playerRb =
            player.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
            playerRb.position = positions[index];
        }
        else
        {
            player.position = positions[index];
        }

        Physics2D.SyncTransforms();
    }

    public void LookUpSignal()
    {
        SetDirection(Direction.Up);
    }

    public void LookDownSignal()
    {
        SetDirection(Direction.Down);
    }

    public void LookLeftSignal()
    {
        SetDirection(Direction.Left);
    }

    public void LookRightSignal()
    {
        SetDirection(Direction.Right);
    }

    private void SetDirection(Direction direction)
    {
        if (playerAnimator == null)
            return;

        int horizontal = 0;
        int vertical = 0;

        switch (direction)
        {
            case Direction.Up:
                vertical = 1;
                break;

            case Direction.Down:
                vertical = -1;
                break;

            case Direction.Left:
                horizontal = -1;
                break;

            case Direction.Right:
                horizontal = 1;
                break;
        }

        playerAnimator.SetInteger(
            "hAxisRaw",
            horizontal
        );

        playerAnimator.SetInteger(
            "vAxisRaw",
            vertical
        );

        playerAnimator.SetBool(
            "isChange",
            true
        );
    }

    public void SetDirectorToStructureSignal()
    {
        tag = "Structure";

        ObjectData directorData = GetComponent<ObjectData>();
        directorData.SetDialogueCondition(true);
        
    }
}