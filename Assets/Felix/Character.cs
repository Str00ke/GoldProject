using UnityEngine;

public class Character : MonoBehaviour
{
    public enum ECharacterType
    {
        Char0,
        Char1,
        Char2
    }

    private CharacterScriptableObject charSO;

    public string charName;
    public int health;
    public int maxHealth;
    public int armor;
    public int attack;
    public float dodge;
    public float criticalChance;
    public float crititalDamage;
    public float initiative;

    public ECharacterType characterType;
    public Sprite charHead;
    public Sprite[] itemSprites;

    private NItem.ItemScriptableObject[] items;

    private void Awake()
    {
        items = new NItem.ItemScriptableObject[4];
        itemSprites = new Sprite[3];
    }

    public void SetCharacterScriptableObject(CharacterScriptableObject characterScriptableObject)
    {
        charName = characterScriptableObject.charName;
        health = characterScriptableObject.health;
        maxHealth = characterScriptableObject.health;
        armor = characterScriptableObject.armor;
        attack = characterScriptableObject.attack;
        dodge = characterScriptableObject.dodge;
        criticalChance = characterScriptableObject.criticalChance;
        crititalDamage = characterScriptableObject.crititalDamage;
        initiative = characterScriptableObject.initiative;

        characterType = characterScriptableObject.characterType;
        charHead = characterScriptableObject.characterHead;
    }

    public void AddItem(NItem.ItemScriptableObject item, NItem.EPartType itemPart)
    {
        int itemIndex = GetIndexItempart(itemPart);

        if (items[itemIndex] != null)
            RemoveItem(items[itemIndex].itemPartType);

        SetStats(item.health, item.armor, item.attack, item.dodge, item.criticalChance, item.crititalDamage);

        items[itemIndex] = item;

        if (item.itemPartType == NItem.EPartType.Gem)
        {
            itemSprites[0] = item.itemSprites[(int)characterType];
        }
        else if (item.itemPartType == NItem.EPartType.Head)
        {
            itemSprites[1] = item.itemSprites[(int)characterType];
        }
        else if (item.itemPartType == NItem.EPartType.Body)
        {
            itemSprites[2] = item.itemSprites[(int)characterType];
        }
    }

    public void RemoveItem(NItem.EPartType itemPart)
    {
        int itemIndex = GetIndexItempart(itemPart);

        SetStats(-items[itemIndex].health, -items[itemIndex].armor, -items[itemIndex].attack, -items[itemIndex].dodge, -items[itemIndex].criticalChance, -items[itemIndex].crititalDamage);

        items[itemIndex] = null;

        if (itemPart == NItem.EPartType.Gem)
        {
            itemSprites[0] = null;
        }
        else if (itemPart == NItem.EPartType.Head)
        {
            itemSprites[1] = null;
        }
        else if (itemPart == NItem.EPartType.Body)
        {
            itemSprites[2] = null;
        }
    }

    private void SetStats(int _health, int _armor, int _attack, float _dodge, float _criticalChance, float _criticalDamage)
    {
        health += _health;
        maxHealth += _health;
        armor += _armor;
        attack += _attack;
        dodge += _dodge;
        criticalChance += _criticalChance;
        crititalDamage += _criticalDamage;
    }

    private int GetIndexItempart(NItem.EPartType itemPart)
    {
        switch (itemPart)
        {
            case NItem.EPartType.Head:
                return 0;
            case NItem.EPartType.Body:
                return 1;
            case NItem.EPartType.Weapon:
                return 2;
            case NItem.EPartType.Gem:
                return 3;
            default:
                break;
        }

        return 0;
    }

    public NItem.ItemScriptableObject GetItem(NItem.EPartType itemPart)
    {
        switch (itemPart)
        {
            case NItem.EPartType.Head:
                return items[0];
            case NItem.EPartType.Body:
                return items[1];
            case NItem.EPartType.Weapon:
                return items[2];
            case NItem.EPartType.Gem:
                return items[3];
            default:
                break;
        }

        return null;
    }
}
