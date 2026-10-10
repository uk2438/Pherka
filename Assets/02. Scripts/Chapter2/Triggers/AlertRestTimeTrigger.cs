using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertRestTimeTrigger : MonoBehaviour
{
    bool triggered;
    ObjectData triggerObjectData;

    void Awake()
    {
        triggerObjectData = GetComponent<ObjectData>();
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;

        if(!Chapter2Manager.Instance.GetRestTime()) return;
        
        if(triggered) return;

        triggered = true;

        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();
        
    }
}
