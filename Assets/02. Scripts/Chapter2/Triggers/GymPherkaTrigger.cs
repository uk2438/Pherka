using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymPherkaTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Pherka;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Carried")) return;

        if(other.name != "Sweatsuit")
        {
            GameManager.Instance.StartMonologue(40002);
            return;
        }

        other.tag = "Structure";
        GameManager.Instance.StartMonologue(40003);
        Pherka.GetComponent<ObjectData>().SetDialogueCondition(true);
        Chapter2Manager.Instance.SetClear(1);
        if(!Chapter2Manager.Instance.GetFirstCompleted())
            Chapter2Manager.Instance.SetFirstCompleted(true);
    }
}
