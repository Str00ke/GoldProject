using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGamesController : MonoBehaviour {

    private void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        // enables saving game progress.
        //.EnableSavedGames()
        // requests the email address of the player be available.
        // Will bring up a prompt for consent.
        //.RequestEmail()
        // requests a server auth code be generated so it can be passed to an
        //  associated back end server application and exchanged for an OAuth token.
        //.RequestServerAuthCode(false)
        // requests an ID token be generated.  This OAuth token can be used to
        //  identify the player to other services such as Firebase.
        //.RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
        
    }
    private void Start()
    {
        Authenticate();
    }

    public void Authenticate()
    {
        AuthenticateUser();
    }
    
    void AuthenticateUser()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            return;

        Social.localUser.Authenticate((bool success) =>
        {
            if (success == true)
            {
                Debug.Log("Logged in to Google Play Games Services");
                AchievementsManager.Logged();
                //SceneManager.LoadScene("LeaderboardUI");
            }
            else
            {
                Debug.LogError("Unable to sign in to Google Play Games Services");
            }
        });
        
    }

    public static void PostToLeaderboard(long newScore)
    {
        Social.ReportScore(newScore, GPGSIds.leaderboard_testleaderboard, (bool success) => {
            if (success)
            {
                Debug.Log("Posted new score to leaderboard");
            }
            else
            {
                Debug.LogError("Unable to post new score to leaderboard");
            }
        });
    }

    public static void ShowLeaderboardUI()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_testleaderboard);
    }

    public static void ShowAchievementsUI()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            Social.ShowAchievementsUI();
    }
}
