using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EElement
{
    BASE,
    ICE,
    ASH,
    MUD,
    PSY
}

public enum EEnemyType
{
    BASE,
    SNAKE,
    GIRAFFE
}


[CreateAssetMenu]
public class Ennemy : ScriptableObject
{
    //name maxHealth damageRange(V2) dodge(0 - 1) critChance(0 - 1) critDamage(0 - 1) initiative armor (element? ->enum: base/Ice/Ash/Mud/Psy (in order))
    public string enemyName;
    //public Sprite sprite;
    public EEnemyType enemyType;
    public Vector2 damageRange;
    [Range(0, 1)]
    public float dodge;
    [Range(0, 1)]
    public float critChance;
    [Range(0, 1)]
    public float critDamage;
    public float initiative;
    public float armor;
    public EElement element;
}
