using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private Character[] characters;

    public static CharacterManager characterManager;

    private void Awake()
    {
        characterManager = this;
        characters = new Character[3];
    }

    public Character AskForCharacter(int indexChar)
    {
        return characters[indexChar];
    }

    public void SetCharacter(int indexChar, Character character)
    {
        characters[indexChar] = character;
    }

    public void SummonCharacter()
    {
        GameObject nGameObject = new GameObject("Character");
        nGameObject.transform.parent = gameObject.transform;
        Character nChar = nGameObject.AddComponent<Character>();

        bool isSet = false;

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null)
            {
                characters[i] = nChar;
                isSet = true;
                break;
            }
        }

        if (!isSet)
        {
            Destroy(nGameObject);
        }
    }

    public void SwapCharacterIndex(int indexChar1, int indexChar2)
    {
        Character charRef = characters[indexChar1];
        characters[indexChar1] = characters[indexChar2];
        characters[indexChar2] = charRef;
    }
}
