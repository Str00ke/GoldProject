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
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.Log("You're not logged in!");
            return;
        }

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
        List<string> achTypes = new List<string>();
        if (value >= 20)
        {
            achTypes.Add(GPGSIds.achievement_damage_dealer_copper);
            if (value >= 50)
            {
                achTypes.Add(GPGSIds.achievement_damage_dealer_silver);
                if (value >= 100)
                {
                    achTypes.Add(GPGSIds.achievement_damage_dealer_gold);
                    if (value >= 200)
                    {
                        achTypes.Add(GPGSIds.achievement_damage_dealer_diamond);
                    }    
                }     
            }            
        }
        else
            return;

        for (int i = 0; i < achTypes.Count - 1; ++i)
            UnlockAchievement(achTypes[i]);
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
