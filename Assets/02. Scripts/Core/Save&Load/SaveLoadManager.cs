using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
public class SaveLoadManager : Singleton<SaveLoadManager>
{
    [Serializable]
    public class ObjectState
    {
        public int id;
        public Vector3 position;
        public Quaternion rotation;
        public string tag;
    }

    [Serializable]
    public class Wrapper
    {   //json을 위한 wrapper
        public List<ObjectState> objects;
    }

    public void SaveGame()
    {
        var data = CollectSavableObjects();
        string json = JsonUtility.ToJson(new Wrapper {objects = data}, true);
        string path = Application.persistentDataPath + "/save.json";
        File.WriteAllText(path, json);
        Debug.Log(path);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/save.json";

        if(!File.Exists(path))
        {
            //추후 dialogue 추가
            return;
        }

        string json = File.ReadAllText(path);
        Wrapper loaded = JsonUtility.FromJson<Wrapper>(json);

        RestoreObjectStates(loaded.objects);

    }

    List<ObjectState> CollectSavableObjects()
    {
        var result = new List<ObjectState>();

        var allObjects = FindObjectsOfType<ObjectData>();

        foreach(var obj in allObjects)
        {
            if(obj.CompareTag("Carried") || obj.CompareTag("Usable"))
            {
                result.Add(new ObjectState
                {
                    id = obj.id,
                    position = obj.transform.position,
                    rotation = obj.transform.rotation,
                    tag = obj.tag
                });

            }
        }

        return result;
    }

    void RestoreObjectStates(List<ObjectState> states)
    {
        var allObjects = FindObjectsOfType<ObjectData>();
        var lookup = allObjects.ToDictionary(o => o.id, o => o);

        foreach (var state in states)
        {
            
            if(lookup.TryGetValue(state.id, out var obj))
            {
                obj.transform.position = state.position;
                obj.transform.rotation = state.rotation;
            }
        }
    }
}
