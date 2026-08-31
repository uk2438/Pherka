using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlateTrigger : MonoBehaviour
{
    [SerializeField] private Sprite dogFoodOnPlate;
    [SerializeField] private GameObject bedTrigger;
    [SerializeField] private GameObject door;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "DogFood")
        {
            ChangePlate(other);

            GameManager.Instance.StartMonologue(20001);
            bedTrigger.SetActive(true);
            door.GetComponent<ObjectData>().SetDialogueCondition(true);
        }
    }

    private void ChangePlate(Collider2D other)
    {
        other.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = dogFoodOnPlate;
    }
}
