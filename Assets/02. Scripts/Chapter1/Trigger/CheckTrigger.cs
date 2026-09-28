using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    private BackStepFunc backStep;
    private ObjectData triggerObjectData;

    private void Awake()
    {
        triggerObjectData = GetComponent<ObjectData>();
        backStep = GetComponent<BackStepFunc>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag("Player")) return;

        int time = Chapter1Manager.Instance.GetHomeTime();
        bool pass = false;

        switch (time)
        {
            case 2:
                if (Chapter1Manager.Instance.GetCheckCondition(time)) pass = true;
                break;
            case 3:
                if (Chapter1Manager.Instance.GetCheckCondition(time)) pass = true;
                break;
            case 4:
                if (Chapter1Manager.Instance.GetCheckCondition(time)) pass = true;
                break;
            case 5:
                if (Chapter1Manager.Instance.GetCheckCondition(time)) pass = true;
                break;
            case 6:
                if (Chapter1Manager.Instance.GetCheckCondition(time)) pass = true;
                break;
            default:
                pass = true;
                break;
        }

        if (pass) return;

        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();

        backStep.BackStep(other.transform, backStep.stepDistance);
    }


}
