using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Manager : Singleton<Chapter1Manager>
{
    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;
    [Header("Player 텔레포트 위치")]
    [SerializeField] private Vector3 Chpater1Map;
    private bool FirstToSecond = false;
    public void CheckWasAction(ObjectData objectData)
    {
        int id = objectData.GetCurrentDialogueId();

        if (id == 11001)
        {
            //해당 object Condition은 CutScenePlayerBehavior에서 제어
            FirstToSecond = true;

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

    public Vector3 GetChapter1MapPosition()
    {
        return Chpater1Map;
    }
    
}
