using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstWorkTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Carried"))
        {
            Transform parent = transform.parent;
            ObjectData data = parent.GetComponent<ObjectData>();
            data.SetDialogueCondition(true);
        }
    }
}
