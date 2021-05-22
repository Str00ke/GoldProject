using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public Characters statusTarget;
    public int turnsActive;
    public float bonusmalus;
    public float diffModif;
    public enum StatusTypes
    {
        ARMORBONUSPERC,
        ARMORBONUSFLAT,
        ARMORMALUS,
        BLEEDING,
        HEALBONUS,
        PRECISIONMALUS,
        DODGEBONUSFLAT,
        DODGEMALUS,
        CRITDAMAGEBONUS,
        CRITRATEBONUS,
        DAMAGEBONUS,
        DAMAGEMALUS,
        HEALTHDEBUFF,
        DOTASH,
        DOTICE,
        DOTMUD,
        DOTPSY
    }
    public StatusTypes statusType;

    public Status(Characters target, float bm,  int turns, StatusTypes statusT)
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
                    break;
                case StatusTypes.ARMORBONUSFLAT:
                    diffModif = bonusmalus;
                    statusTarget.armor += diffModif;
                    break;
                case StatusTypes.ARMORMALUS:
                    diffModif = statusTarget.armor * bonusmalus;
                    statusTarget.armor -= diffModif;
                    break;
                case StatusTypes.BLEEDING:
                    diffModif = bonusmalus;
                    statusTarget.bleedingDmg += diffModif;
                    break;
                case StatusTypes.HEALBONUS:
                    diffModif = bonusmalus;
                    statusTarget.healReceivedModif += diffModif;
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
                    break;
                case StatusTypes.DODGEBONUSFLAT:
                    diffModif = bonusmalus;
                    statusTarget.dodge += diffModif;
                    break;
                case StatusTypes.DODGEMALUS:
                    diffModif = bonusmalus;
                    statusTarget.dodge -= diffModif;
                    break;
                case StatusTypes.CRITDAMAGEBONUS:
                    diffModif = bonusmalus;
                    statusTarget.critDamage += diffModif;
                    break;
                case StatusTypes.CRITRATEBONUS:
                    diffModif = bonusmalus;
                    statusTarget.critChance += diffModif;
                    break;
                case StatusTypes.DAMAGEBONUS:
                    diffModif = bonusmalus;
                    statusTarget.damageRange.x += diffModif;
                    statusTarget.damageRange.y += diffModif;
                    break;
                case StatusTypes.DAMAGEMALUS:
                    diffModif = bonusmalus;
                    statusTarget.damageRange.x -= diffModif;
                    statusTarget.damageRange.y -= diffModif;
                    break;
                case StatusTypes.HEALTHDEBUFF:
                    diffModif = bonusmalus;
                    statusTarget.maxHealth -= diffModif;
                    break;
                case StatusTypes.DOTASH:
                    diffModif = bonusmalus;
                    statusTarget.dmgDotAsh += diffModif;
                    break;
                case StatusTypes.DOTICE:
                    diffModif = bonusmalus;
                    statusTarget.dmgDotIce += diffModif;
                    break;
                case StatusTypes.DOTMUD:
                    diffModif = bonusmalus;
                    statusTarget.dmgDotMud += diffModif;
                    break;
                case StatusTypes.DOTPSY:
                    diffModif = bonusmalus;
                    statusTarget.dmgDotPsy += diffModif;
                    break;
            }
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
            case StatusTypes.DOTASH:
                statusTarget.dmgDotAsh -= diffModif;
                break;
            case StatusTypes.DOTICE:
                statusTarget.dmgDotIce -= diffModif;
                break;
            case StatusTypes.DOTMUD:
                statusTarget.dmgDotMud -= diffModif;
                break;
            case StatusTypes.DOTPSY:
                statusTarget.dmgDotPsy -= diffModif;
                break;
        }
    }
}
