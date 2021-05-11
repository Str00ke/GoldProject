using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private Character char1;
    private Character char2;
    private Character char3;

    public static CharacterManager characterManager;

    private void Awake()
    {
        characterManager = this;
    }

    public Character AskForCharacter(int character)
    {
        if (character == 1)
            return char1;
        else if (character == 2)
            return char2;
        if (character == 3)
            return char3;

        return null;
    }

    public void SetCharacter(int index, Character character)
    {
        if (index == 1)
            char1 = character;
        else if (index == 2)
            char2 = character;
        if (index == 3)
            char3 = character;
    }

    public void SummonCharacter()
    {
        GameObject nGameObject = new GameObject("Character");
        nGameObject.transform.parent = gameObject.transform;
        Character nChar = nGameObject.AddComponent<Character>();

        if (char1 == null)
            char1 = nChar;
        else if (char2 == null)
            char2 = nChar;
        else if (char3 == null)
            char3 = nChar;
        else
            Destroy(nGameObject);
    }
}
