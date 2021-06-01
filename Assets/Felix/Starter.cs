using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Starter : MonoBehaviour
{
    [Header("Loading Screen")]
    private AsyncOperation loadingAsync;
    public GameObject loadingScene;
    public Image loadingSlider;

    private void Start()
    {
        // If it's the player first launch
        // then load tutorial
        // LoadScene tuto

        // Else load lobby
        //LoadScene("FScene");

        if (LobbyManager.lobbyManager)
            Destroy(LobbyManager.lobbyManager.mainCanvas.gameObject);
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
