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
    public Animator anim;
    Vector3 vecDir;
    GameObject obj;
    Vector3 distancePlayerObj;
    public float grabDelay;
    float lastGrabTime;
    RaycastHit2D rayhit;
    RaycastHit2D prevRayHit = default;
    Vector2 rayDirection;
    public BoxCollider2D hitBox, objCol;
    Vector2 prevSize, prevOffset;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // get axis 방향키에따라 입력값이 -1, 0, 1로 받음
        // 쯔꾸르형 게임에선 대각선 이동이 안되도록 하기 위해서 수평과 수직 중 하나의 입력만 받도록 함
        // 대화창이 열려있을때에는 움직이지 못하게 하기
        if (GameManager.Instance.isAction || FadeManager.Instance.isFading || GameManager.Instance.isPause)
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
        // 현재 애니메이터가 들고 있는 값과 새로 입력된 값이 다를 때만 업데이트
        int curH = anim.GetInteger("hAxisRaw");
        int curV = anim.GetInteger("vAxisRaw");

        // 실제 애니메이터에 꽂아줄 목표 값 계산
        int targetH = isHorizontalMove ? (int)h : 0;
        int targetV = !isHorizontalMove ? (int)v : 0;

        if (curH != targetH || curV != targetV)
        {
            // 1. 값이 변했으므로 isChange를 true로 켜서 트랜지션 유도
            anim.SetBool("isChange", true);

            // 2. 바뀐 방향 값 전달
            anim.SetInteger("hAxisRaw", targetH);
            anim.SetInteger("vAxisRaw", targetV);
        }
        else
        {
            // 3. 값이 변하지 않은 상태(이미 걷는 중이거나 가만히 있는 중)라면 false
            anim.SetBool("isChange", false);
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
        if (Input.GetKeyDown(KeyCode.Space) && obj != null)
        {
            if (rayhit.collider.CompareTag("Structure") || rayhit.collider.CompareTag("Carried"))
            {
                GameManager.Instance.scanObject = obj;
                GameManager.Instance.Action();
            }

            else if (rayhit.collider.CompareTag("Movement"))
            {
                StartCoroutine(Move());
            }


        }
        // pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
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
                if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastGrabTime + grabDelay)
                {   //player와 obj의 간격 계산
                    distancePlayerObj = obj.transform.position - transform.position;

                    //보는 방향 감지 후 물체 위치 재정의
                    rayDirection = -rayhit.normal;

                    if (rayDirection.y == 0)
                    {   //obj를 playser position 축에 맞추기
                        distancePlayerObj = new Vector3(distancePlayerObj.x, 0, 0);

                        // if (distancePlayerObj.x < 0)
                        // {   //1. 초기 개발 당시 obj와 player collider conflict 방지를 위해 offset만큼 띄어줌
                        //     //2. 지금은 obj의 collider를 제거한 후 player의 collider를 2배로 증가 하는 식으로 개발
                        //     distancePlayerObj += new Vector3(-offset, 0, 0);
                        // }
                        // else if (distancePlayerObj.x > 0)
                        // {
                        //     distancePlayerObj += new Vector3(offset, 0, 0);
                        // }
                    }
                    else
                    {   //playser position 축에 맞추기
                        distancePlayerObj = new Vector3(0, distancePlayerObj.y, 0);

                        // if (distancePlayerObj.y < 0)
                        // {
                        //     distancePlayerObj += new Vector3(0, -offset, 0);
                        // }
                        // else if (distancePlayerObj.y > 0)
                        // {
                        //     distancePlayerObj += new Vector3(0, offset, 0);
                        // }
                    }
                    if (!isGrabbing)
                        Grab();


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
        anim.SetBool("isGrabbing", true);

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
        anim.SetBool("isGrabbing", false);
        obj.transform.SetParent(null);

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
    {   //물건의 상하좌우를 판별 후 히트박스 재정의
        //player의 히트박스를 크게하는 형식으로 개발
        prevSize = hitBox.size;
        prevOffset = hitBox.offset;
        if (obj != null)
        {
            if (rayDirection.y == 0)
            {
                if (rayDirection.x > 0)
                {
                    hitBox.offset = new Vector2(0.5f, 0f);
                }
                else
                {
                    hitBox.offset = new Vector2(-0.5f, 0f);
                }
                hitBox.size = new Vector2(1.8f, 0.9f);
            }
            else
            {
                if (rayDirection.y > 0)
                {
                    hitBox.offset = new Vector2(0f, 0.5f);
                }
                else
                {
                    hitBox.offset = new Vector2(0f, -0.5f);
                }
                hitBox.size = new Vector2(0.9f, 1.8f);
            }
        }

    }

    void InitHitBox()
    {   //player hitbox init
        hitBox.offset = prevOffset;
        hitBox.size = prevSize;
    }

    IEnumerator Move()
    {
        rayDirection = -rayhit.normal;

        MovementData objMovementData = obj.GetComponent<MovementData>();

        float xOffset = objMovementData.hOffset;
        float yOffset = objMovementData.vOffset;
        Vector3 originPos = this.transform.position;
        yield return StartCoroutine(FadeManager.Instance.FadeOut(0.5f));
        if (rayDirection == Vector2.up)
        {
            this.transform.position = new Vector3(originPos.x, originPos.y + yOffset, originPos.z);
        }
        else if (rayDirection == Vector2.down)
        {
            this.transform.position = new Vector3(originPos.x, originPos.y - yOffset, originPos.z);
        }
        else if (rayDirection == Vector2.left)
        {
            this.transform.position = new Vector3(originPos.x + xOffset, originPos.y, originPos.z);
        }
        else if (rayDirection == Vector2.down)
        {
            this.transform.position = new Vector3(originPos.x - xOffset, originPos.y, originPos.z);
        }
        yield return StartCoroutine(FadeManager.Instance.FadeIn(0.5f));
    }
}
