using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    private Character[] characters;

    public static CharacterManager characterManager;

    [Header("Character Swap Scene")]
    private int indexSwap1 = -1, indexSwap2 = -1;
    [SerializeField] private GameObject characterSwapGo;
    [SerializeField] private GameObject buttonSwap;
    [SerializeField] private GameObject[] slots;

    private void Awake()
    {
        CharacterManager[] objs = FindObjectsOfType<CharacterManager>();

        if (objs.Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

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

    #region CharacterSwapScene

    public void OpenCharacterSwapScene()
    {
        characterSwapGo.SetActive(true);

        slots[0].SetActive(characters[0] == null ? false : true);
        slots[1].SetActive(characters[1] == null ? false : true);
        slots[2].SetActive(characters[2] == null ? false : true);
    }

    public void CloseCharacterSwapScene()
    {
        characterSwapGo.SetActive(false);
    }

    public void ChoseCharacterSwap(int index)
    {
        if (indexSwap1 == index)
        {
            indexSwap1 = -1;
            slots[index].GetComponent<Image>().color = new Vector4(1f, 0f, 0f, 0f);
            buttonSwap.gameObject.SetActive(false);

            if (indexSwap2 != -1)
            {
                indexSwap1 = indexSwap2;
                indexSwap2 = -1;
            }

            return;
        }
        else if (indexSwap1 == -1)
        {
            indexSwap1 = index;

            slots[index].GetComponent<Image>().color = new Vector4(1f, 0f, 0f, 1f);

            if (indexSwap2 == -1)
                buttonSwap.gameObject.SetActive(false);

            return;
        }

        if (indexSwap2 == index)
        {
            indexSwap2 = -1;
            slots[index].GetComponent<Image>().color = new Vector4(1f, 0f, 0f, 0f);
            buttonSwap.gameObject.SetActive(false);
            return;
        }

        if (indexSwap2 != -1)
            slots[indexSwap2].GetComponent<Image>().color = new Vector4(1f, 0f, 0f, 0f);

        slots[index].GetComponent<Image>().color = new Vector4(1f, 0f, 0f, 1f);
        buttonSwap.gameObject.SetActive(true);

        indexSwap2 = index;
    }

    public void SwapCharacter()
    {
        Character charRef = characters[indexSwap1];
        characters[indexSwap1] = characters[indexSwap2];
        characters[indexSwap2] = charRef;

        slots[indexSwap1].GetComponent<Image>().color = new Vector4(1f, 0f, 0f, 0f);
        slots[indexSwap2].GetComponent<Image>().color = new Vector4(1f, 0f, 0f, 0f);

        indexSwap1 = -1;
        indexSwap2 = -1;
    }

    #endregion
}
