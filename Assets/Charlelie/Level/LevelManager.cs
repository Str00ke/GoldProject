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
        _gold += value;
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
    string levelName;
    public GameObject combatPrefab, shop, obliterate, levelFinishedTxt, losePanel;
    GameObject combatRef;
    MapManager mapManager;
    public Text testTxt;
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


    // Start is called before the first frame update
    void Start()
    {
        levelName = FindObjectOfType<CGameManager>().GetLevelName();
        mapManager = MapManager.GetInstance();
        LoadLevel();
        mapManager.level = level;
        testTxt.text = "Start Map Init";
        mapManager.Init();
        testTxt.text = "Start Map Generation";
        mapManager.GenerateMap();
        //mapManager.InitPlayerPoint();
        testTxt.text = "Start Init PlayerPoint";
        FindObjectOfType<PlayerPoint>().Init();
        testTxt.text = "Start Link Rooms";
        mapManager.MapLinkRooms();
        testTxt.text = "Start Randomize Shop";
        mapManager.RandomizeShop();     //Something wrong here
        testTxt.text = "Start Check StartToEnd";
        mapManager.StartToEnd(PlayerPoint._playerPoint.startRoom, 0);
        testTxt.text = "Start StartRoom()";
        StartRoom();
        testTxt.text = "Disable Shop";
        shop.SetActive(false);
        testTxt.text = "Disable Obliterate";
        obliterate.SetActive(false);
        EnnemyManager._enemyManager.SetRoomsDiff(mapManager.testMax);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadLevel()
    {
        level = SaveSystem.Load(levelName);
        //testTxt.text = level.endRoom.pos.GetLength(0).ToString() + ", " + level.endRoom.pos.GetLength(1).ToString();
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
    }

    public void LoseFight()
    {
        Debug.Log("Game Over!");
        LevelData.EraseData();
    }

    public void StartRoom()
    {
        MapRoom room = PlayerPoint._playerPoint.onRoom;
        testTxt.text = (room.pos.GetLength(0) + "  " + room.pos.GetLength(1)).ToString();
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

        testTxt.text = "Function done";
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

    void SpawnBoss()
    {
        
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
        LobbyManager.lobbyManager.LoadScene("FScene");
    }

    /*public void OnGetLoot(Object loot)
    {
        switch (loot)
        {
            case /*Gold:
                Add
        }
    }*/

    public void UpdateDataValues()
    {
        Inventory.inventory.AddGolds(LevelData.GetGold());
        //Inventory.inventory.AddSouls(LevelData.GetSouls());
        foreach (NItem.ItemScriptableObject so in LevelData.GetSO())
        {
            Inventory.inventory.AddItem(so);
        }
    }
}
