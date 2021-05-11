using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int health;
    public int armor;
    public int attack;
    public float dodge;
    public float criticalChance;
    public float crititalDamage;

    public NItem.ItemScriptableObject head;
    public NItem.ItemScriptableObject body;
    public NItem.ItemScriptableObject weapon;
    public NItem.ItemScriptableObject gem;
}
