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

    public static void PostToSoulLeaderboard(long newScore)
    {
        Debug.Log("Try to post score to leadeboard");
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.Log("I'm authenticated");
            Social.ReportScore(newScore, GPGSIds.leaderboard_soul_leaderboard, (bool success) => {
                if (success)
                {
                    Debug.Log("Posted new score to leaderboard");
                }
                else
                {
                    Debug.LogError("Unable to post new score to leaderboard");
                }
            });
        } else
            Debug.Log("Can't post score because you're not authenticated");
    }

    public static void PostToDeathLeaderboard(long newScore)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Social.ReportScore(newScore, GPGSIds.leaderboard_death_leaderboard, (bool success) => {
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
        else
            Debug.Log("Can't post score because you're not authenticated");
    }

    public static void ShowSoulLeaderboardUI()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_soul_leaderboard);
    }

    public static void ShowDeathLeaderboardUI()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_death_leaderboard);
    }

    public static void ShowAchievementsUI()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            Social.ShowAchievementsUI();
    }

    void test() //Get score from leaderboard
    {
        PlayGamesPlatform.Instance.LoadScores(
             GPGSIds.leaderboard_soul_leaderboard,
             LeaderboardStart.PlayerCentered,
             1,
             LeaderboardCollection.Public,
             LeaderboardTimeSpan.AllTime,
         (LeaderboardScoreData data) => {
             Debug.Log(data.Valid);
             Debug.Log(data.Id);
             Debug.Log(data.PlayerScore);
             Debug.Log(data.PlayerScore.userID);
             Debug.Log(data.PlayerScore.formattedValue);
         });
    }

}
