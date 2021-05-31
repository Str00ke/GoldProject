using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterManager : MonoBehaviour
{
    private Character[] characters;

    public static CharacterManager characterManager;

    [Header("Summon")]
    public CharacterScriptableObject[] charactersScriptable;
    public NItem.ItemScriptableObject[] gemsScriptable;

    [Header("Team Scene")]
    [SerializeField] private GameObject[] slotsTeam;
    [SerializeField] private GameObject itemsUIPanel;
    [SerializeField] private GameObject informationsUIPanel;
    [SerializeField] private GameObject statsUIPanel;
    [HideInInspector] public int selectedChar = 0;
    [HideInInspector] public NItem.ItemScriptableObject selectedItem;


    private void Start()
    {
        characterManager = this;
        characters = new Character[3];

        RefreshTeamScene();
    }

    public Character AskForCharacter(int indexChar)
    {
        return characters[indexChar];
    }

    public int AskForCharacterIndex(Character character)
    {
        return Array.IndexOf(characters, character);
    }

    public void SetCharacter(int indexChar, Character character)
    {
        characters[indexChar] = character;
    }

    public void SummonCharacter(int indexChar)
    {
        if (AskForCharacter(indexChar) != null)
            return;

        GameObject nGameObject = new GameObject("Character");
        nGameObject.transform.parent = gameObject.transform;
        Character nChar = nGameObject.AddComponent<Character>();
        nChar.SetCharacterScriptableObject(charactersScriptable[UnityEngine.Random.Range(0, charactersScriptable.Length)]);
        nChar.AddItem(gemsScriptable[UnityEngine.Random.Range(0, gemsScriptable.Length)], NItem.EPartType.Gem);

        characters[indexChar] = nChar;

        RefreshTeamScene();
    }

    public void SummonCharacter(CharacterScriptableObject cso)
    {
        GameObject nGameObject = new GameObject("Character");
        nGameObject.transform.parent = gameObject.transform;
        Character nChar = nGameObject.AddComponent<Character>();
        nChar.SetCharacterScriptableObject(cso);

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

    public void RefreshTeamScene()
    {
        for (int i = 0; i < slotsTeam.Length; i++)
        {
            bool charIsNull = AskForCharacter(i) == null;

            slotsTeam[i].transform.GetChild(4).gameObject.SetActive(charIsNull);

            /* Set characters sprites */
            GameObject spritesGo = slotsTeam[i].transform.GetChild(2).gameObject;
            Character character = AskForCharacter(i);

            // HEAD
            Image img = spritesGo.transform.GetChild(3).GetComponent<Image>();
            if (!charIsNull && character.charHead != null)
            {
                img.sprite = character.charHead;
                img.color = new Vector4(1f, 1f, 1f, 1f);
            }
            else
            {
                img.color = new Vector4(1f, 1f, 1f, 0f);
            }

            // BODY 
            img = spritesGo.transform.GetChild(1).GetComponent<Image>();
            if (!charIsNull && character.itemSprites[0] != null)
            {
                img.sprite = character.itemSprites[0];
                img.color = new Vector4(1f, 1f, 1f, 1f);
            }
            else
            {
                img.color = new Vector4(1f, 1f, 1f, 0f);
            }

            // HELMET
            img = spritesGo.transform.GetChild(4).GetComponent<Image>();
            if (!charIsNull && character.itemSprites[1] != null)
            {
                img.sprite = character.itemSprites[1];
                img.color = new Vector4(1f, 1f, 1f, 1f);
            }
            else
            {
                img.color = new Vector4(1f, 1f, 1f, 0f);
            }

            // ARMOR
            img = spritesGo.transform.GetChild(2).GetComponent<Image>();
            if (!charIsNull && character.itemSprites[2] != null)
            {
                img.sprite = character.itemSprites[2];
                img.color = new Vector4(1f, 1f, 1f, 1f);
            }
            else
            {
                img.color = new Vector4(1f, 1f, 1f, 0f);
            }

            // WEAPON
            img = spritesGo.transform.GetChild(0).GetComponent<Image>();
            if (!charIsNull && character.itemSprites[3] != null)
            {
                img.sprite = character.itemSprites[3];
                img.color = new Vector4(1f, 1f, 1f, 1f);
            }
            else
            {
                img.color = new Vector4(1f, 1f, 1f, 0f);
            }
        }
    }

    public void SelectCharacterStats(int indexChar)
    {
        if (characters[indexChar] == null)
            return;

        selectedChar = indexChar;

        for (int z = 0; z < 4; z++)
        {
            itemsUIPanel.transform.GetChild(z).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            if (z != 3)
            {
                itemsUIPanel.transform.GetChild(z).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        // SELECTED CHARACTER
        itemsUIPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = characters[indexChar].GetItem(NItem.EPartType.Head) != null ? characters[indexChar].GetItem(NItem.EPartType.Head).itemUiSprite : null;
        if (characters[indexChar].GetItem(NItem.EPartType.Head) != null)
        {
            int x = indexChar;
            NItem.ItemScriptableObject head = characters[indexChar].GetItem(NItem.EPartType.Head);

            //items.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowItemPanel(x, 0, head));
            itemsUIPanel.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelectItemStats(head));

            // Remove item
            itemsUIPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            itemsUIPanel.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveCharacterItem(indexChar, head));

            itemsUIPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            itemsUIPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

            itemsUIPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            itemsUIPanel.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.OpenInventory());
            itemsUIPanel.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.SelectionCharacterOneItemPart(indexChar));
            itemsUIPanel.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.SelectionOneItemPart(0));
        }

        itemsUIPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = characters[indexChar].GetItem(NItem.EPartType.Body) != null ? characters[indexChar].GetItem(NItem.EPartType.Body).itemUiSprite : null;
        if (characters[indexChar].GetItem(NItem.EPartType.Body) != null)
        {
            int x = indexChar;
            NItem.ItemScriptableObject body = characters[indexChar].GetItem(NItem.EPartType.Body);

            //items.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ShowItemPanel(x, 1, body));
            itemsUIPanel.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelectItemStats(body));

            // Remove item
            itemsUIPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            itemsUIPanel.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveCharacterItem(indexChar, body));

            itemsUIPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            itemsUIPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);

            itemsUIPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            itemsUIPanel.transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.OpenInventory());
            itemsUIPanel.transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.SelectionCharacterOneItemPart(indexChar));
            itemsUIPanel.transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.SelectionOneItemPart(1));

        }

        itemsUIPanel.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = characters[indexChar].GetItem(NItem.EPartType.Weapon) != null ? characters[indexChar].GetItem(NItem.EPartType.Weapon).itemUiSprite : null;
        if (characters[indexChar].GetItem(NItem.EPartType.Weapon) != null)
        {
            int x = indexChar;
            NItem.ItemScriptableObject weapon = characters[indexChar].GetItem(NItem.EPartType.Weapon);

            //items.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ShowItemPanel(x, 2, weapon));
            itemsUIPanel.transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelectItemStats(weapon));

            // Remove item
            itemsUIPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            itemsUIPanel.transform.GetChild(2).GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveCharacterItem(indexChar, weapon));

            itemsUIPanel.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            itemsUIPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);

            itemsUIPanel.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
            itemsUIPanel.transform.GetChild(2).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.OpenInventory());
            itemsUIPanel.transform.GetChild(2).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.SelectionCharacterOneItemPart(indexChar));
            itemsUIPanel.transform.GetChild(2).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.SelectionOneItemPart(2));
        }

        itemsUIPanel.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = characters[indexChar].GetItem(NItem.EPartType.Gem) != null ? characters[indexChar].GetItem(NItem.EPartType.Gem).itemUiSprite : null;
        if (characters[indexChar].GetItem(NItem.EPartType.Gem) != null)
        {
            int x = indexChar;
            NItem.ItemScriptableObject gem = characters[indexChar].GetItem(NItem.EPartType.Gem);

            //items.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => ShowItemPanel(x, 3, gem));
            itemsUIPanel.transform.GetChild(3).GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelectItemStats(gem));
        }

        // Show informations
        informationsUIPanel.transform.GetChild(0).GetComponent<Text>().text = characters[indexChar].charName;
        informationsUIPanel.transform.GetChild(1).GetComponent<Text>().text = "";

        // Show Stats
        statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = " Health " + characters[indexChar].maxHealth;
        statsUIPanel.transform.GetChild(1).GetComponent<Text>().text = " Armor " + characters[indexChar].armor;
        statsUIPanel.transform.GetChild(2).GetComponent<Text>().text = " Attack " + characters[indexChar].attack;
        statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " Dodge% " + characters[indexChar].dodge * 100f + "%";
        statsUIPanel.transform.GetChild(4).GetComponent<Text>().text = " Crit% " + characters[indexChar].criticalChance * 100f + "%";
        statsUIPanel.transform.GetChild(5).GetComponent<Text>().text = " CritDmg% " + characters[indexChar].crititalDamage * 100f + "%";
    }

    public void SelectItemStats(NItem.ItemScriptableObject item)
    {
        if (item == null)
            return; // Or et only item for this

        selectedItem = item;

        // Show informations
        informationsUIPanel.transform.GetChild(0).GetComponent<Text>().text = item.itemName;
        informationsUIPanel.transform.GetChild(1).GetComponent<Text>().text = item.itemRarity.ToString();

        // Show Stats
        string valueName = "";

        if (valueName == "" && item.health > 0)
        {
            statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = " Health " + item.health;
            valueName = "Health";
        }
        else if (valueName == "" && item.armor > 0)
        {
            statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = " Armor " + item.armor;
            valueName = "Armor";
        }
        else if (valueName == "" && item.attack > 0)
        {
            statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = " Attack " + item.attack;
            valueName = "Attack";
        }
        else if (valueName == "" && item.dodge > 0)
        {
            statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = " Dodge " + item.dodge;
            valueName = "Dodge";
        }
        else if (valueName == "" && item.criticalChance > 0)
        {
            statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = " Crit% " + item.criticalChance + "%";
            valueName = "Crit%";
        }
        else if (valueName == "" && item.crititalDamage > 0)
        {
            statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = " CritDmg " + item.crititalDamage * 100f + "%";
            valueName = "CritDmg";
        }

        if (valueName != "Health" && item.health > 0)
        {
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " Health " + item.health;
        }
        else if (valueName != "Armor" && item.armor > 0)
        {
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " Armor " + item.armor;
        }
        else if (valueName != "Attack" && item.attack > 0)
        {
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " Attack " + item.attack;
        }
        else if (valueName != "Dodge" && item.dodge > 0)
        {
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " Dodge " + item.dodge;
        }
        else if (valueName != "Crit%" && item.criticalChance > 0)
        {
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " Crit% " + item.criticalChance + "%";
        }
        else if (valueName != "CritDmg" && item.crititalDamage > 0)
        {
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " CritDmg " + item.crititalDamage * 100f + "%";
        }

        statsUIPanel.transform.GetChild(1).GetComponent<Text>().text =  item.itemType != NItem.EItemType.None ? " " + item.itemType.ToString() : ""; // gem
        statsUIPanel.transform.GetChild(2).GetComponent<Text>().text = "";
        statsUIPanel.transform.GetChild(4).GetComponent<Text>().text = "";
        statsUIPanel.transform.GetChild(5).GetComponent<Text>().text = "";
    }

    /*private void ShowItemPanel(int indexSlot, int indexPartItem, NItem.ItemScriptableObject item)
    {
        itemPanel.SetActive(true);

        Inventory inv = Inventory.inventory;

        Vector3 itemPos = slotsTeam[indexSlot].transform.GetChild(0).GetChild(indexPartItem).position;

        itemPanel.transform.position = itemPos + new Vector3(175f * inv.mainCanvas.localScale.x, 0f, 0f);

        for (int i = 0; i < 4; i++)
        {
            itemPanel.transform.GetChild(i).gameObject.SetActive(true);
            itemPanel.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        }

        itemPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => inv.OpenInventory());
        itemPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => inv.SelectionCharacterOneItemPart(indexSlot));
        itemPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => inv.SelectionOneItemPart(indexPartItem));

        if (item != null)
        {
            //itemPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => inv.ToggleItemStatsScreen(item));

            if (item.itemPartType != NItem.EPartType.Gem)
            {
                itemPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => RemoveCharacterItem(indexSlot, item));
                itemPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => OpenTeamScene());
            }
            else
            {
                itemPanel.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        else
        {
            itemPanel.transform.GetChild(1).gameObject.SetActive(false);
            itemPanel.transform.GetChild(2).gameObject.SetActive(false);
        }

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
    }*/

    private void RemoveCharacterItem(int characterIndex, NItem.ItemScriptableObject item)
    {
        Character character = AskForCharacter(characterIndex);

        Inventory.inventory.AddItem(item);

        character.RemoveItem(item.itemPartType);

        RefreshTeamScene();
        SelectCharacterStats(characterIndex);
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

    /*public void OpenCharacterSwapScene()
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

            // Set characters sprites 
            GameObject spritesGo = slots[i].transform.GetChild(0).GetChild(1).gameObject;
            Character character = AskForCharacter(i);
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


            Transform itemSlotTransform = slots[i].transform.GetChild(0).GetChild(0);
            NItem.ItemScriptableObject item = characters[i].GetItem(NItem.EPartType.Head);
            itemSlotTransform.GetChild(0).GetComponent<Image>().sprite = item != null ? item.itemUiSprite : null;
            if (item != null)
            {
                NItem.ItemScriptableObject nItem = characters[i].GetItem(NItem.EPartType.Head);
                itemSlotTransform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                //itemSlotTransform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.ToggleItemStatsScreen(nItem));
            }

            item = characters[i].GetItem(NItem.EPartType.Body);
            itemSlotTransform.GetChild(1).GetComponent<Image>().sprite = item != null ? item.itemUiSprite : null;
            if (item != null)
            {
                NItem.ItemScriptableObject nItem = characters[i].GetItem(NItem.EPartType.Body);
                itemSlotTransform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                //itemSlotTransform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.ToggleItemStatsScreen(nItem));
            }

            item = characters[i].GetItem(NItem.EPartType.Weapon);
            itemSlotTransform.GetChild(2).GetComponent<Image>().sprite = item != null ? item.itemUiSprite : null;
            if (item != null)
            {
                NItem.ItemScriptableObject nItem = characters[i].GetItem(NItem.EPartType.Weapon);
                itemSlotTransform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                //itemSlotTransform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.ToggleItemStatsScreen(nItem));
            }

            item = characters[i].GetItem(NItem.EPartType.Gem);
            itemSlotTransform.GetChild(3).GetComponent<Image>().sprite = item != null ? item.itemUiSprite : null;
            if (item != null)
            {
                NItem.ItemScriptableObject nItem = characters[i].GetItem(NItem.EPartType.Gem);
                itemSlotTransform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                //itemSlotTransform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.ToggleItemStatsScreen(nItem));
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
    }*/

    #endregion
}
