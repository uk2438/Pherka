using System;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class Chapter1Manager : Singleton<Chapter1Manager>
{
    [Serializable]
    private class Group
    {
        public Transform[] transforms;
    }

    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;
    [Header("Player 텔레포트 위치")]
    [SerializeField] private Vector3 Chpater1Map;
    [Header("Pherka Objects")]
    [SerializeField] private Transform Pherka1;
    [SerializeField] private Transform Pherka2;
    [SerializeField] private Transform Pherka3;
    [SerializeField] private Transform Pherka4;
    [SerializeField] private Transform Pherka5;
    [SerializeField] private Transform Pherka6;
    [Header("메인 퍼즐 objects")]
    [SerializeField] private Transform[] potTransforms;
    [SerializeField] private Transform[] certain0Transforms;
    [SerializeField] private Transform[] certain1Transforms;

    private bool FirstToSecond = false;
    private bool startPuzzle = false;
    private int homeTime = 1;
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
                if(data != null)
                    data.transform.Find("Outline").gameObject.SetActive(true);
                break;

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
            if (data.GetCurrentDialogueId() == id) return data;
        }

        return null;
    }

    private void UpdateState(int t1, int t2, Transform[] transforms, bool satisfied)
    {
        int time = GetHomeTime();
        bool isIntact = (t1 <= time && time <= t2) || satisfied;
        
        transforms[0].gameObject.SetActive(isIntact);
        transforms[1].gameObject.SetActive(!isIntact);
    }

    private void UpdateState(int t, Transform transform, bool satisfied)
    {
        bool isIntact = GetHomeTime() == t || satisfied;

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

    }



    public int GetHomeTime()
    {
        return homeTime;
    }
}
