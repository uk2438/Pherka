using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter4Manager : Singleton<Chapter4Manager>
{
    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;

    private bool Chapter1ClearCheck;
    public void CheckWasAction(ObjectData objectData)
    {
        int id = objectData.GetCurrentDialogueId();


    }

    public void SevenOClock()
    {
        Chapter1Manager.Instance.SetHomeTime(7);
    }

    public void SetChapter1ClearCheck(bool b)
    {
        Chapter1ClearCheck = b;
        GetObjectData(104).SetDialogueCondition(b);
    }

    public bool GetChapter1ClearCheck()
    {
        return Chapter1ClearCheck;
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
}
