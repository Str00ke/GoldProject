using UnityEngine;

public class Character : MonoBehaviour
{
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

    private NItem.ItemScriptableObject[] items;

    private void Awake()
    {
        items = new NItem.ItemScriptableObject[4];
    }

    public Character()
    {

    }

    public Character(CharacterScriptableObject characterScriptableObject)
    {
        charSO = characterScriptableObject;
        SetCharacterScriptableObject(characterScriptableObject);
    }

    private void SetCharacterScriptableObject(CharacterScriptableObject characterScriptableObject)
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
    }

    public void AddItem(NItem.ItemScriptableObject item, NItem.EPartType itemPart)
    {
        int itemIndex = GetIndexItempart(itemPart);

        if (items[itemIndex] != null)
            RemoveItem(items[itemIndex].itemPartType);

        SetStats(item.health, item.armor, item.attack, item.dodge, item.criticalChance, item.crititalDamage);

        items[itemIndex] = item;
    }

    public void RemoveItem(NItem.EPartType itemPart)
    {
        int itemIndex = GetIndexItempart(itemPart);

        SetStats(-items[itemIndex].health, -items[itemIndex].armor, -items[itemIndex].attack, -items[itemIndex].dodge, -items[itemIndex].criticalChance, -items[itemIndex].crititalDamage);

        items[itemIndex] = null;
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
