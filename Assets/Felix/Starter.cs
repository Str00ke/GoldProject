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
    public Slider loadingSlider;
    public RectTransform charSpriteLoading;

    private void Start()
    {
        // If it's the player first launch
        // then load tutorial
        // LoadScene tuto

        // Else load lobby
        //LoadScene("FScene");
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

        float sliderSize = loadingSlider.GetComponent<RectTransform>().sizeDelta.x;

        while (!loadingAsync.isDone)
        {
            loadingSlider.value = loadingAsync.progress;

            charSpriteLoading.anchoredPosition = new Vector2((loadingSlider.value < 0.5f ? -1f : 1f) * (sliderSize / 2f) * (1 - loadingSlider.value), charSpriteLoading.anchoredPosition.y);

            if (loadingAsync.progress >= 0.9f)
            {
                loadingSlider.value = 0.99f;
                charSpriteLoading.anchoredPosition = new Vector2(sliderSize / 2f, charSpriteLoading.anchoredPosition.y);
                loadingAsync.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
