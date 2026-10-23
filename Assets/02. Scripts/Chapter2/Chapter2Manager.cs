using System;
using UnityEngine;

public class Chapter2Manager : Singleton<Chapter2Manager>
{
    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;

    [Header("교실 문 태그 변경 목록")]
    [SerializeField] private Transform[] classroomDoors;

    [Header("복도 NPC")]
    [SerializeField] private Transform[] corridorNPC;

    [Header("GymPherka 관련 Object")]
    [SerializeField] private Transform lockerKey;
    [SerializeField] private Transform locker;
    [SerializeField] private Sprite openLocker;
    public GameObject[] books;

    [Header("ArtroomPherka 관련 Object")]
    [SerializeField] private Sprite emptyCanvas;
    [SerializeField] private Sprite landscapeCanvas;

    [HideInInspector] public bool clear;

    private bool restTime, prevRestTime, getKey, firstCompleted, isBook, checkedGymPherka, checkedCanvas;
    private bool[] checkedBookPherka, checkedArtPherka, completed;

    private void Awake()
    {
        checkedBookPherka = new bool[2];
        checkedArtPherka = new bool[7];
        completed = new bool[3];
    }


    public void CheckWasAction(ObjectData objectData)
    {
        int id = objectData.GetCurrentDialogueId();

        switch (id)
        {
            case 7000:
                checkedBookPherka[0] = true;
                break;
            case 7006:
                checkedGymPherka = true;
                break;
            case 7009:
                checkedBookPherka[1] = true;
                break;
            case 3005:
                getKey = true;
                AcquireKey(objectData);
                break;
            case 3007:
                OpenLocker(objectData);
                break;
            case 3010:
                checkedArtPherka[0] = true;
                break;
            case 3011:
                checkedArtPherka[1] = true;
                break;
            case 3012:
                checkedArtPherka[2] = true;
                break;
            case 3013:
                checkedArtPherka[3] = true;
                break;
            case 3014:
                checkedArtPherka[4] = true;
                break;
            case 7003:
                checkedArtPherka[5] = true;
                break;
            case 7017:
                checkedArtPherka[6] = true;
                break;
            case 3015:
                ChangeEmptyCanvas(objectData);
                break;
            case 3016:
                ChangeLandscapeCanvas(objectData);

                break;



        }

        if (checkedBookPherka[0] && checkedBookPherka[1] && !isBook)
        {
            isBook = true;
            ObjectData book = GetObjectData(3002);

            if (book != null)
                book.SetDialogueCondition(true);
        }

        if (AllCheckArtPherka() && !checkedCanvas)
        {
            checkedCanvas = true;
            GameManager.Instance.SetDialogueFinishedCallback(() =>
            {
                ObjectData data = GetObjectData(3010);
                data.SetDialogueCondition(true);
                GameManager.Instance.StartMonologue(40004, () =>
                {
                    data.transform.Find("ShinyEffect").gameObject.SetActive(true);
                });
            },
            false
            );
        }

    }



    private ObjectData GetObjectData(int id)
    {
        foreach (ObjectData data in ConditionData)
        {
            if (data == null || data.defaultDialogueIds == null)
                continue;

            if (Array.IndexOf(data.defaultDialogueIds, id) >= 0)
                return data;
        }

        foreach (ObjectData data in ConditionData)
        {
            if (data == null || data.satisfyDialogueIds == null)
                continue;

            if (Array.IndexOf(data.satisfyDialogueIds, id) >= 0)
                return data;
        }

        return null;
    }

    public void SetRestTime(bool time)
    {
        prevRestTime = restTime;
        restTime = time;

        SetLockerKeyActive();
        SetActiveCorridorNPC();
        SetDoorTag();
    }
    public bool GetRestTime()
    {
        return restTime;
    }

    private void ChangeLandscapeCanvas(ObjectData objectData)
    {
        objectData.transform.GetComponent<SpriteRenderer>().sprite = landscapeCanvas;
        ObjectData data = GetObjectData(7004);
        data.SetDialogueCondition(true);
        SetClear(2);
        if (!GetFirstCompleted())
            SetFirstCompleted(true);
    }

    private void ChangeEmptyCanvas(ObjectData objectData)
    {
        GameManager.Instance.SetDialogueFinishedCallback(() =>
        {
            objectData.transform.Find("ShinyEffect").gameObject.SetActive(false);
            objectData.GetComponent<SpriteRenderer>().sprite = emptyCanvas;
        });

        ObjectData data = GetObjectData(3014);

        data.SetDialogueCondition(true);

    }

    private bool AllCheckArtPherka()
    {
        foreach (bool b in checkedArtPherka)
        {
            if (!b) return false;
        }

        return true;
    }

    private void AcquireKey(ObjectData obj)
    {
        obj.gameObject.SetActive(false);
        ObjectData data = GetObjectData(3006);
        data.SetDialogueCondition(true);
        data.transform.Find("ShinyEffect").gameObject.SetActive(true);
    }

    private void OpenLocker(ObjectData objectData)
    {
        objectData.transform.Find("ShinyEffect").gameObject.SetActive(false);

        SpriteRenderer lockerSprite = objectData.GetComponent<SpriteRenderer>();
        lockerSprite.sprite = openLocker;

        // 그림과 오브젝트를 월드 기준으로 0.3 올림
        Vector3 moveOffset = Vector3.up * 0.375f;
        objectData.transform.position += moveOffset;

        // Collider는 반대로 보정해서 기존 위치 유지
        BoxCollider2D boxCollider = objectData.GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            Vector3 localOffset = objectData.transform.InverseTransformVector(moveOffset);
            boxCollider.offset -= new Vector2(localOffset.x, localOffset.y);
        }
    }

    private void SetActiveCorridorNPC()
    {
        foreach (Transform transform in corridorNPC)
        {
            if (transform == null) return;
            transform.gameObject.SetActive(restTime);
        }
    }
    private void SetLockerKeyActive()
    {

        if (lockerKey == null) return;

        if (getKey)
        {
            lockerKey.gameObject.SetActive(false);
            return;
        }

        if (prevRestTime && !restTime && checkedGymPherka) lockerKey.gameObject.SetActive(true);
        else lockerKey.gameObject.SetActive(false);
    }
    private void SetDoorTag()
    {
        if (restTime)
        {
            foreach (Transform door in classroomDoors)
            {
                door.tag = "Door";
            }
        }
        else
        {
            foreach (Transform door in classroomDoors)
            {
                door.tag = "Structure";
            }
        }
    }

    public void ChangeAllTag(GameObject obj, string tag)
    {
        obj.tag = tag;

    }

    public void ChangeAllTag(GameObject[] objects, string tag)
    {
        foreach (GameObject obj in objects)
        {
            obj.tag = tag;
        }
    }


    public void SetGetKey(bool key)
    {
        getKey = key;
    }

    public bool GetGetKey()
    {
        return getKey;
    }

    public void SetFirstCompleted(bool completed)
    {
        firstCompleted = completed;
    }

    public bool GetFirstCompleted()
    {
        return firstCompleted;
    }

    public void SetClear(int i)
    {
        completed[i] = true;
        Clear();
    }

    public void Clear()
    {
        if (completed[0] && completed[1] && completed[2] && !clear)
        {
            clear = true;
            GameManager.Instance.SetDialogueFinishedCallback(() =>
            {
                GameManager.Instance.StartMonologue(40005);
                GetObjectData(104).SetDialogueCondition(true);

            });
        }
    }

}
