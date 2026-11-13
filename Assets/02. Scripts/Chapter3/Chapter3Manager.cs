using System;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Manager : Singleton<Chapter3Manager>
{
    [Header("조건 충족 시 Condition 바꿔야 할 ObjectData 목록")]
    [SerializeField] private ObjectData[] ConditionData;

    [Header("클리어시 나타나는 Doors")]
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject[] walls;
    [Header("stage3 puzzle activation")]
    [SerializeField] private Transform[] documents;
    [SerializeField] private GameObject pileOfDocumentsEffect;
    [SerializeField] private Transform pileOfDocuments;

    [Header("stage4 puzzle activation")]
    [SerializeField] private GameObject[] furnitures;

    [Serializable]
    private class Puzzle1Pair
    {
        public SpriteRenderer spriteRenderer;
        public bool condition;
    }
    [SerializeField] private Sprite goodGrade, poorGrade;
    [SerializeField] private ComputerPuzzle computerPuzzle;
    [HideInInspector] public bool start1, start2, start3;
    [HideInInspector] public bool puzzle1, puzzle2, puzzle3;

    [SerializeField] private List<Puzzle1Pair> pair = new List<Puzzle1Pair>();

    [Header("Chapter4 관련 Objects")]
    [SerializeField] private GameObject Chapter4Obj;
    [SerializeField] private GameObject[] DeactiveObj;
    [SerializeField] private ObjectData[] changeConditions;
    private int isCheckedDocuments = 0;
    private bool[] isCheckedFurnitures;
    private string structureTag = "Structure";

    private void Awake() {
        isCheckedFurnitures = new bool[furnitures.Length-1];
    }
    public void CheckWasAction(ObjectData objectData)
    {
        int id = objectData.GetCurrentDialogueId();

        if (4000 <= id && id <= 4005 && !puzzle1)
        {
            ChangeCondition(id);
            if (GradeAllSatisfied())
            {
                foreach (var item in pair)
                {
                    SetTag(item.spriteRenderer.gameObject, structureTag);
                }
                puzzle1 = true;
                GameManager.Instance.StartMonologue(22000, () =>
                {
                    doors[1].SetActive(true);
                    walls[1].SetActive(false);

                });
                GetObjectData(4006).SetDialogueCondition(true);
            }
        }
        else if (4016 <= id && id <= 4020)
        {
            GameManager.Instance.SetDialogueFinishedCallback(() =>
            {
                objectData.gameObject.SetActive(false);
                isCheckedDocuments++;

                if (isCheckedDocuments >= 8)
                {
                    GameManager.Instance.StartMonologue(22003, () =>
                    {
                        pileOfDocumentsEffect.SetActive(true);
                    });
                }
            });
        }
        else if(4023 <= id && id <= 4026 && !GetAllCheckedFurnitures())
        {
            int offset = id - 4023;
            if(isCheckedFurnitures[offset]) return;

            isCheckedFurnitures[offset] = true;

            if(GetAllCheckedFurnitures()) GameManager.Instance.SetDialogueFinishedCallback(() =>
            {
                GameManager.Instance.StartMonologue(22004);
                ObjectData data = GetObjectData(4022);
                data.SetDialogueCondition(true);
                SetTag(data.gameObject, "Director");
            });
        }
        else
        {
            switch (id)
            {
                case 4006:
                    start1 = true;
                    break;
                case 4007:
                    GetObjectData(4008).SetDialogueCondition(true);
                    GameManager.Instance.SetDialogueFinishedCallback(() =>
                    {
                        doors[0].SetActive(true);
                        walls[0].SetActive(false);

                    });
                    break;
                case 4010:
                    SetTag(GetObjectData(4012).gameObject, structureTag);
                    break;
                case 4011:
                    GameManager.Instance.SetDialogueFinishedCallback(() =>
                    {
                        doors[2].SetActive(true);
                        walls[2].SetActive(false);

                    });
                    break;
                case 4014:
                    if (!start3)
                    {
                        foreach (Transform transform in documents)
                        {
                            SetTag(transform.gameObject, structureTag);
                            transform.Find("ShinyEffect").gameObject.SetActive(true);
                            start3 = true;
                        }
                    }
                    break;
                case 4015:
                    GameManager.Instance.SetDialogueFinishedCallback(() =>
                    {
                        doors[3].SetActive(true);
                        walls[3].SetActive(false);
                    });
                    break;
                case 4022:
                    foreach(GameObject obj in furnitures)
                    {
                        SetTag(obj, structureTag);
                    }
                    break;
                case 13000:
                    GetObjectData(4014).SetDialogueCondition(true);
                    break;
                case 13001:
                    SetTag(objectData.gameObject, structureTag);
                    break;


            }
        }

        if (start1)
        {
            ObjectData data = GetObjectData(4008);

            if (data != null)
                data.GetComponent<BoxCollider2D>().enabled = true;
        }



    }

    public void ShowComputerPuzzle()
    {
        StartCoroutine(computerPuzzle.ShowPuzzle());
    }

    public void InputComputerPuzzle()
    {
        computerPuzzle.OpenInputAfterDialogue(); ;
    }

    public void StartChapter4()
    {
        Chapter4Obj.SetActive(true);
        foreach(GameObject obj in DeactiveObj){
            if(obj == null) break;
            obj.SetActive(false);
        }
        
        foreach(ObjectData data in changeConditions){
            if(data == null) break;
            data.SetDialogueCondition(true);
        }

        Chapter1Manager.Instance.SetHomeTime(1);
        Chapter2Manager.Instance.SetRestTime(false);

        DialogueManager.Instance.SetPresentChapter(4);
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

    private void ChangeCondition(int id)
    {

        switch (id)
        {
            case 4000:
                pair[0].condition = !pair[0].condition;
                pair[5].condition = !pair[5].condition;
                break;
            case 4001:
                pair[1].condition = !pair[1].condition;
                pair[3].condition = !pair[3].condition;
                break;
            case 4002:
                pair[0].condition = !pair[0].condition;
                pair[2].condition = !pair[2].condition;
                break;
            case 4003:
                pair[1].condition = !pair[1].condition;
                pair[3].condition = !pair[3].condition;
                pair[4].condition = !pair[4].condition;
                break;
            case 4004:
                pair[2].condition = !pair[2].condition;
                pair[4].condition = !pair[4].condition;
                break;
            case 4005:
                pair[2].condition = !pair[2].condition;
                pair[3].condition = !pair[3].condition;
                pair[5].condition = !pair[5].condition;
                break;
            default:
                break;
        }

        ChangeSprite();
    }

    private void ChangeSprite()
    {
        foreach (var item in pair)
        {
            if (item.condition) item.spriteRenderer.sprite = goodGrade;
            else item.spriteRenderer.sprite = poorGrade;
        }
    }
    private bool GradeAllSatisfied()
    {
        foreach (var item in pair)
        {
            if (!item.condition) return false;
        }
        return true;

    }

    private void SetTag(GameObject obj, string tag)
    {
        obj.tag = tag;
    }

    private bool GetAllCheckedFurnitures()
    {
        foreach(bool b in isCheckedFurnitures)
        {
            if(!b) return false;
        }
        return true;
    }
}
