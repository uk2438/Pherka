using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1ClearTrigger : MonoBehaviour
{
    [SerializeField] GameObject Chapter2Block;
    ObjectData triggerObjectData;
    BackStepFunc backStep;

    void Awake()
    {
        triggerObjectData = GetComponent<ObjectData>();
        backStep = GetComponent<BackStepFunc>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Chapter1Manager.Instance.clear)
        {
            Chapter2Block.SetActive(false);
            Chapter1Manager.Instance.StartChapterTwo();
        }
        else
        {
            backStep.BackStep(other.transform, backStep.stepDistance);
        }

        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();
    }
}
