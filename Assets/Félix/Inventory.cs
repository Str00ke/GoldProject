using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public enum EInventoryState
    {
        None,
        Item_Inventory,
        Item_Choosing
    }

    private EInventoryState inventoryState = EInventoryState.None; 

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

    private void Start()
    {
        playerItemSelectionGo.SetActive(false);
        //inventoryGo.SetActive(false);
    }

    #region ItemInventoryPart

    public void AddItem()
    {
        if (itemList.Count % 5 == 0)
        {
            nbLines++;
            ScaleContent();
        }

        GameObject nItem = Instantiate(itemPrefab, rectTransformInventoryContent.transform);
        Vector4 nColorItem = new Vector4();
        for (int i = 0; i < 3; i++)
        {
            nColorItem[i] = Random.Range(0f, 1f);
        }

        nColorItem.w = 1f;
        Color col = nColorItem;

        nItem.GetComponent<Image>().color = col;
        
        Button buttonNItem = nItem.GetComponent<Button>();
        buttonNItem.onClick.AddListener(() => ToggleItemChoosingScreen(col));
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

    private void ToggleItemChoosingScreen(/* item */ Color itemColor)
    {
        playerItemSelectionGo.SetActive(!playerItemSelectionGo.activeSelf);

        inventoryState = EInventoryState.Item_Choosing;

        /* Set item to set */
        itemSelection.GetComponent<Image>().color = itemColor;

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

        /* */

    }

    public void CloseItemChoosingScreen()
    {
        playerItemSelectionGo.SetActive(!playerItemSelectionGo.activeSelf);

        inventoryState = EInventoryState.Item_Inventory;
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

    #endregion
}
