using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    public int index;
    public string input;

    [ContextMenu("DEBUG")]
    public void DEBUG_Func()
    {
        CharacterManager _CharacterManager = CharacterManager.characterManager;
        CharacterData getChar = _CharacterManager.GetCharacter(index);
        getChar.ChangeSkill(index, input);
    }
}
