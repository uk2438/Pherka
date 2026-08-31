using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewCharacterData",
    menuName = "Dialogue/Character Data"
)]
public class CharacterData : ScriptableObject
{
    public string charactorName;
    public Sprite[] potraits;

}
