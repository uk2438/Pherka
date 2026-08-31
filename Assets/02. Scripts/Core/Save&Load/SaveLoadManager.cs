using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    [Serializable]
    public class ObjectState
    {
        // 저장 대상 오브젝트를 구분하는 고유 ID
        public int saveId;

        // Transform 상태
        public Vector3 position;
        public Quaternion rotation;

        // 오브젝트 상태
        public string tag;
        public bool isActive;

        // 대화 진행 상태
        public int dialogueIndex;
        public bool dialogueConditionSatisfied;
    }

    [Serializable]
    public class Wrapper
    {
        public List<ObjectState> objects;
    }

    public void SaveGame()
    {
        List<ObjectState> data = CollectSavableObjects();

        Wrapper wrapper = new Wrapper
        {
            objects = data
        };

        string json = JsonUtility.ToJson(wrapper, true);
        string path = Path.Combine(Application.persistentDataPath, "save.json");

        try
        {
            File.WriteAllText(path, json);
            Debug.Log($"저장 완료: {path}");
        }
        catch (Exception exception)
        {
            Debug.LogError($"저장 실패: {exception.Message}");
        }
    }

    public void LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning($"저장 파일이 없습니다: {path}");
            return;
        }

        try
        {
            string json = File.ReadAllText(path);
            Wrapper loaded = JsonUtility.FromJson<Wrapper>(json);

            if (loaded == null || loaded.objects == null)
            {
                Debug.LogWarning("저장 데이터를 불러올 수 없습니다.");
                return;
            }

            RestoreObjectStates(loaded.objects);

            Debug.Log("불러오기 완료");
        }
        catch (Exception exception)
        {
            Debug.LogError($"불러오기 실패: {exception.Message}");
        }
    }

    private List<ObjectState> CollectSavableObjects()
    {
        List<ObjectState> result = new List<ObjectState>();

        /*
         * Unity 2022 방식.
         * 비활성화된 GameObject도 검색 대상에 포함합니다.
         */
        ObjectData[] allObjects = FindObjectsByType<ObjectData>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        HashSet<int> usedSaveIds = new HashSet<int>();

        foreach (ObjectData obj in allObjects)
        {
            if (obj == null)
                continue;

            bool isMovableObject =
                obj.CompareTag("Carried") ||
                obj.CompareTag("Usable");

            bool hasDefaultDialogue =
                obj.defaultDialogueIds != null &&
                obj.defaultDialogueIds.Length > 0;

            bool hasSatisfyDialogue =
                obj.satisfyDialogueIds != null &&
                obj.satisfyDialogueIds.Length > 0;

            bool hasDialogue =
                hasDefaultDialogue ||
                hasSatisfyDialogue;

            /*
             * 이동 가능한 오브젝트도 아니고
             * 대화 상태를 가진 오브젝트도 아니라면 저장하지 않습니다.
             */
            if (!isMovableObject && !hasDialogue)
                continue;

            if (obj.SaveId < 0)
            {
                Debug.LogWarning(
                    $"{obj.gameObject.name}의 saveId가 설정되지 않았습니다.",
                    obj
                );

                continue;
            }

            if (!usedSaveIds.Add(obj.SaveId))
            {
                Debug.LogWarning(
                    $"중복된 saveId가 발견되었습니다: {obj.SaveId}",
                    obj
                );

                continue;
            }

            ObjectState state = new ObjectState
            {
                saveId = obj.SaveId,

                position = obj.transform.position,
                rotation = obj.transform.rotation,

                tag = obj.tag,
                isActive = obj.gameObject.activeSelf,

                dialogueIndex = obj.GetDialogueIndex(),
                dialogueConditionSatisfied =
                    obj.IsDialogueConditionSatisfied()
            };

            result.Add(state);
        }

        return result;
    }

    private void RestoreObjectStates(List<ObjectState> states)
    {
        if (states == null)
            return;

        /*
         * 비활성화된 GameObject도 찾아야
         * 저장 당시 꺼져 있던 오브젝트를 다시 복원할 수 있습니다.
         */
        ObjectData[] allObjects = FindObjectsByType<ObjectData>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        Dictionary<int, ObjectData> lookup =
            new Dictionary<int, ObjectData>();

        foreach (ObjectData obj in allObjects)
        {
            if (obj == null)
                continue;

            if (obj.SaveId < 0)
                continue;

            if (lookup.ContainsKey(obj.SaveId))
            {
                Debug.LogWarning(
                    $"중복된 saveId 발견: {obj.SaveId}",
                    obj
                );

                continue;
            }

            lookup.Add(obj.SaveId, obj);
        }

        foreach (ObjectState state in states)
        {
            if (!lookup.TryGetValue(state.saveId, out ObjectData obj))
            {
                Debug.LogWarning(
                    $"saveId {state.saveId}에 해당하는 오브젝트를 찾지 못했습니다."
                );

                continue;
            }

            /*
             * SetActive를 먼저 false로 만들면
             * 이후 상태 처리 과정이 꼬일 수 있으므로 마지막에 적용합니다.
             */

            obj.transform.position = state.position;
            obj.transform.rotation = state.rotation;

            /*
             * SetDialogueCondition()에서
             * dialogueIndex가 0으로 초기화되므로
             * 조건을 먼저 복원한 후 인덱스를 복원해야 합니다.
             */
            obj.SetDialogueCondition(
                state.dialogueConditionSatisfied
            );

            obj.SetDialogueIndex(
                state.dialogueIndex
            );

            // 저장 당시 활성화 상태를 마지막에 복원
            obj.gameObject.SetActive(
                state.isActive
            );
        }
    }

    public bool HasSaveFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.json");
        return File.Exists(path);
    }

    public void DeleteSaveFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning("삭제할 저장 파일이 없습니다.");
            return;
        }

        try
        {
            File.Delete(path);
            Debug.Log($"저장 파일 삭제 완료: {path}");
        }
        catch (Exception exception)
        {
            Debug.LogError($"저장 파일 삭제 실패: {exception.Message}");
        }
    }
}