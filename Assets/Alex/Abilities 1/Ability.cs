using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ability : ScriptableObject
{
    public new string name;
    public string type;
    public float multiplicator;
    public Sprite icon;
    public float bonusmalus;
    public float destruModif;
    public float dotMult;
    public float markMult;
    public int turnDuration;

    public enum ObjectType 
    {
        CRISTAL,
        WEAPON
    }
    public ObjectType objectType;
    public enum WeaponAbilityType
    {
        NONE,
        BASE,
        WAVE,
        PIERCE,
        DEFENSE
    }
    public WeaponAbilityType weaponAbilityType;


    public enum CristalAbilityType
    {
        NONE,
        HEAL,
        ATTACK,
        OTHERS
    }
    public CristalAbilityType crType;

    public enum CristalHealType 
    {
        NONE,
        BOOST,
        BATH,
        DRINK
    }
    public CristalHealType crHealType;
    public enum CristalAttackType
    {
        NONE,
        NORMAL,
        DOT,
        MARK
    }
    public CristalAttackType crAttackType;
    public enum CristalSpecialType
    {
        DESTRUCTION,
        COPY
    }
    public CristalSpecialType crSpecialType;
    
    public enum TargetType
    {
        MELEE,
        RANGE,
        ALLIES
    }
    public TargetType targetType;
    public enum ElementType
    {
        BASE,
        ASH,
        ICE,
        MUD,
        PSY
    }
    public ElementType elementType;
    
}
