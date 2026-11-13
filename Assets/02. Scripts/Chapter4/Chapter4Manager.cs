using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter4Manager : Singleton<Chapter4Manager>
{
    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;
    [Header("MainStreet Block Triggers")]
    [SerializeField] private GameObject block0;
    [SerializeField] private GameObject block1;

    [Header("Clear시 활성화 오브젝트")]
    [SerializeField] private GameObject clearBlcok0;
    [SerializeField] private GameObject clearBlcok1;
    private bool chapter1ClearCheck;
    private string structureTag = "Structure";

    private bool artClear, gymClear, chapter2ClearCheck;
    public void CheckWasAction(ObjectData objectData)
    {
        int id = objectData.GetCurrentDialogueId();

        switch(id)
        {
            case 14002:
                GameManager.Instance.SetDialogueFinishedCallback(() =>
                {
                 objectData.SetDialogueCondition(true);
                SetTag(objectData.gameObject, structureTag);
                
                ObjectData data = GetObjectData(9001);
                data.SetDialogueCondition(true);
                SetTag(data.gameObject, structureTag);

                data = GetObjectData(3030);
                data.SetDialogueCondition(true);

                artClear = true;
                CheckChapter2Clear();
                });

                break;
        }

    }

    public void SevenOClock()
    {
        Chapter1Manager.Instance.SetHomeTime(7);
    }

    public void SetChapter1ClearCheck(bool b)
    {
        chapter1ClearCheck = b;
        block0.SetActive(!b);
        clearBlcok0.SetActive(b);
        
    }

    public bool GetChapter1ClearCheck()
    {
        return chapter1ClearCheck;
    }

    public bool GetChapter2ClearCheck()
    {
        return chapter2ClearCheck;
    }

    public void GymCutSceneEnd(int id)
    {
        ObjectData data = GetObjectData(id);

        if(data == null) Debug.Log("not exist");

        data.SetDialogueCondition(true);
        gymClear = true;
        CheckChapter2Clear();
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

    private void SetTag(GameObject gameObject, string tag)
    {
        gameObject.tag = tag;
    }

    private void CheckChapter2Clear()
    {
        if(artClear && gymClear && !chapter2ClearCheck) {
            chapter2ClearCheck = true;
            block1.SetActive(false);
            clearBlcok1.SetActive(true);
            GameManager.Instance.SetDialogueFinishedCallback(() =>
            {
                GameManager.Instance.StartMonologue(41000);
            });
        }
    }
}
