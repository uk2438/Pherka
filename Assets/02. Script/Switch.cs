using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Singleton<Switch>
{
    public bool isActive = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Carried"))
        {
            isActive = true;
            Problem1.Instance.Correct();
            Debug.Log("true");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Carried"))
        {
            isActive = false;
            Debug.Log("false");
        }
    }
}
