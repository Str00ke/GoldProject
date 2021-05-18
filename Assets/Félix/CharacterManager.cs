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
    public GameObject characterSwapGo;
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

        for (int i = 0; i < characters.Length; i++)
        {
            if (!slots[i].activeSelf)
                continue;

            Transform itemSlotTransform = slots[i].transform.GetChild(0).GetChild(0);
            NItem.ItemScriptableObject item = characters[i].GetItem(NItem.EPartType.Head);

            itemSlotTransform.GetChild(0).GetComponent<Image>().sprite = item != null ? item.itemUiSprite : null;
            if (item != null)
            {
                NItem.ItemScriptableObject nItem = characters[i].GetItem(NItem.EPartType.Head);
                itemSlotTransform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                itemSlotTransform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.ToggleItemStatsScreen(nItem));
            }

            item = characters[i].GetItem(NItem.EPartType.Body);
            itemSlotTransform.GetChild(1).GetComponent<Image>().sprite = item != null ? item.itemUiSprite : null;
            if (item != null)
            {
                NItem.ItemScriptableObject nItem = characters[i].GetItem(NItem.EPartType.Body);
                itemSlotTransform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                itemSlotTransform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.ToggleItemStatsScreen(nItem));
            }

            item = characters[i].GetItem(NItem.EPartType.Weapon);
            itemSlotTransform.GetChild(2).GetComponent<Image>().sprite = item != null ? item.itemUiSprite : null;
            if (item != null)
            {
                NItem.ItemScriptableObject nItem = characters[i].GetItem(NItem.EPartType.Weapon);
                itemSlotTransform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                itemSlotTransform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.ToggleItemStatsScreen(nItem));
            }

            item = characters[i].GetItem(NItem.EPartType.Gem);
            itemSlotTransform.GetChild(3).GetComponent<Image>().sprite = item != null ? item.itemUiSprite : null;
            if (item != null)
            {
                NItem.ItemScriptableObject nItem = characters[i].GetItem(NItem.EPartType.Gem);
                itemSlotTransform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                itemSlotTransform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.ToggleItemStatsScreen(nItem));
            }
        }
    }

    public void CloseCharacterSwapScene()
    {
        indexSwap1 = -1;
        indexSwap2 = -1;

        for (int i = 0; i < 3; i++)
            slots[i].GetComponent<Image>().color = new Vector4(1f, 0f, 0f, 0f);

        buttonSwap.gameObject.SetActive(false);

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

        CloseCharacterSwapScene();
    }

    #endregion
}
