using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMainStreetTrigger : MonoBehaviour
{
    private ObjectData triggerObjectData;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag("Player")) return;

        triggerObjectData = GetComponent<ObjectData>();

        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();
        Chapter3Manager.Instance.StartChapter4();
    }
}