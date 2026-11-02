using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1ReplayClearTrigger : MonoBehaviour
{
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

        if(!Chapter4Manager.Instance.GetChapter1ClearCheck()) 
            backStep.BackStep(other.transform, backStep.stepDistance);

        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();
    }
}
