using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoChapter3MapTrigger : MonoBehaviour
{
    ObjectData triggerObjectData;
    bool isTriggered;

    void Awake()
    {
        triggerObjectData = GetComponent<ObjectData>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        if(isTriggered) return;

        isTriggered = true;

        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();
    }
}
