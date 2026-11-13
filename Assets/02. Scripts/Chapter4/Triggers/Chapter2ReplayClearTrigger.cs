using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2ReplayClearTrigger : MonoBehaviour
{
    ObjectData triggerObjectData;
    BackStepFunc backStep;

    void Awake()
    {
        backStep = GetComponent<BackStepFunc>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        triggerObjectData = GetComponent<ObjectData>();

        if(!Chapter4Manager.Instance.GetChapter2ClearCheck()) {
            backStep.BackStep(other.transform, backStep.stepDistance);
        }
        else
        {
            triggerObjectData.SetDialogueCondition(true);
        }

        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();
    }
}
