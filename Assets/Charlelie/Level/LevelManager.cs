using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class LevelData
{
    static int _gold = 0;
    static int _souls = 0;
    static List<NItem.ItemScriptableObject> _so = new List<NItem.ItemScriptableObject>();


    public static void SetData(int gold, int souls, List<NItem.ItemScriptableObject> so)
    {
        _gold = gold;
        _souls = souls;
        _so = so;
    }


    public static int GetGold()
    {
        return _gold;
    }

    public static int GetSouls()
    {
        return _souls;
    }

    public static List<NItem.ItemScriptableObject> GetSO()
    {
        return _so;
    }

    public static void AddSoToList(NItem.ItemScriptableObject so)
    {
        _so.Add(so);
    }

    public static void AddGold(int value)
    {
        _gold += value * LootManager.lootManager.goldValue;
        Debug.Log("Gold: " + _gold);
    }

    public static void AddSouls(int value)
    {
        _souls += value;
    }

    public static void EraseData()
    {
        _gold = 0;
        _souls = 0;
        _so.Clear();
    }
}



public class LevelManager : MonoBehaviour
{
    public Level level;
    public string levelName;
    public GameObject combatPrefab, shop, obliterate, levelFinishedTxt, losePanel, pauseButton;
    GameObject combatRef;
    MapManager mapManager;
    public bool fightFMiniBoss, fightSMiniBoss = false;
    static LevelManager _levelManager;
    [Header("Rate for fighting room")]
    public float fightRate = 30;


    void Awake()
    {
        if (_levelManager != null && _levelManager != this)
            Destroy(gameObject);

        _levelManager = this;
    }

    public static LevelManager GetInstance()
    {
        return _levelManager;
    }


    void Start()
    {
        levelName = FindObjectOfType<CGameManager>().GetLevelName();
        mapManager = MapManager.GetInstance();
        LoadLevel();
        mapManager.level = level;
        mapManager.Init();
        mapManager.GenerateMap();
        FindObjectOfType<PlayerPoint>().Init();
        mapManager.MapLinkRooms();     
        mapManager.StartToEnd(PlayerPoint._playerPoint.startRoom, 0);
        mapManager.RandomizeShop();
        StartRoom();
        shop.SetActive(false);
        obliterate.SetActive(false);
        EnnemyManager._enemyManager.SetRoomsDiff(mapManager.testMax);
        LevelData.EraseData();
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
        obliterate.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void LoseFight()
    {
        Debug.Log("Game Over!");
        LevelData.EraseData();
    }

    public void StartRoom()
    {
        MapRoom room = PlayerPoint._playerPoint.onRoom;
        switch (room.roomType)
        {
            case RoomType.BASE:
                CreateFight();
                break;

            case RoomType.SHOP:
                SpawnShop();
                break;

            case RoomType.END:
                room.OnFinishRoom();
                levelFinishedTxt.SetActive(true);
                break;

            case RoomType.START:
                room.OnFinishRoom();
                break;

        }
    }

    void CreateFight()
    {
        if (!FindObjectOfType<PlayerPoint>().onRoom.isFinished)
        {
            if (!EnnemyManager._enemyManager.CheckIfOnBossRoom(PlayerPoint._playerPoint.onRoom))
            {
                float rand = Random.Range(0, 100);
                if (rand > fightRate)
                {
                    FindObjectOfType<PlayerPoint>().onRoom.OnFinishRoom();
                    return;
                }
            }
            
            combatRef = Instantiate(combatPrefab, Vector2.zero, transform.rotation);
            FindObjectOfType<MapManager>().ShowMap();
            FindObjectOfType<MapManager>().UpdateBtn();
            obliterate.SetActive(true);
            pauseButton.SetActive(false);
        }
    }

    void SpawnShop()
    {
        shop.SetActive(true);
        FindObjectOfType<MapManager>().ShowMap();
    }

    public void LeaveShop()
    {
        FindObjectOfType<MapManager>().ShowMap();
        FindObjectOfType<PlayerPoint>().onRoom.OnFinishRoom();
        shop.SetActive(false);
    }


    public void Obliterate()
    {
        CombatManager.combatManager.Obliterate();
    }


    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ReturnToLobby()
    {
        UpdateDataValues();
        LobbyManager.lobbyManager.LoadScene("FScene");
    }


    public void UpdateDataValues()
    {
        Inventory.inventory.AddGolds(LevelData.GetGold());
        Inventory.inventory.AddSouls(LevelData.GetSouls());
        foreach (NItem.ItemScriptableObject so in LevelData.GetSO())
        {
            Inventory.inventory.AddItem(so);
        }
        //LobbyManager.lobbyManager.AddScoreToLeaderboard();
    }
}
