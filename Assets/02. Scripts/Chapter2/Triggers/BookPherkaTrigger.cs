using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPherkaTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Pherka;
    private int count = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Carried")) {

            if(other.name != "Book") {
                GameManager.Instance.StartMonologue(40002);
                return;
            }

            count++;
            if(count == 2)
            {
                Chapter2Manager.Instance.ChangeAllChildrenTag(Chapter2Manager.Instance.books, "Structure");
                if(!Chapter2Manager.Instance.GetFirstCompleted())
                    GameManager.Instance.StartMonologue(40000);
                else GameManager.Instance.StartMonologue(40001);
                Pherka.GetComponent<ObjectData>().SetDialogueCondition(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Carried"))
        {
            if(other.name != "Book") return;
            count--;
        }
    }
}
