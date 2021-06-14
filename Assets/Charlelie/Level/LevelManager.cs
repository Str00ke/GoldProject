using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class LevelData
{
    static int _gold = 0;
    static int _souls = 0;
    static int _deaths = 0;
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

    public static int GetDeaths()
    {
        return _deaths;
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

    public static void AddDeath()
    {
        _deaths++;
    }

    public static void EraseData()
    {
        _gold = 0;
        _souls = 0;
        _so.Clear();
    }

    public static void EraseDeath()
    {
        _deaths = 0;
    }
}



public class LevelManager : MonoBehaviour
{
    public Level level;
    public string levelName;
    public GameObject combatPrefab, shop, obliterate, levelFinishedTxt, losePanel, pauseButton, enterRoom;
    GameObject combatRef;
    MapManager mapManager;
    public bool fightFMiniBoss, fightSMiniBoss, isFirstMiniBossDead, isSecondMiniBossDead = false;
    public MapRoom firstMiniBossRoom, secondMiniBossRoom;
    public Image fader;
    bool fadeInActive, fadeOutActive = false;
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
        FadeOut();
        levelName = FindObjectOfType<CGameManager>().GetLevelName();
        mapManager = MapManager.GetInstance();
        LoadLevel();
        mapManager.level = level;
        mapManager.Init();
        mapManager.GenerateMap();
        FindObjectOfType<PlayerPoint>().Init();
        mapManager.MapLinkRooms();
        StartCoroutine(mapManager.Test());
        //mapManager.StartToEnd(PlayerPoint._playerPoint.startRoom, 0);
        /*mapManager.RandomizeShop();
        StartRoom();
        shop.SetActive(false);
        obliterate.SetActive(false);
        EnnemyManager._enemyManager.SetRoomsDiff(mapManager.testMax);
        LevelData.EraseData();*/   
    }

    public void Continue()
    {
        mapManager.RandomizeShop();
        StartRoom();
        shop.SetActive(false);
        obliterate.SetActive(false);
        EnnemyManager._enemyManager.SetRoomsDiff(mapManager.testMax);
        LevelData.EraseData();
        LevelData.EraseDeath();
    }

    void FadeOut()
    {
        StartCoroutine(FadeOutCor(0.25f, isDone => { 
            if (isDone)
            {

            }
        }));
    }

    void FadeIn()
    {
        StartCoroutine(FadeInCor(0, isDone => {
            if (isDone)
            {

            }
        }));
    }

    public void FadeInOut(bool isInFight)
    {
        StartCoroutine(FadeInCor(0, isDone => {
            if (isDone)
            {
                if (!isInFight)
                    StartRoom();
                else
                    CombatManager.combatManager.EndFightAfterLoot();
                StartCoroutine(FadeOutCor(0.5f, isDone => {
                    if (isDone)
                    {
                        
                    }
                }));
            }
        }));
    }

    IEnumerator FadeOutCor(float startOff, System.Action<bool> isDone)
    {
        fader.maskable = true;
        fader.raycastTarget = true;
        yield return startOff;
        if (!fader.gameObject.activeSelf)
            fader.gameObject.SetActive(true);
        float a = 1;
        if (fader.color.a < 1)
            fader.color = Color.black;
        while (fader.color.a > 0)
        {
            if (a <= 0.5f)
                a -= Time.deltaTime;
            else
                a -= Time.deltaTime / 2;

            fader.color = new Color(0, 0, 0, a);
            
            yield return Time.deltaTime;
        }
        fader.color = new Color(0, 0, 0, 0);
        //fader.gameObject.SetActive(false);
        yield return null;
        fader.maskable = false;
        fader.raycastTarget = false;
        isDone(true);
        
    }

    IEnumerator FadeInCor(float startOff, System.Action<bool> isDone)
    {
        fader.maskable = true;
        fader.raycastTarget = true;
        yield return startOff;
        if (!fader.gameObject.activeSelf)
            fader.gameObject.SetActive(true);
        float a = 0;
        if (fader.color.a > 0)
            fader.color = new Color(0, 0, 0, 0);
        while (fader.color.a < 1)
        {
            a += Time.deltaTime;
            fader.color = new Color(0, 0, 0, a);
            yield return Time.deltaTime;
        }
        //fader.gameObject.SetActive(false);
        yield return null;
        fader.maskable = false;
        fader.raycastTarget = false;
        isDone(true);
    }

    void LoadLevel()
    {
        level = SaveSystem.Load(levelName);
    }

    public void WinFight()
    {
        AudioManager.audioManager.StopPlaying("ThemeFight");
        AudioManager.audioManager.StopPlaying("ThemeMenu");
        AudioManager.audioManager.Play("ThemeMap");
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
        enterRoom.SetActive(false);
        switch (room.roomType)
        {
            case RoomType.BASE:
                CreateFight();
                break;

            case RoomType.SHOP:
                SpawnShop();
                break;

            case RoomType.END:
                /*room.OnFinishRoom();
                levelFinishedTxt.SetActive(true);*/
                CreateFight();
                break;

            case RoomType.START:
                room.OnFinishRoom();
                break;

        }
    }

    void CreateFight()
    {
        AudioManager.audioManager.Play("EnterCombat");
        AudioManager.audioManager.StopPlaying("ThemeMenu");
        AudioManager.audioManager.StopPlaying("ThemeMap");
        AudioManager.audioManager.Play("ThemeFight");
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
        AudioManager.audioManager.StopPlaying("ThemeFight"); 
        AudioManager.audioManager.StopPlaying("ThemeMap");
        AudioManager.audioManager.Play("ThemeMenu");
        LobbyManager.lobbyManager.LoadScene("FScene");
    }


    public void UpdateDataValues()
    {
        if (!bool.Parse(PlayerPrefs.GetString("FirstGame", "false")) && LevelData.GetGold() != 0)
            PlayerPrefs.SetString("FirstGame", "true");

        Inventory.inventory.AddGolds(LevelData.GetGold());
        Inventory.inventory.AddSouls(LevelData.GetSouls());
        foreach (NItem.ItemScriptableObject so in LevelData.GetSO())
        {
            Inventory.inventory.AddItem(so);
        }
        Inventory.inventory.AddDeath(LevelData.GetDeaths());
        //LobbyManager.lobbyManager.AddScoreToLeaderboard();
    }
}
