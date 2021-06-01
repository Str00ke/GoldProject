using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public RectTransform mainCanvas;
    public int golds = 0;
    public int souls = 0;

    [Header("Item Inventory")]
    private int nbLines = 0;
    private List<GameObject> itemList = new List<GameObject>();
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
    [SerializeField] private Button[] buttonsItemSet;

    private void Start()
    {
        inventory = this;

        panelItem.SetActive(false);
        inventoryGo.SetActive(false);
    }

    #region ItemInventoryPart

    public void OpenInventory()
    {
        LobbyManager.lobbyManager.lobbyState = ELobbyState.Inventory;

        inventoryGo.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryGo.SetActive(false);

        CloseItemPanel();

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

        // Item panel
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
        ItemInInventory iii = item.GetComponent<ItemInInventory>();

        if (LobbyManager.lobbyManager.lobbyState == ELobbyState.InventoryItemPartSelection)
        {
            SetChoosedItem(characterItemChoosed, iii.item, item);
            CloseInventory();
            return;
        }

        // Show stats
        CharacterManager.characterManager.SelectItemStats(iii.item);

        //panelItem.SetActive(true);

        // Move panel
        //itemPanelTarget = item.gameObject;

        //RefreshPositionItemPanel();

        //panelItem.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();

        if (LobbyManager.lobbyManager.lobbyState == ELobbyState.Inventory)
        {
            //panelItem.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => DeleteItem(item));

            for (int i = 0; i < buttonsItemSet.Length; i++)
            {
                Character character = CharacterManager.characterManager.AskForCharacter(i);

                buttonsItemSet[i].gameObject.SetActive(character != null);
                buttonsItemSet[i].onClick.RemoveAllListeners();
                buttonsItemSet[i].onClick.AddListener(() => SetChoosedItem(character, iii.item, item));
            }
            // Show stats bellow
        }
        /*else
         * // Set item 
            panelItem.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => SetChoosedItem(characterItemChoosed, iii.item, item));
        */
    }

    public void RefreshPositionItemPanel()
    {
        if (!panelItem.activeSelf)
            return;
        
        float direction = (panelItem.GetComponent<RectTransform>().anchoredPosition + itemPanelTarget.GetComponent<RectTransform>().anchoredPosition + new Vector2(150f , 0f)).x >= itemPanelTarget.GetComponent<RectTransform>().rect.width / 2f ? -1f : 1f;

        panelItem.transform.position = itemPanelTarget.transform.position + new Vector3(direction * (150f * mainCanvas.localScale.x), 0f, 0f);
    }

    public void CloseItemPanel()
    {
        panelItem.SetActive(false);

        for (int i = 0; i < buttonsItemSet.Length; i++)
            buttonsItemSet[i].gameObject.SetActive(false);

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

        LobbyManager.lobbyManager.lobbyState = ELobbyState.InventoryItemPartSelection;
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

        DeleteItem(itemGo);

        // Refresh team
        CharacterManager.characterManager.RefreshTeamScene();
        CharacterManager.characterManager.SelectCharacterStats(CharacterManager.characterManager.AskForCharacterIndex(character));

        /*if (state == EState.Inventory)
            CloseItemChoosingScreen();
        else
            CloseInventory();*/
    }

    public void AddGolds(int _golds)
    {
        golds += _golds;

        if (golds < 0)
            golds = 0;

        // refresh golds text
    }

    public void AddSouls(int _souls)
    {
        souls += _souls;

        if (souls < 0)
            souls = 0;

        // refresh souls text
    }
}

public class ItemInInventory : MonoBehaviour
{
    public NItem.ItemScriptableObject item;
}