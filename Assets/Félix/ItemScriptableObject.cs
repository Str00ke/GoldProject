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
        [Space]
        public Sprite itemUiSprite;
    }
}
