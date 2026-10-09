using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomDoorInteraction : InteractiableObject
{
    private BoxCollider2D doorCollider;
    private Animator doorAnim;
    private Vector2 originalSize, originalOffset;
    protected override void Awake()
    {
        base.Awake(); //부모클래스 awake 호출
        doorCollider = GetComponent<BoxCollider2D>();
        doorAnim = GetComponent<Animator>();

        SaveCollider();
        
    }
    public override void Activate()
    {

        if(GameManager.Instance.gameData.isDoorOpen) return;

        doorAnim?.SetTrigger("Open");
    
        GameManager.Instance.gameData.isDoorOpen = true;

        PlayactivateSound();
        LeftHalfCollider();
    }

    public override void Deactivate()
    {        
        doorAnim?.SetTrigger("Close");
        PlaydeactivateSound();
    
        GameManager.Instance.gameData.isDoorOpen = false;
        BackUpCollider();

        
    }

    private void SaveCollider()
    {
        if(doorCollider == null) return;

        originalSize = doorCollider.size;
        originalOffset = doorCollider.offset;

    }

    private void LeftHalfCollider()
    {
        if(doorCollider == null) return;

        doorCollider.size = new Vector2(originalSize.x * 0.5f, originalSize.y);
        doorCollider.offset = originalOffset + new Vector2(-originalSize.x * 0.25f, 0f);
    }

    private void BackUpCollider()
    {
        doorCollider.size = originalSize;
        doorCollider.offset = originalOffset;
    }
}
