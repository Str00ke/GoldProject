using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager : MonoBehaviour
{
    static string levelName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevelName(string name)
    {
        levelName = name;
    }

    public string GetLevelName()
    {
        return levelName;
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void SaveProgressToPlayerPrefs(int currDungeon)
    {
        if (PlayerPrefs.GetInt("levelHigh") <= currDungeon)
            PlayerPrefs.SetInt("levelHigh", currDungeon);
    }
}
