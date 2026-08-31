using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float h, v;
    bool hDown, vDown, hUp, vUp;
    float moveSpeed = 5f;
    Rigidbody2D rb;
    bool isHorizontalMove = false;
    bool isGrabbing = false;
    Vector3 vecDir;
    GameObject obj;
    Vector3 distancePlayerObj;
    float lastGrabTime;
    RaycastHit2D rayhit;
    RaycastHit2D prevRayHit = default;
    Vector2 rayDirection;
    Vector2 prevSize, prevOffset;
    CapsuleDirection2D prevDirection;
    BoxCollider2D objCol;
    DoorInteraction doorInteraction;
    private bool isAutoMoving;

    public PlayerData playerData = new PlayerData();
    public MovementData objMovementData = new MovementData();
    public FadeData fadeData = new FadeData();
    private Transform originalParent;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // get axis 방향키에따라 입력값이 -1, 0, 1로 받음
        // 쯔꾸르형 게임에선 대각선 이동이 안되도록 하기 위해서 수평과 수직 중 하나의 입력만 받도록 함
        // 대화창이 열려있을때에는 움직이지 못하게 하기
        if (GameManager.Instance.gameData.isAction ||
            FadeManager.Instance.fadeData.isFading ||
            UIManager.Instance.panelData.isPause ||
            GameManager.Instance.gameData.isRunningCutScene||
            GuideManager.Instance.IsShowing)
        {
            h = 0;
            v = 0;
            hDown = false;
            vDown = false;
            hUp = false;
            vUp = false;
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            hDown = Input.GetButtonDown("Horizontal");
            vDown = Input.GetButtonDown("Vertical");
            hUp = Input.GetButtonUp("Horizontal");
            vUp = Input.GetButtonUp("Vertical");
        }

        // 대각선 이동 방지
        if (hDown)
        {
            isHorizontalMove = true;
        }
        else if (vDown)
        {
            isHorizontalMove = false;
        }
        else if (hUp || vUp)
        {
            isHorizontalMove = h != 0;
        }

        // 최종 h, v 값 확정 (대각선 방지 및 우선순위 결정)
        if (isHorizontalMove && h != 0) v = 0;
        else if (!isHorizontalMove && v != 0) h = 0;

        // 4. 애니메이션 파라미터 전달
        if (!isAutoMoving)
        {
            int curH =
                playerData.anim.GetInteger("hAxisRaw");

            int curV =
                playerData.anim.GetInteger("vAxisRaw");

            int targetH =
                isHorizontalMove ? (int)h : 0;

            int targetV =
                !isHorizontalMove ? (int)v : 0;

            if (curH != targetH ||
                curV != targetV)
            {
                playerData.anim.SetBool(
                    "isChange",
                    true
                );

                playerData.anim.SetInteger(
                    "hAxisRaw",
                    targetH
                );

                playerData.anim.SetInteger(
                    "vAxisRaw",
                    targetV
                );
            }
            else
            {
                playerData.anim.SetBool(
                    "isChange",
                    false
                );
            }
        }


        // raycast 방향
        if (vDown && v == 1)
        {
            vecDir = Vector3.up;
        }
        else if (vDown && v == -1)
        {
            vecDir = Vector3.down;
        }
        else if (hDown && h == 1)
        {
            vecDir = Vector3.right;
        }
        else if (hDown && h == -1)
        {
            vecDir = Vector3.left;

        }


        // game action 구현
        if (Input.GetKeyDown(KeyCode.Space) &&
            !UIManager.Instance.panelData.isChoice)
        {
            // 트리거 대화 진행
            if (GameManager.Instance.gameData.isTrigger)
            {
                GameManager.Instance.TriggerAction();
            }
            // 일반 대화가 이미 진행 중이면 Raycast와 관계없이 진행
            else if (GameManager.Instance.gameData.isAction)
            {
                GameManager.Instance.Action();
            }
            // 새로운 오브젝트와 상호작용
            else if (obj != null && rayhit.collider != null)
            {
                if (rayhit.collider.CompareTag("Structure") ||
                    rayhit.collider.CompareTag("Carried"))
                {
                    GameManager.Instance.gameData.scanObject = obj;
                    GameManager.Instance.Action();
                }
                else if (rayhit.collider.CompareTag("Door"))
                {
                    doorInteraction =
                        rayhit.collider.GetComponent<DoorInteraction>();

                    if (doorInteraction != null)
                    {
                        doorInteraction.Activate();
                    }
                }
                else if (rayhit.collider.CompareTag("Movement"))
                {
                    StartCoroutine(Move());
                }
            }
        }
        // pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.TogglePause();
        }

        // grab 상태일때는 raycast의 시선이 고정되게
        // grab 상태에서 시선이 고정되기 떄문에 rayhit도 고정시키기
        if (isGrabbing)
        {
            rayhit = prevRayHit;
        }
        else
        {
            // layer가 object인 오브젝트가 플레이어의 앞에 있는지 확인
            rayhit = Physics2D.Raycast(rb.position, vecDir, 0.7f, LayerMask.GetMask("Object"));
        }


        if (rayhit.collider != null)
        {   // 있으면 그 오브젝트를 obj에 저장
            obj = rayhit.collider.gameObject;

            if (rayhit.collider.CompareTag("Switch")) return;
            //옮길 수 있는 obj인지 확인
            if (rayhit.collider.CompareTag("Carried"))
            {   //grab key down && grab cooltime
                if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastGrabTime + playerData.grabDelay)
                {   //player와 obj의 간격 계산
                    distancePlayerObj = obj.transform.position - transform.position;

                    //보는 방향 감지 후 물체 위치 재정의
                    rayDirection = -rayhit.normal;

                    if (rayDirection.y == 0)
                    {   //obj를 playser position 축에 맞추기
                        distancePlayerObj = new Vector3(distancePlayerObj.x, 0, 0);
                    }
                    else
                    {   //playser position 축에 맞추기
                        distancePlayerObj = new Vector3(0, distancePlayerObj.y, 0);
                    }
                    if (!isGrabbing)
                    {
                        prevDirection = playerData.hitBox.direction;
                        Grab();
                    }

                    else
                    {
                        Drop();
                    }
                }

            }


        }
        else
        { // raycast에 물체가 없으면 obj는 null
            obj = null;
        }

        if (isGrabbing && obj != null)
        {   //물체가 player 옆에 위치
            Vector3 transPos = transform.position + distancePlayerObj;

            obj.transform.position = Vector3.Lerp(obj.transform.position, transPos, Time.deltaTime * 10f);
        }

    }

    void FixedUpdate()
    {
        if (isAutoMoving) return;
        // 이동 방향 설정 및 이동 구현
        Vector2 moveDir;
        if (isHorizontalMove)
        {
            moveDir = new Vector2(h, 0);
        }
        else
        {
            moveDir = new Vector2(0, v);
        }
        rb.velocity = moveDir * moveSpeed;

        //raycast debug
        // Debug.DrawRay(rb.position, vecDir * 0.7f, Color.green);

    }

    void Grab()
    {
        isGrabbing = true;
        prevRayHit = rayhit;

        //anim 출력
        playerData.anim.SetBool("isGrabbing", true);

        // 플레이어의 자식으로 바꾸기 전에 원래 부모 저장
        originalParent = obj.transform.parent;
        //obj를 player에 상속
        //grabobj를 쉽게 관리 하기 위함
        obj.transform.SetParent(this.transform);

        //objcolider remove
        objCol = obj.GetComponent<BoxCollider2D>();
        objCol.enabled = false;
        Rigidbody2D grabRb = obj.GetComponent<Rigidbody2D>();
        if (grabRb != null)
        {

            grabRb.bodyType = RigidbodyType2D.Kinematic;
            grabRb.velocity = Vector2.zero;

        }


        obj.transform.position = Vector2.MoveTowards(obj.transform.position, transform.position, 0.1f);

        //grab시 히트박스 재정의
        ChangeHitBox();

    }


    void Drop()
    {
        //값들 초기화
        isGrabbing = false;
        prevRayHit = default;
        playerData.anim.SetBool("isGrabbing", false);
        obj.transform.SetParent(originalParent, true);

        //물체도 초기화
        Rigidbody2D grabRb = obj.GetComponent<Rigidbody2D>();
        if (grabRb != null)
        {
            grabRb.bodyType = RigidbodyType2D.Static;
        }
        //값 초기화
        InitHitBox();
        objCol.enabled = true;
        obj = null;
        objCol = null;
        //grab cooltime calc
        lastGrabTime = Time.time;

    }

    void ChangeHitBox()
    {
        prevSize = playerData.hitBox.size;
        prevOffset = playerData.hitBox.offset;

        if (obj == null) return;

        if (rayDirection.y == 0)
        {
            playerData.hitBox.direction =
                CapsuleDirection2D.Horizontal;

            playerData.hitBox.offset =
                rayDirection.x > 0
                ? new Vector2(0.5f, 0f)
                : new Vector2(-0.5f, 0f);

            playerData.hitBox.size =
                new Vector2(1.8f, 0.9f);
        }
        else
        {
            playerData.hitBox.direction =
                CapsuleDirection2D.Vertical;

            playerData.hitBox.offset =
                rayDirection.y > 0
                ? new Vector2(0f, 0.5f)
                : new Vector2(0f, -0.5f);

            playerData.hitBox.size =
                new Vector2(0.9f, 1.8f);
        }
    }

    void InitHitBox()
    {   //player hitbox init
        playerData.hitBox.offset = prevOffset;
        playerData.hitBox.size = prevSize;
        playerData.hitBox.direction = prevDirection;
    }

    // 방과 방사이를 이동할 때 사용하는 함수
    IEnumerator Move()
    {

        yield return StartCoroutine(FadeManager.Instance.FadeOut(0.5f));

        yield return StartCoroutine(FadeManager.Instance.FadeIn(0.5f));
    }

    public void StopMovement()
    {
        h = 0;
        v = 0;

        rb.velocity = Vector2.zero;

        if (playerData.anim != null)
        {
            playerData.anim.SetInteger("hAxisRaw", 0);
            playerData.anim.SetInteger("vAxisRaw", 0);
            playerData.anim.SetBool("isChange", true);
        }
    }

    public IEnumerator WalkToPosition(
        Vector3 targetPosition,
        float autoMoveSpeed
    )
    {
        isAutoMoving = true;
        rb.velocity = Vector2.zero;

        if (playerData.anim != null)
        {
            playerData.anim.SetBool(
                "isChange",
                false
            );
        }

        yield return null;

        while (Vector2.Distance(
            rb.position,
            targetPosition
        ) > 0.02f)
        {
            Vector2 currentPosition = rb.position;

            Vector2 difference =
                (Vector2)targetPosition - currentPosition;

            Vector2 direction;

            if (Mathf.Abs(difference.x) >
                Mathf.Abs(difference.y))
            {
                direction = new Vector2(
                    Mathf.Sign(difference.x),
                    0
                );
            }
            else
            {
                direction = new Vector2(
                    0,
                    Mathf.Sign(difference.y)
                );
            }

            UpdateCutSceneWalkAnimation(
                direction
            );

            Vector2 nextPosition;

            if (direction.x != 0)
            {
                float nextX = Mathf.MoveTowards(
                    currentPosition.x,
                    targetPosition.x,
                    autoMoveSpeed *
                    Time.fixedDeltaTime
                );

                nextPosition = new Vector2(
                    nextX,
                    currentPosition.y
                );
            }
            else
            {
                float nextY = Mathf.MoveTowards(
                    currentPosition.y,
                    targetPosition.y,
                    autoMoveSpeed *
                    Time.fixedDeltaTime
                );

                nextPosition = new Vector2(
                    currentPosition.x,
                    nextY
                );
            }

            rb.MovePosition(nextPosition);

            yield return new WaitForFixedUpdate();
        }

        rb.position = targetPosition;
        rb.velocity = Vector2.zero;

        isAutoMoving = false;

        StopMovement();
    }
    private void UpdateCutSceneWalkAnimation(
        Vector2 direction
    )
    {
        if (playerData.anim == null)
            return;

        int horizontal = 0;
        int vertical = 0;

        if (direction.x != 0)
        {
            horizontal =
                direction.x > 0 ? 1 : -1;
        }
        else if (direction.y != 0)
        {
            vertical =
                direction.y > 0 ? 1 : -1;
        }

        int currentH =
            playerData.anim.GetInteger(
                "hAxisRaw"
            );

        int currentV =
            playerData.anim.GetInteger(
                "vAxisRaw"
            );

        if (currentH != horizontal ||
            currentV != vertical)
        {
            playerData.anim.SetBool(
                "isChange",
                true
            );

            playerData.anim.SetInteger(
                "hAxisRaw",
                horizontal
            );

            playerData.anim.SetInteger(
                "vAxisRaw",
                vertical
            );
        }
        else
        {
            playerData.anim.SetBool(
                "isChange",
                false
            );
        }
    }
}
