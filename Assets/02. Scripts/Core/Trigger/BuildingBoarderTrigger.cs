using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBoarderTrigger : MonoBehaviour
{
    private ObjectData objectData;
    private BackStepFunc backStep;
    private void Awake() {
        objectData = GetComponent<ObjectData>();
        backStep = GetComponent<BackStepFunc>();
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;

        GameManager.Instance.gameData.triggerObjectData = objectData;
        GameManager.Instance.TriggerAction();

        backStep.BackStep(other.transform, backStep.stepDistance);
    }
}
