using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    [Header("Item Inventory")]
    private int nbLines = 0;
    private List<GameObject> itemList = new List<GameObject>();

    public static Inventory inventory;

    [SerializeField] private GameObject inventoryGo;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private RectTransform rectTransformInventoryContent;

    [Header("Item Choosing")]
    [SerializeField] private GameObject playerItemSelectionGo;
    [SerializeField] private GameObject itemSelection;
    [SerializeField] private GameObject[] slots = new GameObject[3];

    [Header("Item Stats")]
    [SerializeField] private GameObject itemStatsGo;

    private void Start()
    {
        playerItemSelectionGo.SetActive(false);
        itemStatsGo.SetActive(false);
        //inventoryGo.SetActive(false);
    }

    #region ItemInventoryPart

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

        Vector4 nColorItem = new Vector4();
        for (int i = 0; i < 3; i++)
        {
            nColorItem[i] = Random.Range(0f, 1f);
        }

        nColorItem.w = 1f;
        Color col = nColorItem;

        nItem.GetComponent<Image>().color = col;
        
        Button buttonNItem = nItem.GetComponent<Button>();
        buttonNItem.onClick.AddListener(() => ToggleItemChoosingScreen(nItem) /*ToggleItemStatsScreen(iii)*/ );
        itemList.Add(nItem);
    }

    public void DeleteItem(GameObject item)
    {
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

    #endregion

    #region ItemChoosingPart

    private void ToggleItemChoosingScreen(GameObject itemGo)
    {
        ItemInInventory item = itemGo.GetComponent<ItemInInventory>();

        playerItemSelectionGo.SetActive(!playerItemSelectionGo.activeSelf);

        /* Set item to set */
        itemSelection.GetComponent<Image>().sprite = item.item.itemUiSprite;
        itemSelection.GetComponent<Button>().onClick.AddListener(() => ToggleItemStatsScreen(item));

        /* Get current characters */
        Character[] characters = new Character[3];
        for (int i = 0; i < 3; i++)
        {
            characters[i] = CharacterManager.characterManager.AskForCharacter(i + 1);
        }

        /* Enable/Disable character slots */
        slots[0].SetActive(characters[0] == null ? false : true);
        slots[1].SetActive(characters[1] == null ? false : true);
        slots[2].SetActive(characters[2] == null ? false : true);

        /* Set characters items */
        for (int i = 0; i < 3; i++)
        {
            if (!slots[i].activeSelf)
                continue;

            GameObject items = slots[i].transform.GetChild(0).gameObject;
            items.transform.GetChild(0).GetComponent<Image>().sprite = characters[i].head != null ? characters[i].head.itemUiSprite : null;
            if (characters[i].head != null)
            {
                NItem.ItemScriptableObject head = characters[i].head;
                items.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ToggleItemStatsScreen(head));
            }

            items.transform.GetChild(1).GetComponent<Image>().sprite = characters[i].body != null ? characters[i].body.itemUiSprite : null;
            if (characters[i].body != null)
            {
                NItem.ItemScriptableObject body = characters[i].body;
                items.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ToggleItemStatsScreen(body));
            }

            items.transform.GetChild(2).GetComponent<Image>().sprite = characters[i].weapon != null ? characters[i].weapon.itemUiSprite : null;
            if (characters[i].weapon != null)
            {
                NItem.ItemScriptableObject weapon = characters[i].weapon;
                items.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ToggleItemStatsScreen(weapon));
            }

            items.transform.GetChild(3).GetComponent<Image>().sprite = characters[i].gem != null ? characters[i].gem.itemUiSprite : null;
            if (characters[i].gem != null)
            {
                NItem.ItemScriptableObject gem = characters[i].gem;
                items.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => ToggleItemStatsScreen(gem));
            }
        }

        /* Set characters stats */
        for (int i = 0; i < 3; i++)
        {
            if (!slots[i].activeSelf)
                continue;

            GameObject stats = slots[i].transform.GetChild(1).gameObject;
            stats.transform.GetChild(0).GetComponent<Text>().text = "Health: " + characters[i].health;
            stats.transform.GetChild(1).GetComponent<Text>().text = "Armor: " + characters[i].armor;
            stats.transform.GetChild(2).GetComponent<Text>().text = "Attack: " + characters[i].attack;
            stats.transform.GetChild(3).GetComponent<Text>().text = "Dodge: " + characters[i].dodge;
        }

        /* Set the Set buttons */
        for (int i = 0; i < 3; i++)
        {
            if (!slots[i].activeSelf)
                continue;

            Character slotCharacter = characters[i];
            slots[i].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => SetChoosedItem(slotCharacter, item.item, itemGo));
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
                lastItem = character.head;
                character.head = item;
                break;
            
            case "Body":
                lastItem = character.body;
                character.body = item;
                break;

            case "Weapon":
                lastItem = character.weapon;
                character.weapon = item;
                break;

            case "Gem":
                lastItem = character.gem;
                character.gem = item;
                break;

        default:
                break;
        }

        // character refresh stats
        DeleteItem(itemGo);

        if (lastItem != null)
            AddItem(lastItem);

        CloseItemChoosingScreen();
    }

    #endregion

    #region ItemStats

    public void ToggleItemStatsScreen(ItemInInventory item)
    {
        itemStatsGo.SetActive(true);

        itemStatsGo.transform.GetChild(1).GetComponent<Image>().sprite = item.item.itemUiSprite;

        GameObject panel = itemStatsGo.transform.GetChild(0).GetChild(1).gameObject;

        itemStatsGo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.item.itemName;
        panel.transform.GetChild(0).GetComponent<Text>().text = "Health: " + item.item.health;
        panel.transform.GetChild(1).GetComponent<Text>().text = "Armor: " + item.item.armor;
        panel.transform.GetChild(2).GetComponent<Text>().text = "Attack: " + item.item.attack;
        panel.transform.GetChild(3).GetComponent<Text>().text = "Dodge: " + item.item.dodge;
        panel.transform.GetChild(4).GetComponent<Text>().text = "Crit%: " + item.item.criticalChance;
        panel.transform.GetChild(5).GetComponent<Text>().text = "CritDmg: " + item.item.crititalDamage;
        panel.transform.GetChild(6).GetComponent<Text>().text = "Element: " + item.item.itemType.ToString();
    }

    public void ToggleItemStatsScreen(NItem.ItemScriptableObject item)
    {
        Debug.Log("Pop");
        itemStatsGo.SetActive(true);

        itemStatsGo.transform.GetChild(1).GetComponent<Image>().sprite = item.itemUiSprite;

        GameObject panel = itemStatsGo.transform.GetChild(0).GetChild(1).gameObject;

        itemStatsGo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.itemName;
        panel.transform.GetChild(0).GetComponent<Text>().text = "Health: " + item.health;
        panel.transform.GetChild(1).GetComponent<Text>().text = "Armor: " + item.armor;
        panel.transform.GetChild(2).GetComponent<Text>().text = "Attack: " + item.attack;
        panel.transform.GetChild(3).GetComponent<Text>().text = "Dodge: " + item.dodge;
        panel.transform.GetChild(4).GetComponent<Text>().text = "Crit%: " + item.criticalChance;
        panel.transform.GetChild(5).GetComponent<Text>().text = "CritDmg: " + item.crititalDamage;
        panel.transform.GetChild(6).GetComponent<Text>().text = "Element: " + item.itemType.ToString();
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