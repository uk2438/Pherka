using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Chapter2Manager : Singleton<Chapter2Manager>
{
    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;
    [Header("교실 문 태그 변경 목록")]
    [SerializeField] private Transform[] classroomDoors;
    [Header("텔레포트 활성화 유무")]
    [SerializeField] private BoxCollider2D[] teleports;
    [Header("복도 NPC")]
    [SerializeField] private Transform[] corridorNPC;
    [Header("체육복이 들어있는 사물함 키")]
    [SerializeField] private Transform lockerKey;

    private bool restTime, prevRestTime, getKey;

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

    public void SetSchoolTime(bool time)
    {
        prevRestTime = restTime;
        restTime = time;

        SetLockerKeyActive();
        SetActiveTeleport();
        SetActiveCorridorNPC();
        SetDoorTag();
    }
    public bool GetSchoolTime()
    {
        return restTime;
    }

    private void SetActiveTeleport()
    {
        foreach(BoxCollider2D box in teleports)
        {
            box.enabled = restTime;
        }
    }

    private void SetActiveCorridorNPC()
    {
        foreach(Transform transform in corridorNPC)
        {
            if(transform == null) return;
            transform.gameObject.SetActive(restTime);
        }
    }
    private void SetLockerKeyActive()
    {

        if(lockerKey == null) return;

        if(getKey){
            lockerKey.gameObject.SetActive(false);
            return;
        }

        if(prevRestTime && !restTime) lockerKey.gameObject.SetActive(true);
        else lockerKey.gameObject.SetActive(false);
    }
    private void SetDoorTag()
    {
        if (restTime)
        {
            foreach(Transform door in classroomDoors)
            {
                door.tag = "Door";
            }
        }
        else
        {
            foreach(Transform door in classroomDoors)
            {
                door.tag = "Structure";
            }
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

    
}
