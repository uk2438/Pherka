using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.XR;
using UnityEngine;

public class PrologueManager : Singleton<PrologueManager>
{
    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;

    [Header("진행 순간이동 위치")]
    [SerializeField] private Vector3 FirstGoToWork;
    [SerializeField] private Vector3 SecondGoToWork;
    [SerializeField] private Vector3 GoToHome;

    [Header("나타나게 할 오브젝트들")]
    [SerializeField] private GameObject martNPC;
    [SerializeField] private GameObject[] martShinyEffects;
    [SerializeField] private GameObject square;
    [SerializeField] public GameObject homeBoxs, dogFood;

    [Header("프롤로그 맵 Deactive")]
    [SerializeField] private GameObject deactiveObj;
    [SerializeField] private GameObject activeObj;
    
    //B1F 책 읽었는지에 대한 bool
    private bool id1006, id1007, id1008, id1009, id1010, id1011, id1012, id1013, isCompleted;
    //Mart Check bool
    private bool id1032, id1033, id1034, id1035, id1036, id1037, id1038, id1039, id1040, id1041, id1042;
    //계단 트리거 bool
    private bool FirstToSecond, FirstToBasement, FirstWorkPass;



    public void CheckWasAction(ObjectData objectData)
    {
        int id = objectData.GetCurrentDialogueId();

        //B1 책 한번씩 읽었는지 확인하는 트리거
        if (1006 <= id && id <= 1013)
        {

            GameObject shinyEffect = objectData.transform.Find("ShinyEffect").gameObject;

            if (shinyEffect == null) return;

            shinyEffect.SetActive(false);

            switch (id)
            {
                case 1006:
                    id1006 = true;
                    break;
                case 1007:
                    id1007 = true;
                    break;
                case 1008:
                    id1008 = true;
                    break;
                case 1009:
                    id1009 = true;
                    break;
                case 1010:
                    id1010 = true;
                    break;
                case 1011:
                    id1011 = true;
                    break;
                case 1012:
                    id1012 = true;
                    break;
                case 1013:
                    id1013 = true;
                    break;

                default: break;
            }

            if (CheckBooks() && !isCompleted)
            {
                ObjectData tmpdata = GetObjectData(5003);
                if (tmpdata == null)
                    return;

                isCompleted = true;
                GameManager.Instance.SetDialogueFinishedCallback(() =>
                {
                    tmpdata.SetDialogueCondition(true);
                    GameManager.Instance.StartMonologue(20003);
                }
                );
            }
        }
        else if (id == 5002)
        {
            FirstToBasement = true;
        }
        else if (id == 5004)
        {
            FirstToSecond = true;
        }
        else if (1032 <= id && id <= 1042)
        {
            GameObject shinyEffect = objectData.transform.Find("ShinyEffect").gameObject;

            if (shinyEffect == null) return;

            shinyEffect.SetActive(false);

            switch (id)
            {
                case 1032:
                    id1032 = true;
                    break;
                case 1033:
                    id1033 = true;
                    break;
                case 1034:
                    id1034 = true;
                    break;
                case 1035:
                    id1035 = true;
                    break;
                case 1036:
                    id1036 = true;
                    break;
                case 1037:
                    id1037 = true;
                    break;
                case 1038:
                    id1038 = true;
                    break;
                case 1039:
                    id1039 = true;
                    break;
                case 1040:
                    id1040 = true;
                    break;
                case 1041:
                    id1041 = true;
                    break;
                case 1042:
                    id1042 = true;
                    break;

                default: break;
            }

            if (CheckMart())
            {
                ObjectData tmpdata = GetObjectData(5013);
                if (tmpdata == null)
                    return;

                tmpdata.SetDialogueCondition(true);
            }

        }
        else if (id == 5016)
        {
            ChangeAllChildrenTag(homeBoxs, "Carried");
        }
        else if(id == 5018)
        {
            ChangeTag(dogFood, "Carried");
        }


    }

    private bool CheckBooks()
    {
        if (id1006 && id1007 && id1008 && id1009 && id1010 && id1011 && id1012 && id1013) return true;

        return false;
    }
    private bool CheckMart()
    {
        if(id1032&& id1033&& id1034&& id1035&& id1036&& id1037&& id1038&& id1039&& id1040&& id1041&& id1042) return true;

        return false;
    }

    private ObjectData GetObjectData(int id)
    {
        foreach (ObjectData data in ConditionData)
        {
            if (data.GetCurrentDialogueId() == id) return data;
        }

        return null;
    }

    public void ChangeTag(GameObject obj, string tag)
    {
        obj.tag = tag;
    }

    public void ChangeAllChildrenTag(GameObject parent, string tag)
    {
        foreach(Transform child in parent.GetComponentInChildren<Transform>(true))
        {
            if(child == parent.transform) continue;
            child.tag = tag;
        }
    }


    public Vector3 GetFirstGoToWork()
    {
        return FirstGoToWork;
    }

    public Vector3 GetSecondGoToWork()
    {
        return SecondGoToWork;
    }

    public Vector3 GetGoToHome()
    {
        return GoToHome;
    }

    public bool GetFirstToSecond()
    {
        return FirstToSecond;
    }

    public bool GetFirstToBasement()
    {
        return FirstToBasement;
    }

    public void SetFistWorkPass(bool boolean)
    {
        FirstWorkPass = boolean;
    }

    public bool GetFirstWorkPass()
    {
        return FirstWorkPass;
    }

    

    public IEnumerator SetMartNPCActive(bool boolean)
    {
        martNPC.SetActive(boolean);
        foreach (GameObject obj in martShinyEffects)
        {
            ObjectData data = obj.GetComponent<ObjectData>();
            GameObject effect = obj.transform.Find("ShinyEffect").gameObject;
            data.SetDialogueCondition(boolean);
            effect.SetActive(boolean);
        }
        square.SetActive(boolean);
        yield return null;

    }

    public void StartChapterOne()
    {
        activeObj.SetActive(true);
    }

    public void EndPrologue()
    {
        deactiveObj.SetActive(false);
    }

}
