using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2ClearTrigger : MonoBehaviour
{
    [SerializeField] GameObject Chapter3Block;
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

        if (Chapter2Manager.Instance.clear)
        {
            Chapter3Block.SetActive(false);
            // Chapter2Manager.Instance.StartChapterThree();
        }
        else
        {
            backStep.BackStep(other.transform, backStep.stepDistance);
        }

        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();
    }
}
