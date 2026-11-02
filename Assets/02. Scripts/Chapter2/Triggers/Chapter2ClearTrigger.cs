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


        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();

        if (Chapter2Manager.Instance.clear)
        {
            GameManager.Instance.SetDialogueFinishedCallback(() =>
            {
            Chapter3Block.SetActive(false);
            DialogueManager.Instance.SetPresentChapter(3);                
            });
        }
        else
        {
            backStep.BackStep(other.transform, backStep.stepDistance);
        }
    }
}
