using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Starter : MonoBehaviour
{
    private bool isTutorialEnded = false;

    [Header("Loading Screen")]
    private AsyncOperation loadingAsync;
    public GameObject loadingScene;
    public Image loadingSlider;
    public string levelName = "FScene";

    private void Start()
    {
        isTutorialEnded = bool.Parse(PlayerPrefs.GetString("TutoEnded", "false"));
        if (LobbyManager.lobbyManager)
            Destroy(LobbyManager.lobbyManager.mainCanvas.gameObject);
    }

    public void StartButton()
    {
        if (isTutorialEnded)
            LoadScene(levelName);
        else
        {
            LoadScene("TutoScene");
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingScreen(sceneName));
    }

    IEnumerator LoadingScreen(string sceneName)
    {
        loadingScene.SetActive(true);
        loadingAsync = SceneManager.LoadSceneAsync(sceneName);
        loadingAsync.allowSceneActivation = false;

        while (!loadingAsync.isDone)
        {
            loadingSlider.fillAmount = loadingAsync.progress;

            if (loadingAsync.progress >= 0.9f)
            {
                loadingSlider.fillAmount = 1f;
                loadingAsync.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
