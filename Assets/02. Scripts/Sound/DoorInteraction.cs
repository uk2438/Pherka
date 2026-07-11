using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : InteractiableObject

{
    private Collider2D doorCollider;

    private Animator doorAnim;

    protected override void Awake()
    {
        base.Awake(); //부모클래스 awake 호출
        doorCollider = GetComponent<Collider2D>();
        doorAnim = GetComponent<Animator>();
        
    }

    public override void Activate() {
        doorAnim?.SetTrigger("Open");
        PlayactivateSound();
    
        GameManager.Instance.gameData.isDoorOpen = true;
        if (doorCollider != null) doorCollider.enabled = false;

    }

    public override void Deactivate() {

        doorAnim?.SetTrigger("Close");
        PlaydeactivateSound();

        GameManager.Instance.gameData.isDoorOpen = false;
        if (doorCollider != null) doorCollider.enabled = true;
        
    }


}
