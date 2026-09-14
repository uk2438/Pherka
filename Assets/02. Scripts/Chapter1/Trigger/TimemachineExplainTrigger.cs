using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimemachineExplainTrigger : MonoBehaviour
{
    private ObjectData triggerObjectData;
    private bool isTriggered = false;

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
