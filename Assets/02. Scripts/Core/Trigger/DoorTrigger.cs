using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorInteraction doorInteraction;
        void OnTriggerExit2D(Collider2D other)
    {
        if(doorInteraction == null) return;
        if (GameManager.Instance.gameData.isDoorOpen && doorInteraction != null)
        {
            doorInteraction.Deactivate();
            doorInteraction = null;
        }
    }
}
