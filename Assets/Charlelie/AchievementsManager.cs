using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public static class AchievementsManager
{
    public static void Logged()
    {
        Social.ReportProgress("CgkIv6KQjKwJEAIQCQ", 100.00, (bool success) => { });
    }
}
