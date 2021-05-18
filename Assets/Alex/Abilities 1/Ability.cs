using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ability : ScriptableObject
{
    public new string name;
    public float multiplicator;
    public Sprite icon;
    public enum UnitType 
    {
        ALLY,
        ENEMY
    }
    public UnitType unitType;

    public enum WeaponAbilityType
    {
        BASE,
        WAVE,
        PIERCE,
        DEFENSE
    }
    public WeaponAbilityType weaponAbilityType;
    public enum TargetType 
    {
        MELEE,
        RANGE,
        TEAM
    }
    public TargetType targetType;
    public enum DamageType 
    {
        BASE,
        MUD,
        ASH,
        ICE,
        EROSION
    }
    public DamageType damageType;
    
}
