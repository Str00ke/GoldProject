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
        ARMORBONUSPERC,
        ARMORBONUSFLAT,
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
        MARK
    }
    public StatusTypes statusType;

    // ------------------------------------MARK & DOT------------------------------------------------------------------
    public Status(Characters target, float bm, Ability a, StatusTypes statusT, float damage)
    {
        bonusmalus = bm;
        statusTarget = target;
        turnsActive = a.turnDuration;
        statusType = statusT;
        dmg = damage;
        abi = a;
        statusElement = (StatusElement)System.Enum.Parse(typeof(StatusElement), a.elementType.ToString());
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
        if (statusTarget)
        {
            switch (statusType)
            {
                case StatusTypes.ARMORBONUSPERC:
                    diffModif = statusTarget.armor * bonusmalus;
                    statusTarget.armor += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.ARMORBONUSFLAT:
                    diffModif = bonusmalus;
                    statusTarget.armor += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.ARMORMALUS:
                    diffModif = statusTarget.armor * bonusmalus;
                    statusTarget.armor -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.BLEEDING:
                    diffModif = bonusmalus;
                    statusElement = StatusElement.BASE;
                    statusTarget.bleedingDmg += diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.HEALBONUS:
                    diffModif = bonusmalus;
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
                    statusTarget.dodge += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.DODGEMALUS:
                    diffModif = bonusmalus;
                    statusTarget.dodge -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.CRITDAMAGEBONUS:
                    diffModif = bonusmalus;
                    statusTarget.critDamage += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.CRITRATEBONUS:
                    diffModif = bonusmalus;
                    statusTarget.critChance += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.DAMAGEBONUS:
                    diffModif = bonusmalus;
                    statusTarget.damageRange.x += diffModif;
                    statusTarget.damageRange.y += diffModif;
                    buffOrDebuff = BuffOrDebuff.BUFF;
                    break;
                case StatusTypes.DAMAGEMALUS:
                    diffModif = bonusmalus;
                    statusTarget.damageRange.x -= diffModif;
                    statusTarget.damageRange.y -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.HEALTHDEBUFF:
                    diffModif = bonusmalus;
                    statusTarget.maxHealth -= diffModif;
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.DOT:
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
                    break;
                case StatusTypes.MARK:
                    buffOrDebuff = BuffOrDebuff.DEBUFF;
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
            case StatusTypes.ARMORBONUSPERC:
                statusTarget.armor -= diffModif;
                break;
            case StatusTypes.ARMORBONUSFLAT:
                statusTarget.armor -= diffModif;
                break;
            case StatusTypes.ARMORMALUS:
                statusTarget.armor += diffModif;
                break;
            case StatusTypes.BLEEDING:
                statusTarget.bleedingDmg -= diffModif;
                break;
            case StatusTypes.HEALBONUS:
                statusTarget.healReceivedModif -= diffModif;
                break;
            case StatusTypes.PRECISIONMALUS:
                statusTarget.precision += diffModif;
                break;
            case StatusTypes.DODGEBONUSFLAT:
                statusTarget.dodge -= diffModif;
                break;
            case StatusTypes.DODGEMALUS:
                statusTarget.dodge += diffModif;
                break;
            case StatusTypes.CRITDAMAGEBONUS:
                statusTarget.critDamage -= diffModif;
                break;
            case StatusTypes.CRITRATEBONUS:
                statusTarget.critChance -= diffModif;
                break;
            case StatusTypes.DAMAGEBONUS:
                statusTarget.damageRange.x -= diffModif;
                statusTarget.damageRange.y -= diffModif;
                break;
            case StatusTypes.DAMAGEMALUS:
                statusTarget.damageRange.x += diffModif;
                statusTarget.damageRange.y += diffModif;
                break;
            case StatusTypes.HEALTHDEBUFF:
                statusTarget.maxHealth += diffModif;
                break;
        }
        StatusManager.statusManager.DeleteDisplayStatus(statusTarget, this);
    }
}
