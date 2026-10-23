using System;
using UnityEngine;

public class Chapter3Manager : MonoBehaviour
{
    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;
    public void CheckWasAction(ObjectData objectData)
    {
        int id = objectData.GetCurrentDialogueId();


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
