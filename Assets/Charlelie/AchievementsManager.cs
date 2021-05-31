using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public static class AchievementsManager
{
    static void UnlockAchievement(string ach)
    {
        Social.ReportProgress(ach, 100.00f, (bool success) =>
        {
            if (success)
                Debug.Log("Achievement unlocked: " + ach);
            else
                Debug.Log("Achievement problem for: " + ach);
        });
    }

    public static void Logged()
    {
        UnlockAchievement(GPGSIds.achievement_logged);
    }

    public static void DamageDeal(float value)
    {
        string achType = "";
        if (value >= 20)
        {
            if (value >= 50)
            {
                if (value >= 100)
                {
                    if (value >= 200)
                    {
                        achType = GPGSIds.achievement_damage_dealer_diamond;
                    }
                    else
                        achType = GPGSIds.achievement_damage_dealer_gold;
                }
                else
                    achType = GPGSIds.achievement_damage_dealer_silver;
            }
            else
                achType = GPGSIds.achievement_damage_dealer_copper;
        }
        else
            return;

        UnlockAchievement(achType);
    }

    public static void OnCombatEnd(CombatManager combatManager)
    {
        if (!combatManager.hasDied)
        {
            UnlockAchievement(GPGSIds.achievement_immortel);
        }

        if (!combatManager.hasTookDamage)
        {
            UnlockAchievement(GPGSIds.achievement_invicible);
        }

        if (!combatManager.hasHeal)
        {
            UnlockAchievement(GPGSIds.achievement_like_rambo);
        }
    }
}
