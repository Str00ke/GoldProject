using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public static class AchievementsManager
{
    public static void Logged()
    {   
        Social.ReportProgress(GPGSIds.achievement_logged, 100.00f, (bool success) =>
        {
            if (success)
                Debug.Log("Achievement unlocked");
            else
                Debug.Log("Achievement problem");
        });
    }
}
