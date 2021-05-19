using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NItem
{
    public enum EItemType
    {
        None,
        Ice,
        Ash,
        Mud,
        Erosion
    }

    public enum EPartType
    {
        Head,
        Body,
        Weapon,
        Gem
    }

    public enum ERarity
    {
        Ultimate,
        Legendary,
        Epic,
        Rare,
        Normal
    }

    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemScriptableObject : ScriptableObject
    {
        public string itemName;
        [Space]
        public int health;
        public int armor;
        public int attack;
        public float dodge;
        public float criticalChance;
        public float crititalDamage;
        [Space]
        public EPartType itemPartType;
        public EItemType itemType;
        public ERarity itemRarity;
        [Space]
        public Sprite itemUiSprite;
    }
}
