using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusTuto1
{
    public CharactersTuto statusTarget;
    public int turnsActive;
    public float bonusmalus;
    public float diffModif;
    public float dmg;
    public Ability abi;
    public int statusId;
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
        ARMORBONUS,
        ARMORMALUS,
        HEALBONUS,
        PRECISIONMALUS,
        DODGEBONUSFLAT,
        DODGEMALUS,
        CRITDAMAGEBONUS,
        CRITRATEBONUS,
        DAMAGEBONUS,
        DAMAGEMALUS,
        HEALTHDEBUFF,
        BLEEDING,
        DOT,
        MARK,
        STUN,
        DEFENCE
    }
    public StatusTypes statusType;

    // ------------------------------------MARK & DOT------------------------------------------------------------------
    public StatusTuto1(CharactersTuto target, CharactersTuto caster, float bm, Ability a, StatusTypes statusT, float damage)
    {
        bonusmalus = bm;
        statusTarget = target;
        turnsActive = a.turnDuration;
        statusType = statusT;
        dmg = damage;
        abi = a;
        if (a.elementType != Ability.ElementType.BASE)
            statusElement = (StatusElement)System.Enum.Parse(typeof(StatusElement), a.elementType.ToString());
        else
            statusElement = (StatusElement)System.Enum.Parse(typeof(StatusElement), caster.itemElement.ToString());
        StatusTuto.statusTuto.StatusList.Add(this);
        ApplyStatus();
    }
    // ------------------------------------DEBUFFS & BUFFS FROM CRISTALS----------------------------------------------
    public StatusTuto1(CharactersTuto target, float bm, Ability a, StatusTypes statusT)
    {
        bonusmalus = bm;
        statusTarget = target;
        turnsActive = a.turnDuration;
        statusType = statusT;
        abi = a;
        statusElement = (StatusElement)System.Enum.Parse(typeof(StatusElement), a.elementType.ToString());
        StatusTuto.statusTuto.StatusList.Add(this);
        ApplyStatus();
    }
    // ------------------------------------DEBUFFS & BUFFS FROM ELEMENT REACTIONS----------------------------------------
    public StatusTuto1(CharactersTuto target, float bm, int turns, StatusTypes statusT)
    {
        bonusmalus = bm;
        statusTarget = target;
        turnsActive = turns;
        statusType = statusT;
        StatusTuto.statusTuto.StatusList.Add(this);
        ApplyStatus();
    }
    public void ApplyStatus()
    {
        statusTarget.statusList.Add(this);
        if (statusTarget)
        {
            switch (statusType)
            {
                case StatusTypes.ARMORBONUS:
                    diffModif = bonusmalus;
                    statusTarget.armorBonus += diffModif;
                    statusTarget.armor += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.ARMORMALUS:
                    diffModif = statusTarget.armor * bonusmalus;
                    statusTarget.armorBonus -= diffModif;
                    statusTarget.armor -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.BLEEDING:
                    diffModif = bonusmalus;
                    statusTarget.dotDamage -= (int)diffModif;
                    statusElement = StatusElement.BASE;
                    statusTarget.bleedingDmg += diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.HEALBONUS:
                    diffModif = bonusmalus;
                    statusTarget.healBonus += (int)diffModif;
                    statusTarget.healReceivedModif += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.PRECISIONMALUS:
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
                case StatusTypes.DODGEBONUSFLAT:
                    diffModif = bonusmalus;
                    statusTarget.dodgeBonus += (int)diffModif;
                    statusTarget.dodge += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.DODGEMALUS:
                    diffModif = bonusmalus;
                    statusTarget.dodgeBonus -= (int)diffModif;
                    statusTarget.dodge -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.CRITDAMAGEBONUS:
                    diffModif = bonusmalus;
                    statusTarget.critDamageBonus += (int)diffModif;
                    statusTarget.critDamage += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.CRITRATEBONUS:
                    diffModif = bonusmalus;
                    statusTarget.critChanceBonus += (int)diffModif;
                    statusTarget.critChance += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.DAMAGEBONUS:
                    diffModif = bonusmalus;
                    statusTarget.damageBonus += (int)diffModif;
                    statusTarget.damageRange.x += diffModif;
                    statusTarget.damageRange.y += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.DAMAGEMALUS:
                    diffModif = bonusmalus;
                    statusTarget.damageBonus += (int)diffModif;
                    statusTarget.damageRange.x -= diffModif;
                    statusTarget.damageRange.y -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.HEALTHDEBUFF:
                    diffModif = bonusmalus;
                    statusTarget.healthDebuff += (int)diffModif;
                    statusTarget.maxHealth -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.DOT:
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    statusTarget.dotDamage += (int)dmg;
                    break;
                case StatusTypes.MARK:
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.STUN:
                    statusTarget.stunned = true;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.DEFENCE:
                    statusTarget.inDefenceMode = true;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
            }
            statusId = StatusTuto.statusTuto.statusId;
            StatusTuto.statusTuto.statusId++;
            StatusTuto.statusTuto.AddDisplayStatus(statusTarget, this);
        }
    }
    public void RevertStatus()
    {
        switch (statusType)
        {
            case StatusTypes.ARMORBONUS:
                statusTarget.armor -= diffModif;
                statusTarget.armorBonus -= (int)diffModif;
                break;
            case StatusTypes.ARMORMALUS:
                statusTarget.armor += diffModif;
                statusTarget.armorBonus += (int)diffModif;
                break;
            case StatusTypes.BLEEDING:
                statusTarget.bleedingDmg -= diffModif;
                statusTarget.dotDamage -= (int)diffModif;
                break;
            case StatusTypes.HEALBONUS:
                statusTarget.healReceivedModif -= diffModif;
                statusTarget.healBonus -= (int)diffModif;
                break;
            case StatusTypes.PRECISIONMALUS:
                statusTarget.precision += diffModif;
                break;
            case StatusTypes.DODGEBONUSFLAT:
                statusTarget.dodge -= diffModif;
                statusTarget.dodgeBonus -= (int)diffModif;
                break;
            case StatusTypes.DODGEMALUS:
                statusTarget.dodge += diffModif;
                statusTarget.dodgeBonus += (int)diffModif;
                break;
            case StatusTypes.CRITDAMAGEBONUS:
                statusTarget.critDamage -= diffModif;
                statusTarget.critDamageBonus -= (int)diffModif;
                break;
            case StatusTypes.CRITRATEBONUS:
                statusTarget.critChance -= diffModif;
                statusTarget.critChanceBonus -= (int)diffModif;
                break;
            case StatusTypes.DAMAGEBONUS:
                statusTarget.damageRange.x -= diffModif;
                statusTarget.damageRange.y -= diffModif;
                statusTarget.damageBonus -= (int)diffModif;
                break;
            case StatusTypes.DAMAGEMALUS:
                statusTarget.damageRange.x += diffModif;
                statusTarget.damageRange.y += diffModif;
                statusTarget.damageBonus += (int)diffModif;
                break;
            case StatusTypes.HEALTHDEBUFF:
                statusTarget.maxHealth += diffModif;
                statusTarget.healthDebuff += (int)diffModif;
                break;
            case StatusTypes.DOT:
                statusTarget.dotDamage -= (int)dmg;
                break;
            case StatusTypes.STUN:
                statusTarget.stunned = false;
                break;
            case StatusTypes.DEFENCE:
                statusTarget.inDefenceMode = false;
                buffOrDebuff = BuffOrDebuff.BUFF;
                break;
        }
        statusTarget.statusList.Remove(this);
        StatusTuto.statusTuto.DeleteDisplayStatus(statusTarget, this);
    }
}
