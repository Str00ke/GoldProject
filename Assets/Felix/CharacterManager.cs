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
    public NItem.ItemScriptableObject stickSo;

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
        ResetCharacterStats();
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
        nChar.AddItem(stickSo, NItem.EPartType.Weapon);

        characters[indexChar] = nChar;

        RefreshTeamScene();
        SelectCharacterStats(indexChar);
    }

    public void RemoveCharacter(Character character)
    {
        Destroy(character.gameObject);

        RefreshTeamScene();
        SelectCharacterStats(0);
    }

    #region TeamScene

    public void RefreshTeamScene()
    {
        for (int i = 0; i < 3; i++)
        {
            if (characters[i] != null)
                characters[i].ResetHealth();
        }

        for (int i = 0; i < slotsTeam.Length; i++)
        {
            bool charIsNull = AskForCharacter(i) == null;

            // Summon button
            slotsTeam[i].transform.GetChild(4).gameObject.SetActive(charIsNull);

            // Carroussel
            if (i == 0 || charIsNull)
                slotsTeam[i].transform.GetChild(5).gameObject.SetActive(false);
            else
                slotsTeam[i].transform.GetChild(5).gameObject.SetActive(true);

            if (i == slotsTeam.Length - 1 || charIsNull)
                slotsTeam[i].transform.GetChild(6).gameObject.SetActive(false);
            else
                slotsTeam[i].transform.GetChild(6).gameObject.SetActive(true);

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
            FloatingObject floatingObject = spritesGo.transform.GetChild(0).gameObject.GetComponent<FloatingObject>();
            img = spritesGo.transform.GetChild(0).GetComponent<Image>();
            if (!charIsNull && character.itemSprites[3] != null)
            {
                img.sprite = character.itemSprites[3];
                img.color = new Vector4(1f, 1f, 1f, 1f);
                floatingObject.isPlaying = true;
            }
            else
            {
                img.color = new Vector4(1f, 1f, 1f, 0f);
                floatingObject.isPlaying = false;
            }
        }
    }

    public void ResetCharacterStats()
    {
        selectedChar = 0;

        for (int z = 0; z < 4; z++)
        {
            itemsUIPanel.transform.GetChild(z).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            if (z != 3 && z != 2)
            {
                itemsUIPanel.transform.GetChild(z).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                itemsUIPanel.transform.GetChild(z).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        // SELECTED CHARACTER ITEMS
        for (int i = 0; i < 2; i++)
        {
            itemsUIPanel.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
            itemsUIPanel.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            itemsUIPanel.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
        }

        // Show informations
        informationsUIPanel.transform.GetChild(0).GetComponent<Text>().text = "";
        informationsUIPanel.transform.GetChild(1).GetComponent<Text>().text = "";

        // Show Stats
        statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = "";
        statsUIPanel.transform.GetChild(1).GetComponent<Text>().text = "";
        statsUIPanel.transform.GetChild(2).GetComponent<Text>().text = "";
        statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = "";
        statsUIPanel.transform.GetChild(4).GetComponent<Text>().text = "";
        statsUIPanel.transform.GetChild(5).GetComponent<Text>().text = "";
    }

    public void SelectCharacterStats(int indexChar)
    {
        int tempActualChar = selectedChar;

        if (indexChar != -1)
            selectedChar = indexChar;

        if (characters[selectedChar] == null)
        {
            selectedChar = tempActualChar;

            ResetCharacterStats();
            return;
        }

        LobbyManager lobbyManager = LobbyManager.lobbyManager;

        for (int z = 0; z < 4; z++)
        {
            itemsUIPanel.transform.GetChild(z).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            if (z != 3 && z != 2)
            {
                itemsUIPanel.transform.GetChild(z).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                itemsUIPanel.transform.GetChild(z).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        // SELECTED CHARACTER
        itemsUIPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = characters[selectedChar].GetItem(NItem.EPartType.Head) != null ? characters[selectedChar].GetItem(NItem.EPartType.Head).itemUiSprite : null;
        if (characters[selectedChar].GetItem(NItem.EPartType.Head) != null)
        {
            int x = selectedChar;
            NItem.ItemScriptableObject head = characters[selectedChar].GetItem(NItem.EPartType.Head);
            itemsUIPanel.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.inventory.raritySprites[(int)head.itemRarity];

            //items.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ShowItemPanel(x, 0, head));
            itemsUIPanel.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelectItemStats(head));

            // Remove item
            itemsUIPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            itemsUIPanel.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveCharacterItem(selectedChar, head));

            itemsUIPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            itemsUIPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            itemsUIPanel.transform.GetChild(0).GetComponent<Image>().sprite = null;

            if (lobbyManager.lobbyState == ELobbyState.Menu || lobbyManager.lobbyState == ELobbyState.Inventory || lobbyManager.lobbyState == ELobbyState.InventoryItemPartSelection || lobbyManager.lobbyState == ELobbyState.Shop)
            {
                itemsUIPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                itemsUIPanel.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.OpenInventory());
                itemsUIPanel.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.SelectionCharacterOneItemPart(selectedChar));
                itemsUIPanel.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.SelectionOneItemPart(0));
            }
            else
            {
                itemsUIPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            }
        }

        itemsUIPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = characters[selectedChar].GetItem(NItem.EPartType.Body) != null ? characters[selectedChar].GetItem(NItem.EPartType.Body).itemUiSprite : null;
        if (characters[selectedChar].GetItem(NItem.EPartType.Body) != null)
        {
            int x = selectedChar;
            NItem.ItemScriptableObject body = characters[selectedChar].GetItem(NItem.EPartType.Body);
            itemsUIPanel.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.inventory.raritySprites[(int)body.itemRarity];

            //items.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ShowItemPanel(x, 1, body));
            itemsUIPanel.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelectItemStats(body));

            // Remove item
            itemsUIPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            itemsUIPanel.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveCharacterItem(selectedChar, body));

            itemsUIPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            itemsUIPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            itemsUIPanel.transform.GetChild(1).GetComponent<Image>().sprite = null;

            if (lobbyManager.lobbyState == ELobbyState.Menu || lobbyManager.lobbyState == ELobbyState.Inventory || lobbyManager.lobbyState == ELobbyState.InventoryItemPartSelection || lobbyManager.lobbyState == ELobbyState.Shop)
            {
                itemsUIPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                itemsUIPanel.transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.OpenInventory());
                itemsUIPanel.transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.SelectionCharacterOneItemPart(selectedChar));
                itemsUIPanel.transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Inventory.inventory.SelectionOneItemPart(1));
            }
            else
            {
                itemsUIPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            }

        }

        itemsUIPanel.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = characters[selectedChar].GetItem(NItem.EPartType.Weapon) != null ? characters[selectedChar].GetItem(NItem.EPartType.Weapon).itemUiSprite : null;
        if (characters[selectedChar].GetItem(NItem.EPartType.Weapon) != null)
        {
            int x = selectedChar;
            NItem.ItemScriptableObject weapon = characters[selectedChar].GetItem(NItem.EPartType.Weapon);
            itemsUIPanel.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.inventory.raritySprites[(int)weapon.itemRarity];

            itemsUIPanel.transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelectItemStats(weapon));
        }
        else
        {
            itemsUIPanel.transform.GetChild(2).GetComponent<Image>().sprite = null;
        }

        itemsUIPanel.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = characters[selectedChar].GetItem(NItem.EPartType.Gem) != null ? characters[selectedChar].GetItem(NItem.EPartType.Gem).itemUiSprite : null;
        if (characters[selectedChar].GetItem(NItem.EPartType.Gem) != null)
        {
            int x = selectedChar;
            NItem.ItemScriptableObject gem = characters[selectedChar].GetItem(NItem.EPartType.Gem);
            itemsUIPanel.transform.GetChild(3).GetComponent<Image>().sprite = Inventory.inventory.raritySprites[(int)gem.itemRarity];

            itemsUIPanel.transform.GetChild(3).GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelectItemStats(gem));
        }
        else
        {
            itemsUIPanel.transform.GetChild(3).GetComponent<Image>().sprite = null;
        }

        if (indexChar != -1)
        {
            // Show informations
            informationsUIPanel.transform.GetChild(0).GetComponent<Text>().text = characters[indexChar].charName;
            informationsUIPanel.transform.GetChild(1).GetComponent<Text>().text = "";

            // Show Stats
            statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = " Health " + characters[indexChar].maxHealth;
            statsUIPanel.transform.GetChild(1).GetComponent<Text>().text = " Armor " + characters[indexChar].armor;
            statsUIPanel.transform.GetChild(2).GetComponent<Text>().text = " Attack " + characters[indexChar].attack;
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " Dodge " + characters[indexChar].dodge * 100f + "%";
            statsUIPanel.transform.GetChild(4).GetComponent<Text>().text = " Crit " + characters[indexChar].criticalChance * 100f + "%";
            statsUIPanel.transform.GetChild(5).GetComponent<Text>().text = " CritDmg " + characters[indexChar].crititalDamage * 100f + "%";
        }
    }

    public void SelectItemStats(NItem.ItemScriptableObject item)
    {
        if (item == null)
            return;

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
            statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = " Dodge " + item.dodge * 100f + "%";
            valueName = "Dodge";
        }
        else if (valueName == "" && item.criticalChance > 0)
        {
            statsUIPanel.transform.GetChild(0).GetComponent<Text>().text = " Crit " + item.criticalChance * 100f + "%";
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
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " Dodge " + item.dodge * 100f + "%";
        }
        else if (valueName != "Crit%" && item.criticalChance > 0)
        {
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " Crit " + item.criticalChance * 100f + "%";
        }
        else if (valueName != "CritDmg" && item.crititalDamage > 0)
        {
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " CritDmg " + item.crititalDamage * 100f + "%";
        }
        else if (item.itemType.ToString() != "None")
        {
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = " " + item.itemType.ToString();
        }
        else
        {
            statsUIPanel.transform.GetChild(3).GetComponent<Text>().text = "";
        }

        statsUIPanel.transform.GetChild(1).GetComponent<Text>().text = "";
        statsUIPanel.transform.GetChild(2).GetComponent<Text>().text = "";
        statsUIPanel.transform.GetChild(4).GetComponent<Text>().text = "";
        statsUIPanel.transform.GetChild(5).GetComponent<Text>().text = "";
    }

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

    public void SwapRight(int indexChar)
    {
        if (indexChar <= 0)
            return;

        Character characterTemp = characters[indexChar];
        characters[indexChar] = characters[indexChar - 1];
        characters[indexChar - 1] = characterTemp;

        RefreshTeamScene();
        SelectCharacterStats(indexChar - 1);
        Inventory.inventory.CloseInventory();
    }

    public void SwapLeft(int indexChar)
    {
        if (indexChar >= characters.Length - 1)
            return;

        Character characterTemp = characters[indexChar];
        characters[indexChar] = characters[indexChar + 1];
        characters[indexChar + 1] = characterTemp;

        RefreshTeamScene();
        SelectCharacterStats(indexChar + 1);
        Inventory.inventory.CloseInventory();
    }

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
