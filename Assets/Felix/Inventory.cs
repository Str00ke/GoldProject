using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private enum EState
    {
        Inventory,
        InventoryItemPartSelection
    }

    public RectTransform mainCanvas;

    [Header("Item Inventory")]
    private int nbLines = 0;
    private List<GameObject> itemList = new List<GameObject>();
    private EState state;
    private GameObject itemPanelTarget;
    private Character characterItemChoosed;

    private int[,] indexItemTypes = new int[4,5];
    [SerializeField] private Image[] itemPartButton = new Image[4];
    [SerializeField] private Image[] itemRarityButton = new Image[5];

    public static Inventory inventory;

    [SerializeField] private GameObject inventoryGo;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private RectTransform rectTransformInventoryContent;
    [SerializeField] private GameObject panelItem;

    [Header("Item Choosing")]
    [SerializeField] private GameObject playerItemSelectionGo;
    [SerializeField] private GameObject itemSelection;
    [SerializeField] private GameObject[] slots = new GameObject[3];

    [Header("Item Stats")]
    [SerializeField] private GameObject itemStatsGo;

    private void Awake()
    {
        Inventory[] inv = FindObjectsOfType<Inventory>();

        if (inv.Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        inventory = this;
    }

    private void Start()
    {
        playerItemSelectionGo.SetActive(false);
        itemStatsGo.SetActive(false);
        panelItem.SetActive(false);
        inventoryGo.SetActive(false);
    }

    #region ItemInventoryPart

    public void OpenInventory()
    {
        state = EState.Inventory;

        CharacterManager.characterManager.CloseTeamScene();
        inventoryGo.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryGo.SetActive(false);
        CharacterManager.characterManager.OpenTeamScene();

        ResetButtonSelection();
    }

    public void AddItem(NItem.ItemScriptableObject item)
    {
        if (itemList.Count % 5 == 0)
        {
            nbLines++;
            ScaleContent();
        }

        GameObject nItem = Instantiate(itemPrefab, rectTransformInventoryContent.transform);
        ItemInInventory iii = nItem.AddComponent<ItemInInventory>();
        iii.item = item;

        nItem.GetComponent<Image>().sprite = item.itemUiSprite;
        
        Button buttonNItem = nItem.GetComponent<Button>();

        itemList.Insert(indexItemTypes[(int)item.itemPartType, (int)item.itemRarity], nItem);
        nItem.transform.SetSiblingIndex(indexItemTypes[(int)item.itemPartType, (int)item.itemRarity]);

        for (int i = 0; i < 4; i++)
        {
            if (i < (int)item.itemPartType)
                continue;

            for (int x = 0; x < 5; x++)
            {
                if (i == (int)item.itemPartType && x < (int)item.itemRarity)
                    continue;

                indexItemTypes[i, x]++;
            }
        }

        buttonNItem.onClick.AddListener(() => ShowItemPanel(nItem));

        bool isActivePart = itemPartButton[(int)item.itemPartType].color.b == 0f;
        bool isActiveRarity = itemRarityButton[(int)item.itemRarity].color.b == 0f;

        nItem.SetActive(isActivePart ? isActiveRarity ? true : false : false);
    }

    public void DeleteItem(GameObject item)
    {
        CloseItemPanel();

        NItem.ItemScriptableObject itemScriptable = item.GetComponent<ItemInInventory>().item;

        for (int i = 0; i < 4; i++)
        {
            if (i < (int)itemScriptable.itemPartType)
                continue;

            for (int x = 0; x < 5; x++)
            {
                if (i == (int)itemScriptable.itemPartType && x < (int)itemScriptable.itemRarity)
                    continue;

                indexItemTypes[i, x]--;
            }
        }

        if (itemList.Count % 5 == 0)
        {
            nbLines--;
            ScaleContent();
        }

        itemList.Remove(item);
        Destroy(item);
    }

    private void ScaleContent()
    {
        Vector2 newSize;
        newSize.x = rectTransformInventoryContent.sizeDelta.x;
        newSize.y = nbLines * 150f + (nbLines + 1) * 50f;
        rectTransformInventoryContent.sizeDelta = newSize;
    }

    private void ShowItemPanel(GameObject item)
    {
        panelItem.SetActive(true);

        itemPanelTarget = item;

        panelItem.transform.GetChild(2).gameObject.SetActive(state == EState.Inventory);

        ItemInInventory iii = item.GetComponent<ItemInInventory>();

        // Move panel
        int index = 0;
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].activeSelf)
                index++;

            if (itemList[i] == item)
                break;
        }

        float direction = index % 5 == 0 ? -1f : 1f;
        
        panelItem.transform.position = item.transform.position + new Vector3(direction * (200f * mainCanvas.localScale.x), 0f, 0f);

        for (int i = 0; i < 3; i++)
            panelItem.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();

        if (state == EState.Inventory)
            panelItem.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ToggleItemChoosingScreen(item));
        else
            panelItem.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => SetChoosedItem(characterItemChoosed, iii.item, item));

        panelItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ToggleItemStatsScreen(iii));
        panelItem.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => DeleteItem(item));
    }

    public void RefreshPositionItemPanel()
    {
        if (!panelItem.activeSelf)
            return;

        panelItem.transform.position = itemPanelTarget.transform.position + new Vector3(150f, 0f, 0f);
    }

    public void CloseItemPanel()
    {
        panelItem.SetActive(false);

        itemPanelTarget = null;
    }

    public void ButtonSelectionRarity(int rarity)
    {
        bool isActive = itemRarityButton[rarity].color.b == 0f;
        itemRarityButton[rarity].color = isActive ? new Vector4(1f, 1f, 1f, 1f) : new Vector4(1f, 0f, 0f, 1f);

        for (int i = 0; i < 4; i++)
        {
            int y = indexItemTypes[i==0 ? 0 : rarity==0 ? i-1 : i, rarity==0 ? i==0 ? 0 : 4 : rarity-1];

            if (y == indexItemTypes[i, rarity])
            {
                continue;
            }

            for (; y < indexItemTypes[i, rarity]; y++)
            {
                bool isActivePart = itemPartButton[(int)itemList[y].GetComponent<ItemInInventory>().item.itemPartType].color.b == 0f;

                itemList[y].SetActive(isActivePart ? !isActive ? true : false : false);
            }
        }
    }

    public void ButtonSelectionItemPart(int itemPart)
    {
        bool isActive = itemPartButton[itemPart].color.b == 0f;
        itemPartButton[itemPart].color = isActive ? new Vector4(1f, 1f, 1f, 1f) : new Vector4(1f, 0f, 0f, 1f);

        int y = indexItemTypes[itemPart == 0 ? 0 : itemPart - 1, itemPart == 0 ? 0 : 4];

        for (; y < indexItemTypes[itemPart, 4]; y++)
        {
            bool isActiveRarity = itemRarityButton[(int)itemList[y].GetComponent<ItemInInventory>().item.itemRarity].color.b == 0f;

            itemList[y].SetActive(isActiveRarity ? !isActive ? true : false : false);
        }
    }

    public void SelectionOneItemPart(int itemPart)
    {
        if (characterItemChoosed == null)
            return;

        for (int i = 0; i < itemRarityButton.Length; i++)
        {
            itemRarityButton[i].color = new Vector4(1f, 1f, 1f, 1f);
            itemRarityButton[i].gameObject.SetActive(false);

            ButtonSelectionRarity(i);
        }

        for (int i = 0; i < itemPartButton.Length; i++)
        {
            itemPartButton[i].color = new Vector4(1f, 0f, 0f, 1f);
            itemPartButton[i].gameObject.SetActive(false);

            ButtonSelectionItemPart(i);
        }

        ButtonSelectionItemPart(itemPart);

        state = EState.InventoryItemPartSelection;
    }

    public void SelectionCharacterOneItemPart(int slotIndex)
    {
        characterItemChoosed = CharacterManager.characterManager.AskForCharacter(slotIndex);
    }

    private void ResetButtonSelection()
    {
        for (int i = 0; i < itemRarityButton.Length; i++)
        {
            itemRarityButton[i].color = new Vector4(1f, 1f, 1f, 1f);
            itemRarityButton[i].gameObject.SetActive(true);

            ButtonSelectionRarity(i);
        }

        for (int i = 0; i < itemPartButton.Length; i++)
        {
            itemPartButton[i].color = new Vector4(1f, 1f, 1f, 1f);
            itemPartButton[i].gameObject.SetActive(true);

            ButtonSelectionItemPart(i);
        }
    }

    #endregion

    #region ItemChoosingPart

    private void ToggleItemChoosingScreen(GameObject itemGo)
    {
        panelItem.SetActive(false);

        ItemInInventory item = itemGo.GetComponent<ItemInInventory>();

        playerItemSelectionGo.SetActive(!playerItemSelectionGo.activeSelf);

        /* Set item to set */
        itemSelection.GetComponent<Image>().sprite = item.item.itemUiSprite;
        Button btnItemSelected = itemSelection.GetComponent<Button>();
        btnItemSelected.onClick.RemoveAllListeners();
        btnItemSelected.onClick.AddListener(() => ToggleItemStatsScreen(item));

        /* Get current characters */
        Character[] characters = new Character[3];
        for (int i = 0; i < 3; i++)
        {
            characters[i] = CharacterManager.characterManager.AskForCharacter(i);
        }

        /* Enable/Disable character slots */
        slots[0].SetActive(characters[0] == null ? false : true);
        slots[1].SetActive(characters[1] == null ? false : true);
        slots[2].SetActive(characters[2] == null ? false : true);

        
        for (int i = 0; i < 3; i++)
        {
            if (!slots[i].activeSelf)
                continue;

            /* Set characters sprites */
            GameObject spritesGo = slots[i].transform.GetChild(3).gameObject;
            Character character = CharacterManager.characterManager.AskForCharacter(i);
            Image img = spritesGo.transform.GetChild(2).GetComponent<Image>();
            if (character.charHead != null)
            {
                img.sprite = character.charHead;
                img.color = new Vector4(1f, 1f, 1f, 1f);
            }
            else
            {
                img.color = new Vector4(1f, 1f, 1f, 0f);
            }

            img = spritesGo.transform.GetChild(0).GetComponent<Image>();
            if (character.itemSprites[0] != null)
            {
                img.sprite = character.itemSprites[0];
                img.color = new Vector4(1f, 1f, 1f, 1f);
            }
            else
            {
                img.color = new Vector4(1f, 1f, 1f, 0f);
            }

            img = spritesGo.transform.GetChild(3).GetComponent<Image>();
            if (character.itemSprites[1] != null)
            {
                img.sprite = character.itemSprites[1];
                img.color = new Vector4(1f, 1f, 1f, 1f);
            }
            else
            {
                img.color = new Vector4(1f, 1f, 1f, 0f);
            }

            img = spritesGo.transform.GetChild(1).GetComponent<Image>();
            if (character.itemSprites[2] != null)
            {
                img.sprite = character.itemSprites[2];
                img.color = new Vector4(1f, 1f, 1f, 1f);
            }
            else
            {
                img.color = new Vector4(1f, 1f, 1f, 0f);
            }


            /* Set characters stats */
            GameObject stats = slots[i].transform.GetChild(1).gameObject;
            stats.transform.GetChild(0).GetComponent<Text>().text = "Health: " + characters[i].health;
            stats.transform.GetChild(1).GetComponent<Text>().text = "Armor: " + characters[i].armor;
            stats.transform.GetChild(2).GetComponent<Text>().text = "Attack: " + characters[i].attack;
            stats.transform.GetChild(3).GetComponent<Text>().text = "Dodge: " + characters[i].dodge;


            /* Set characters items */
            GameObject items = slots[i].transform.GetChild(0).gameObject;
            items.transform.GetChild(0).GetComponent<Image>().sprite = characters[i].GetItem(NItem.EPartType.Head) != null ? characters[i].GetItem(NItem.EPartType.Head).itemUiSprite : null;
            if (characters[i].GetItem(NItem.EPartType.Head) != null)
            {
                NItem.ItemScriptableObject head = characters[i].GetItem(NItem.EPartType.Head);
                items.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                items.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ToggleItemStatsScreen(head));
            }

            items.transform.GetChild(1).GetComponent<Image>().sprite = characters[i].GetItem(NItem.EPartType.Body) != null ? characters[i].GetItem(NItem.EPartType.Body).itemUiSprite : null;
            if (characters[i].GetItem(NItem.EPartType.Body) != null)
            {
                NItem.ItemScriptableObject body = characters[i].GetItem(NItem.EPartType.Body);
                items.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                items.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ToggleItemStatsScreen(body));
            }

            items.transform.GetChild(2).GetComponent<Image>().sprite = characters[i].GetItem(NItem.EPartType.Weapon) != null ? characters[i].GetItem(NItem.EPartType.Weapon).itemUiSprite : null;
            if (characters[i].GetItem(NItem.EPartType.Weapon) != null)
            {
                NItem.ItemScriptableObject weapon = characters[i].GetItem(NItem.EPartType.Weapon);
                items.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                items.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ToggleItemStatsScreen(weapon));
            }

            items.transform.GetChild(3).GetComponent<Image>().sprite = characters[i].GetItem(NItem.EPartType.Gem) != null ? characters[i].GetItem(NItem.EPartType.Gem).itemUiSprite : null;
            if (characters[i].GetItem(NItem.EPartType.Gem) != null)
            {
                NItem.ItemScriptableObject gem = characters[i].GetItem(NItem.EPartType.Gem);
                items.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                items.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => ToggleItemStatsScreen(gem));
            }


            /* Set the Set buttons */
            Character slotCharacter = characters[i];

            Button setButton = slots[i].transform.GetChild(2).GetComponent<Button>();

            setButton.onClick.RemoveAllListeners();
            setButton.onClick.AddListener(() => SetChoosedItem(slotCharacter, item.item, itemGo));
        }
    }

    public void CloseItemChoosingScreen()
    {
        playerItemSelectionGo.SetActive(!playerItemSelectionGo.activeSelf);
    }

    public void SwapCharacterSLotStats(int slot)
    {
        Transform items = slots[slot].transform.GetChild(0);

        if (items.gameObject.activeSelf)
        {
            items.gameObject.SetActive(false);
            slots[slot].transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            items.gameObject.SetActive(true);
            slots[slot].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void SetChoosedItem(Character character, NItem.ItemScriptableObject item, GameObject itemGo)
    {
        NItem.ItemScriptableObject lastItem = null;

        switch (item.itemPartType.ToString())
        {
            case "Head":
                lastItem = character.GetItem(NItem.EPartType.Head);
                character.AddItem(item, NItem.EPartType.Head);
                break;
            
            case "Body":
                lastItem = character.GetItem(NItem.EPartType.Body);
                character.AddItem(item, NItem.EPartType.Body);
                break;

            case "Weapon":
                lastItem = character.GetItem(NItem.EPartType.Weapon);
                character.AddItem(item, NItem.EPartType.Weapon);
                break;

            case "Gem":
                lastItem = character.GetItem(NItem.EPartType.Gem);
                character.AddItem(item, NItem.EPartType.Gem);
                break;

        default:
                break;
        }

        if (lastItem != null)
            AddItem(lastItem);

        // character refresh stats
        DeleteItem(itemGo);

        if (state == EState.Inventory)
            CloseItemChoosingScreen();
        else
            CloseInventory();
    }

    #endregion

    #region ItemStats

    public void ToggleItemStatsScreen(ItemInInventory item)
    {
        itemStatsGo.SetActive(true);
        panelItem.SetActive(false);

        itemStatsGo.transform.GetChild(1).GetComponent<Image>().sprite = item.item.itemUiSprite;

        GameObject panel = itemStatsGo.transform.GetChild(0).GetChild(1).gameObject;

        itemStatsGo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.item.itemName;
        panel.transform.GetChild(0).GetComponent<Text>().text = "Health: " + item.item.health;
        panel.transform.GetChild(0).gameObject.SetActive(item.item.health == 0 ? false : true);

        panel.transform.GetChild(1).GetComponent<Text>().text = "Armor: " + item.item.armor;
        panel.transform.GetChild(1).gameObject.SetActive(item.item.armor == 0 ? false : true);

        panel.transform.GetChild(2).GetComponent<Text>().text = "Attack: " + item.item.attack;
        panel.transform.GetChild(2).gameObject.SetActive(item.item.attack == 0 ? false : true);

        panel.transform.GetChild(3).GetComponent<Text>().text = "Dodge: " + item.item.dodge;
        panel.transform.GetChild(3).gameObject.SetActive(item.item.dodge == 0 ? false : true);

        panel.transform.GetChild(4).GetComponent<Text>().text = "Crit%: " + item.item.criticalChance;
        panel.transform.GetChild(4).gameObject.SetActive(item.item.criticalChance == 0 ? false : true);

        panel.transform.GetChild(5).GetComponent<Text>().text = "CritDmg: " + item.item.crititalDamage;
        panel.transform.GetChild(5).gameObject.SetActive(item.item.crititalDamage == 0 ? false : true);

        panel.transform.GetChild(6).GetComponent<Text>().text = "Element: " + item.item.itemType.ToString();
        panel.transform.GetChild(6).gameObject.SetActive(item.item.itemType == NItem.EItemType.None ? false : true);
    }

    public void ToggleItemStatsScreen(NItem.ItemScriptableObject item)
    {
        itemStatsGo.SetActive(true);

        itemStatsGo.transform.GetChild(1).GetComponent<Image>().sprite = item.itemUiSprite;

        GameObject panel = itemStatsGo.transform.GetChild(0).GetChild(1).gameObject;

        itemStatsGo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.itemName;
        panel.transform.GetChild(0).GetComponent<Text>().text = "Health: " + item.health;
        panel.transform.GetChild(0).gameObject.SetActive(item.health == 0 ? false : true);

        panel.transform.GetChild(1).GetComponent<Text>().text = "Armor: " + item.armor;
        panel.transform.GetChild(1).gameObject.SetActive(item.armor == 0 ? false : true);

        panel.transform.GetChild(2).GetComponent<Text>().text = "Attack: " + item.attack;
        panel.transform.GetChild(2).gameObject.SetActive(item.attack == 0 ? false : true);

        panel.transform.GetChild(3).GetComponent<Text>().text = "Dodge: " + item.dodge;
        panel.transform.GetChild(3).gameObject.SetActive(item.dodge == 0 ? false : true);

        panel.transform.GetChild(4).GetComponent<Text>().text = "Crit%: " + item.criticalChance;
        panel.transform.GetChild(4).gameObject.SetActive(item.criticalChance == 0 ? false : true);

        panel.transform.GetChild(5).GetComponent<Text>().text = "CritDmg: " + item.crititalDamage;
        panel.transform.GetChild(5).gameObject.SetActive(item.crititalDamage == 0 ? false : true);

        panel.transform.GetChild(6).GetComponent<Text>().text = "Element: " + item.itemType.ToString();
        panel.transform.GetChild(6).gameObject.SetActive(item.itemType == NItem.EItemType.None ? false : true);
    }

    public void CloseItemStatsScreen()
    {
        itemStatsGo.SetActive(false);
    }


    #endregion
}

public class ItemInInventory : MonoBehaviour
{
    public NItem.ItemScriptableObject item;
}