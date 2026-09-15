using DialogueSystem;
using UnityEngine;

public class Chapter1Manager : Singleton<Chapter1Manager>
{
    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;
    [Header("Player 텔레포트 위치")]
    [SerializeField] private Vector3 Chpater1Map;
    private bool FirstToSecond = false;
    private bool startPuzzle = false;
    private int homeTIme = 1;
    public void CheckWasAction(ObjectData objectData)
    {
        int id = objectData.GetCurrentDialogueId();

        if (id == 11001)
        {
            //해당 object Condition은 CutScenePlayerBehavior에서 제어
            FirstToSecond = true;

        }
        else if(id == 6006)
        {
            startPuzzle = true;
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

    public bool GetStartPuzzle()
    {
        return startPuzzle;
    }
    public void SetHomeTime(int time)
    {
        homeTIme = time;
    }

    public int GetHomeTime()
    {
        return homeTIme;
    }
}
