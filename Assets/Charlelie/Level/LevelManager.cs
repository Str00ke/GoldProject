using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    Level level;
    string levelName;
    public GameObject combatPrefab, shop, obliterate;
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
        mapManager.StartToEnd(FindObjectOfType<PlayerPoint>().startRoom);

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
            combatRef = Instantiate(combatPrefab, Vector2.zero, transform.rotation);
            FindObjectOfType<MapManager>().ShowMap();
            FindObjectOfType<MapManager>().UpdateBtn();
            obliterate.SetActive(true);
        }
    }

    void SpawnShop()
    {
        shop.SetActive(true);
    }

    public void LeaveShop()
    {
        FindObjectOfType<PlayerPoint>().onRoom.OnFinishRoom();
        shop.SetActive(false);
    }

    void SpawnBoss()
    {
        
    }
    

    public void Obliterate()
    {
        FindObjectOfType<CombatManager>().Obliterate();
    }

}
