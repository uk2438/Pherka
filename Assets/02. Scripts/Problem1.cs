using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Problem1 : Singleton<Problem1>
{
    public GameObject npc;
    public void Correct()
    {
        bool isAnswer = Switch.Instance.isActive;
        if(isAnswer)
        {
            ObjectData obj = npc.GetComponent<ObjectData>();

            obj.id = 1001;
        }
    }
}
