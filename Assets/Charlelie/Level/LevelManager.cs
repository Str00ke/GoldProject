using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    Level level;
    string levelName;
    public GameObject combatPrefab;
    GameObject combatRef;
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

    public void WinFight()
    {
        Destroy(combatRef);
        Ally[] alliesToDestroy = FindObjectsOfType<Ally>();
        foreach (Ally ally in alliesToDestroy)
            Destroy(ally.gameObject);
        FindObjectOfType<MapManager>().ShowMap();
        FindObjectOfType<MapManager>().UpdateBtn();
        FindObjectOfType<MapManager>().OnFinishRoom();
    }

    public void LoseFight()
    {
        Debug.Log("Game Over!");
    }

    public void CreateFight()
    {
        if (!FindObjectOfType<PlayerPoint>().onRoom.isFinished)
        {
            combatRef = Instantiate(combatPrefab, Vector2.zero, transform.rotation);
            FindObjectOfType<MapManager>().ShowMap();
            FindObjectOfType<MapManager>().UpdateBtn();
        }
        
    }

    

    public void Obliterate()
    {
        FindObjectOfType<CombatManager>().Obliterate();
    }

}
