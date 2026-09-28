using System;
using System.Collections.Generic;
using DialogueSystem;
using Unity.VisualScripting;
using UnityEngine;

public class Chapter1Manager : Singleton<Chapter1Manager>
{

    [Serializable]
    private class Condition
    {
        public Transform[] transforms;
        public int t1, t2;
    }

    [Header("AllCheck 시 나타나게할 Object")]
    [SerializeField] private GameObject outline;

    [Header("Choice 충족 시 나타나게 할 Object")]
    [SerializeField] private GameObject otherWateringCan;
    [SerializeField] private GameObject completedPhone;
    [HideInInspector] public bool isOpenBox;
    //3. wateringcan, pot, pherka3, outline
    [HideInInspector] public bool isPherka3, isPherka4, isPherka5, isPherka6, clear;

    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;
    [Header("Player 텔레포트 위치")]
    [SerializeField] private Vector3 Chpater1Map;
    [SerializeField] private Vector3 goToMainStreet;
    [Header("Pherka Objects")]
    [SerializeField] private Transform Pherka1;
    [SerializeField] private Transform Pherka2;
    [SerializeField] private Transform Pherka3;
    [SerializeField] private Transform Pherka4;
    [SerializeField] private Transform Pherka5;
    [SerializeField] private Transform Pherka6;
    [Header("메인 퍼즐 objects")]
    [SerializeField] private Condition box;
    [SerializeField] private Condition pot;
    [SerializeField] private Condition wateringCan;
    [SerializeField] private Condition curtain0;
    [SerializeField] private Condition curtain1;
    [SerializeField] private Condition waterCan;
    [SerializeField] private Condition painting;
    [SerializeField] private Condition phone;

    private bool FirstToSecond = false;
    private bool startPuzzle = false;
    private bool check2, check3, check4, check5, check6, puzzleClear;
    private bool[][] checks;
    private bool allCheck = false;
    private int homeTime = 1;
    // private bool curtainCheck = false;
    // private bool boxCheck = false;
    [HideInInspector] public bool tiedCurtains = false;
    [HideInInspector] public int completeIndex = 0;

    private void Awake()
    {
        checks = new bool[7][];

        checks[0] = new bool[7];

        for (int time = 1; time <= 6; time++)
        {
            if (1 <= time && time <= 4)
                checks[time] = new bool[2];
            else checks[time] = new bool[1];
        }
    }
    public void CheckWasAction(ObjectData objectData)
    {
        int id = objectData.GetCurrentDialogueId();
        ObjectData data = GetObjectData(id);

        switch (id)
        {
            case 11001:
                FirstToSecond = true;
                break;
            case 6006:
                startPuzzle = true;
                break;
            case 6008:
                checks[2][0] = true;
                break;
            case 2005:
                if (homeTime == 2)
                    checks[2][1] = true;
                break;
            case 6009:
                checks[3][0] = true;
                break;
            case 2008:
                if (homeTime == 3)
                    checks[3][1] = true;
                break;
            case 6010:
                checks[4][0] = true;
                break;
            case 2013:
                if (homeTime == 4)
                    checks[4][1] = true;
                break;
            case 6011:
                checks[5][0] = true;
                break;
            case 6012:
                checks[6][0] = true;
                break;
            case 2015:
                puzzleClear = true;
                break;
            case 6014:
                clear = true;
                break;


        }

        if(clear)
        {
            ObjectData clearTrigger = GetObjectData(104);
            clearTrigger.SetDialogueCondition(true);
        }

        if (2 <= homeTime && homeTime <= 4) SetCheckCondition(homeTime, checks[homeTime][0] && checks[homeTime][1]);
        else if (5 <= homeTime && homeTime <= 6) SetCheckCondition(homeTime, checks[homeTime][0]);

        if (check2 && check3 && check4 && check5 && check6) allCheck = true;

        if (!allCheck)
        {
            GameManager.Instance.SetDialogueFinishedCallback(() =>
            {
                if (GetCheckCondition(homeTime) && !checks[0][homeTime])
                {
                    checks[0][homeTime] = true;
                    GameManager.Instance.StartMonologue(21000);


                }
            },
            false
            );
        }
        else
        {
            GameManager.Instance.SetDialogueFinishedCallback(() =>
            {
                if (!checks[0][0])
                {
                    checks[0][0] = true;
                    GameManager.Instance.StartMonologue(21001);
                }
            },
            false
            );
        }
    }

    public bool GetFirstToSecond()
    {
        return FirstToSecond;
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

    private void UpdateState(Condition condition, bool satisfied)
    {
        int time = GetHomeTime();
        bool isIntact = (condition.t1 <= time && time <= condition.t2) || satisfied;

        condition.transforms[0].gameObject.SetActive(isIntact);
        condition.transforms[1].gameObject.SetActive(!isIntact);
    }

    private void UpdateState(int t, Transform transform, bool satisfied)
    {
        bool isIntact = GetHomeTime() == t;

        if (satisfied) isIntact = false;

        transform.gameObject.SetActive(isIntact);
    }

    public Vector3 GetChapter1MapPosition()
    {
        return Chpater1Map;
    }

    public bool GetStartPuzzle()
    {
        return startPuzzle;
    }
    public void SetHomeTime(int time)
    {
        homeTime = time;
        //parameter로 true || false로 적혀있는 내용은 초기 값을 나타냄
        //변수명으로 제어하게 바꿔야함
        if (!isOpenBox)
            UpdateState(box, false);
        else
        {
            foreach (Transform transform in box.transforms)
            {
                transform.gameObject.SetActive(false);
            }
            if (homeTime == 2)
            {
                otherWateringCan.SetActive(true);
            }
            else
            {
                otherWateringCan.SetActive(false);
            }
        }

        if (allCheck)
        {
            ObjectData boxData, curtainData, Pherka2Data, phoneData;

            boxData = GetObjectData(2005);
            if (boxData != null)
            {
                if (homeTime == 2) boxData.SetDialogueCondition(true);
                else boxData.SetDialogueCondition(false);
            }

            curtainData = GetObjectData(2008);

            if (curtainData != null)
            {
                if (homeTime == 3) curtainData.SetDialogueCondition(true);
                else curtainData.SetDialogueCondition(false);
            }

            Pherka2Data = GetObjectData(6008);
            if (Pherka2Data != null)
                Pherka2Data.SetDialogueCondition(true);

            UpdateState(2, outline.transform, isPherka3);

            phoneData = GetObjectData(2013);
            if (phoneData != null)
            {
                if (homeTime == 4) phoneData.SetDialogueCondition(true);
                else phoneData.SetDialogueCondition(false);
            }
        }

        if (isPherka3 && isPherka4)
        {
            foreach (Transform transform in phone.transforms)
            {
                transform.gameObject.SetActive(false);
            }
            if (homeTime == 4)
            {
                completedPhone.SetActive(true);
            }
            else
            {
                completedPhone.SetActive(false);
            }
        }
        else
        {
            UpdateState(phone, false);
        }

        if (puzzleClear)
        {
            ObjectData Pherka6Data = Pherka6.GetComponent<ObjectData>();
            Pherka6Data.SetDialogueCondition(true);
        }

        UpdateState(pot, isPherka3);
        UpdateState(wateringCan, isPherka3);
        UpdateState(curtain0, !tiedCurtains);
        UpdateState(curtain1, !tiedCurtains);
        UpdateState(waterCan, isPherka4);
        UpdateState(painting, isPherka4);


        UpdateState(1, Pherka1, false);
        UpdateState(2, Pherka2, false);
        UpdateState(3, Pherka3, isPherka3);
        UpdateState(4, Pherka4, isPherka4);
        UpdateState(5, Pherka5, isPherka3 && isPherka4);
        UpdateState(6, Pherka6, isPherka6);


    }



    public int GetHomeTime()
    {
        return homeTime;
    }

    public void SetCheckCondition(int time, bool boolean)
    {
        switch (time)
        {
            case 2:
                check2 = boolean;
                break;
            case 3:
                check3 = boolean;
                break;
            case 4:
                check4 = boolean;
                break;
            case 5:
                check5 = boolean;
                break;
            case 6:
                check6 = boolean;
                break;
            default:
                break;
        }
    }

    public bool GetCheckCondition(int time)
    {

        switch (time)
        {
            case 2:
                return check2;
            case 3:
                return check3;
            case 4:
                return check4;
            case 5:
                return check5;
            case 6:
                return check6;
        }

        return false;
    }

}
