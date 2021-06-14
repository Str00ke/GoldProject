using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public Characters statusTarget;
    public int turnsActive;
    public float bonusmalus;
    public float diffModif;
    public float dmg;
    public Ability abi;
    public int statusId;
    public Sprite statusSprite;
    public enum StatusElement
    {
        BASE,
        ICE,
        ASH,
        MUD,
        PSY
    }
    public StatusElement statusElement;
    public enum BuffOrDebuff
    {
        BUFF,
        DEBUFF
    }
    public BuffOrDebuff buffOrDebuff;
    public enum StatusTypes
    {
        ArmorBonus,
        ArmorMalus,
        HealBonus,
        PrecisionMalus,
        DodgeBonus,
        DodgeMalus,
        CriticDamageBonus,
        CriticRateBonus,
        DamageBonus,
        DamageMalus,
        HealthDebuff,
        Bleeding,
        Dot,
        Mark,
        Stun,
        Defence
    }
    public StatusTypes statusType;

    // ------------------------------------MARK & DOT------------------------------------------------------------------
    public Status(Characters target, Characters caster, float bm, Ability a, StatusTypes statusT, float damage)
    {
        bonusmalus = bm;
        statusTarget = target;
        turnsActive = a.turnDuration;
        statusType = statusT;
        dmg = damage;
        abi = a;
        if(a.elementType != Ability.ElementType.BASE)
            statusElement = (StatusElement)System.Enum.Parse(typeof(StatusElement), a.elementType.ToString());
        else
            statusElement = (StatusElement)System.Enum.Parse(typeof(StatusElement), caster.itemElement.ToString());
        StatusManager.statusManager.StatusList.Add(this);
        ApplyStatus();
    }
    // ------------------------------------DEBUFFS & BUFFS FROM CRISTALS----------------------------------------------
    public Status(Characters target, float bm, Ability a, StatusTypes statusT)
    {
        bonusmalus = bm;
        statusTarget = target;
        turnsActive = a.turnDuration;
        statusType = statusT;
        abi = a;
        statusElement = (StatusElement)System.Enum.Parse(typeof(StatusElement), a.elementType.ToString());
        StatusManager.statusManager.StatusList.Add(this);
        ApplyStatus();
    }
    // ------------------------------------DEBUFFS & BUFFS FROM ELEMENT REACTIONS----------------------------------------
    public Status(Characters target, float bm, int turns, StatusTypes statusT)
    {
        bonusmalus = bm;
        statusTarget = target;
        turnsActive = turns;
        statusType = statusT;
        StatusManager.statusManager.StatusList.Add(this);
        ApplyStatus();
    }
    public void ApplyStatus()
    {
        statusTarget.statusList.Add(this);
        if (statusTarget)
        {
            switch (statusType)
            {
                case StatusTypes.ArmorBonus:
                    diffModif = bonusmalus;
                    diffModif = Mathf.Round(diffModif);
                    statusTarget.armorBonus += diffModif;
                    statusTarget.armor += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.ArmorMalus:
                    diffModif = statusTarget.armor * bonusmalus;
                    diffModif = Mathf.Round(diffModif);
                    diffModif += 5;
                    statusTarget.armorBonus -= diffModif;
                    statusTarget.armor -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.HealBonus:
                    diffModif = bonusmalus;
                    statusTarget.healBonus += (int)diffModif;
                    statusTarget.healReceivedModif += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.PrecisionMalus:
                    if (statusTarget.precision < 1.0f)
                    {
                        statusTarget.precision = diffModif;
                    }
                    else
                    {
                        diffModif = bonusmalus;
                        statusTarget.precision -= diffModif;
                    }
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.DodgeBonus:
                    diffModif = bonusmalus;
                    diffModif = Mathf.Round(diffModif);
                    statusTarget.dodgeBonus += Mathf.Round(diffModif);
                    statusTarget.dodge += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.DodgeMalus:
                    diffModif = bonusmalus;
                    diffModif = Mathf.Round(diffModif);
                    statusTarget.dodgeBonus -= diffModif;
                    statusTarget.dodge -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.CriticDamageBonus:
                    diffModif = bonusmalus;
                    statusTarget.critDamageBonus += Mathf.Round(diffModif * 100);
                    statusTarget.critDamage += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.CriticRateBonus:
                    diffModif = bonusmalus;
                    statusTarget.critChanceBonus += Mathf.Round(diffModif * 100);
                    statusTarget.critChance += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.DamageBonus:
                    diffModif = bonusmalus;
                    diffModif = Mathf.Round(diffModif);
                    statusTarget.damageBonus += Mathf.Round(diffModif);
                    statusTarget.damageRange.x += diffModif;
                    statusTarget.damageRange.y += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.DamageMalus:
                    diffModif = bonusmalus;
                    diffModif = Mathf.Round(diffModif);
                    statusTarget.damageBonus -= Mathf.Round(diffModif);
                    statusTarget.damageRange.x -= diffModif;
                    statusTarget.damageRange.y -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.HealthDebuff:
                    diffModif = bonusmalus;
                    statusTarget.healthDebuff += (int)diffModif;
                    statusTarget.maxHealth -= diffModif;
                    if (statusTarget.maxHealth < statusTarget.health)
                        statusTarget.health = statusTarget.maxHealth;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.Dot:
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    statusTarget.dotDamage += (int)dmg;
                    break;
                case StatusTypes.Bleeding:
                    diffModif = bonusmalus;
                    statusTarget.dotDamage += (int)diffModif;
                    statusElement = StatusElement.BASE;
                    dmg = diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.Mark:
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.Stun:
                    statusTarget.stunned = true;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.Defence:
                    statusTarget.inDefenceMode = true;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
            }
            statusId = StatusManager.statusManager.statusId;
            StatusManager.statusManager.statusId++;
            StatusManager.statusManager.AddDisplayStatus(statusTarget, this);
        }
    }
    public void RevertStatus()
    {
        switch (statusType)
        {
            case StatusTypes.ArmorBonus:
                statusTarget.armor -= diffModif;
                statusTarget.armorBonus -= (int)diffModif;
                break;
            case StatusTypes.ArmorMalus:
                statusTarget.armor += diffModif;
                statusTarget.armorBonus += (int)diffModif;
                break;
            case StatusTypes.Bleeding:
                statusTarget.bleedingDmg -= diffModif;
                statusTarget.dotDamage -= (int)diffModif;
                break;
            case StatusTypes.HealBonus:
                statusTarget.healReceivedModif -= diffModif;
                statusTarget.healBonus -= (int)diffModif;
                break;
            case StatusTypes.PrecisionMalus:
                statusTarget.precision += diffModif;
                break;
            case StatusTypes.DodgeBonus:
                statusTarget.dodge -= diffModif;
                statusTarget.dodgeBonus -= (int)diffModif;
                break;
            case StatusTypes.DodgeMalus:
                statusTarget.dodge += diffModif;
                statusTarget.dodgeBonus += (int)diffModif;
                break;
            case StatusTypes.CriticDamageBonus:
                statusTarget.critDamage -= diffModif;
                statusTarget.critDamageBonus -= diffModif*100;
                break;
            case StatusTypes.CriticRateBonus:
                statusTarget.critChance -= diffModif;
                statusTarget.critChanceBonus -= diffModif * 100;
                break;
            case StatusTypes.DamageBonus:
                statusTarget.damageRange.x -= diffModif;
                statusTarget.damageRange.y -= diffModif;
                statusTarget.damageBonus -= (int)diffModif;
                break;
            case StatusTypes.DamageMalus:
                statusTarget.damageRange.x += diffModif;
                statusTarget.damageRange.y += diffModif;
                statusTarget.damageBonus += (int)diffModif;
                break;
            case StatusTypes.HealthDebuff:
                statusTarget.maxHealth += diffModif;
                statusTarget.healthDebuff += (int)diffModif;
                break;
            case StatusTypes.Dot:
                statusTarget.dotDamage -= (int)dmg;
                break;
            case StatusTypes.Stun:
                statusTarget.stunned = false;
                break;
            case StatusTypes.Defence:
                statusTarget.inDefenceMode = false;
                buffOrDebuff = BuffOrDebuff.BUFF;
                break;
        }
        statusTarget.statusList.Remove(this);
        StatusManager.statusManager.DeleteDisplayStatus(statusTarget, this);
    }
}
