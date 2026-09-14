using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMainPuzzleTrigger : MonoBehaviour
{
    private ObjectData triggerObjectData;
    private BackStepFunc backStep;

    void Awake()
    {
        triggerObjectData = GetComponent<ObjectData>();
        backStep = GetComponent<BackStepFunc>();   
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;

        if(Chapter1Manager.Instance.GetStartPuzzle()) return;

        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();

        backStep.BackStep(other.transform, backStep.stepDistance);
    }
}
