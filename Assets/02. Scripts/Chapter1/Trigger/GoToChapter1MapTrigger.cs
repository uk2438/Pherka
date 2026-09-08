using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToChapter1MapTrigger : MonoBehaviour
{
    private bool isTriggered = false;
    private ObjectData triggerObjectData;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        if(isTriggered) return;

        isTriggered = true;
        triggerObjectData = GetComponent<ObjectData>();

        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();

    }
}
