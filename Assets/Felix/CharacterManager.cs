using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    private Character[] characters;

    public static CharacterManager characterManager;

    [Header("Team Scene")]
    public GameObject teamSceneGo;
    [SerializeField] private GameObject[] slotsTeam;
    [SerializeField] private GameObject itemPanel;

    [Header("Team Swap Scene")]
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

    private void Start()
    {
        teamSceneGo.SetActive(false);
        characterSwapGo.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !characterSwapGo.activeSelf)
        {
            OpenTeamScene();
        }
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

    #region TeamScene

    public void OpenTeamScene()
    {
        teamSceneGo.SetActive(true);

        slotsTeam[0].SetActive(characters[0] == null ? false : true);
        slotsTeam[1].SetActive(characters[1] == null ? false : true);
        slotsTeam[2].SetActive(characters[2] == null ? false : true);

        /* Set characters items */
        for (int i = 0; i < 3; i++)
        {
            if (!slotsTeam[i].activeSelf)
                continue;

            GameObject items = slotsTeam[i].transform.GetChild(0).gameObject;

            for (int z = 0; z < 4; z++)
            {
                items.transform.GetChild(z).GetComponent<Button>().onClick.RemoveAllListeners();
            }

            
            items.transform.GetChild(0).GetComponent<Image>().sprite = characters[i].GetItem(NItem.EPartType.Head) != null ? characters[i].GetItem(NItem.EPartType.Head).itemUiSprite : null;
            if (characters[i].GetItem(NItem.EPartType.Head) != null)
            {
                int x = i;
                NItem.ItemScriptableObject head = characters[i].GetItem(NItem.EPartType.Head);
                
                items.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowItemPanel(x, 0, head));
            }

            items.transform.GetChild(1).GetComponent<Image>().sprite = characters[i].GetItem(NItem.EPartType.Body) != null ? characters[i].GetItem(NItem.EPartType.Body).itemUiSprite : null;
            if (characters[i].GetItem(NItem.EPartType.Body) != null)
            {
                int x = i;
                NItem.ItemScriptableObject body = characters[i].GetItem(NItem.EPartType.Body);
                items.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ShowItemPanel(x, 1, body));
            }

            items.transform.GetChild(2).GetComponent<Image>().sprite = characters[i].GetItem(NItem.EPartType.Weapon) != null ? characters[i].GetItem(NItem.EPartType.Weapon).itemUiSprite : null;
            if (characters[i].GetItem(NItem.EPartType.Weapon) != null)
            {
                int x = i;
                NItem.ItemScriptableObject weapon = characters[i].GetItem(NItem.EPartType.Weapon);
                items.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ShowItemPanel(x, 2, weapon));
            }

            items.transform.GetChild(3).GetComponent<Image>().sprite = characters[i].GetItem(NItem.EPartType.Gem) != null ? characters[i].GetItem(NItem.EPartType.Gem).itemUiSprite : null;
            if (characters[i].GetItem(NItem.EPartType.Gem) != null)
            {
                int x = i;
                NItem.ItemScriptableObject gem = characters[i].GetItem(NItem.EPartType.Gem);
                items.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => ShowItemPanel(x, 3, gem));
            }
        }

        /* Set characters stats */
        for (int i = 0; i < 3; i++)
        {
            if (!slotsTeam[i].activeSelf)
                continue;

            GameObject stats = slotsTeam[i].transform.GetChild(1).gameObject;
            stats.transform.GetChild(0).GetComponent<Text>().text = "Health: " + characters[i].health;
            stats.transform.GetChild(1).GetComponent<Text>().text = "Armor: " + characters[i].armor;
            stats.transform.GetChild(2).GetComponent<Text>().text = "Attack: " + characters[i].attack;
            stats.transform.GetChild(3).GetComponent<Text>().text = "Dodge: " + characters[i].dodge;
        }
    }

    private void ShowItemPanel(int indexSlot, int indexPartItem, NItem.ItemScriptableObject item)
    {
        itemPanel.SetActive(true);

        Vector3 itemPos = slotsTeam[indexSlot].transform.GetChild(0).GetChild(indexPartItem).position;

        itemPanel.transform.position = itemPos + new Vector3(150f, 0f, 0f);

        for (int i = 0; i < 4; i++)
            itemPanel.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();

        Inventory inv = Inventory.inventory;

        itemPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => inv.OpenInventory());
        itemPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => inv.SelectionCharacterOneItemPart(indexSlot));
        itemPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => inv.SelectionOneItemPart(indexPartItem));

        itemPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => inv.ToggleItemStatsScreen(item));
        itemPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => RemoveCharacterItem(indexSlot, item));

        itemPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => CloseItemPanel());
    }

    private void CloseItemPanel()
    {
        itemPanel.SetActive(false);
    }

    public void CloseTeamScene()
    {
        teamSceneGo.SetActive(false);
        CloseItemPanel();
    }

    private void RemoveCharacterItem(int characterIndex, NItem.ItemScriptableObject item)
    {
        CloseItemPanel();

        Character character = AskForCharacter(characterIndex);

        Inventory.inventory.AddItem(item);

        character.RemoveItem(item.itemPartType);

        OpenTeamScene();
    }

    public void SwapCharacterSlotStats(int slot)
    {
        Transform items = slotsTeam[slot].transform.GetChild(0);

        if (items.gameObject.activeSelf)
        {
            items.gameObject.SetActive(false);
            slotsTeam[slot].transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            items.gameObject.SetActive(true);
            slotsTeam[slot].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    #endregion

    #region CharacterSwapScene

    public void OpenCharacterSwapScene()
    {
        CloseTeamScene();

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
        OpenTeamScene();
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
