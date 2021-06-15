using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EElement
{
    ICE,
    ASH,
    MUD,
    PSY
}

public enum EEnemyType
{
    SNAKE,
    GIRAFFE,
    DEATH
}


[CreateAssetMenu]
public class Ennemy : ScriptableObject
{
    //name maxHealth damageRange(V2) dodge(0 - 1) critChance(0 - 1) critDamage(0 - 1) initiative armor (element? ->enum: base/Ice/Ash/Mud/Psy (in order))
    public string enemyName;
    //public Sprite sprite;
    public int maxHealth;
    public EEnemyType enemyType;
    public Vector2 damageRange;
    public float dodge;
    public float critChance;
    public float critDamage;
    public float initiative;
    public float armor;
    public EElement element;
    public Sprite[] enemySprites;
    [Header("Abilities")]
    public Ability[] enemyAbilities;
    public Ability[] enemySpecialsAbilities;
}
