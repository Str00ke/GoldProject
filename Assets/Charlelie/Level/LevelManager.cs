using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    Level level;
    string levelName;
    public GameObject combatPrefab, shop, obliterate, levelFinishedTxt, losePanel;
    GameObject combatRef;
    MapManager mapManager;
    public Text testTxt;

    static LevelManager _levelManager;


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
        mapManager.Init();        
        mapManager.GenerateMap();
        //mapManager.InitPlayerPoint();
        FindObjectOfType<PlayerPoint>().Init();
        mapManager.MapLinkRooms();
        mapManager.RandomizeShop();     //Something wrong here
        if (FindObjectOfType<PlayerPoint>())
            mapManager.StartToEnd(FindObjectOfType<PlayerPoint>().startRoom, 0);
        StartRoom();
        shop.SetActive(false);
        obliterate.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadLevel()
    {
        level = SaveSystem.Load(levelName);
        testTxt.text = level.endRoom.pos.GetLength(0).ToString() + ", " + level.endRoom.pos.GetLength(1).ToString();
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
    }

    public void StartRoom()
    {
        MapRoom room = FindObjectOfType<PlayerPoint>().onRoom;
        Debug.Log(room.roomType);
        switch (room.roomType)
        {
            case RoomType.BASE:
                CreateFight();
                break;

            case RoomType.SHOP:
                SpawnShop();
                break;

            case RoomType.END:
                Debug.Log("BOSS");
                room.OnFinishRoom();
                levelFinishedTxt.SetActive(true);
                break;

            case RoomType.START:
                Debug.Log("START");
                room.OnFinishRoom();
                break;

        }
    }

    void CreateFight()
    {
        if (!FindObjectOfType<PlayerPoint>().onRoom.isFinished)
        {
            float rand = Random.Range(0, 10);
            if (rand < 3)
            {
                FindObjectOfType<PlayerPoint>().onRoom.OnFinishRoom();
                return;
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
}
