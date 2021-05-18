using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    Level level;
    string levelName;
    // Start is called before the first frame update
    void Start()
    {
        levelName = FindObjectOfType<CGameManager>().GetLevelName();
        MapManager mapManager = FindObjectOfType<MapManager>();
        LoadLevel();
        mapManager.level = level;
        mapManager.Init();        
        mapManager.GenerateMap();       
        //mapManager.InitPlayerPoint();
        mapManager.MapLinkRooms();
        mapManager.RandomizeShop();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadLevel()
    {
        level = SaveSystem.Load(levelName);
    }

   
}
