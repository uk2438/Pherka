using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Son;
    private int count = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Carried")) {
            count++;
            Debug.Log($"{count}");
            if(count == 4)
            {
                PrologueManager.Instance.ChangeAllChildrenTag(PrologueManager.Instance.homeBoxs, "Structure");
                GameManager.Instance.StartMonologue(20000);
                Son.GetComponent<ObjectData>().SetDialogueCondition(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Carried"))
        {
            count--;
            Debug.Log($"{count}");
        }
    }
}

