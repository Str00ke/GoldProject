using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    float tmpMouseZoom = 0;
    Vector2 startPos;
    Vector2 direction;
    Vector2 currPos;
    public MapRoom[,] roomsList;
    MapRoom[] roomArr;
    public GameObject mapRoom;
    public GameObject map, mapHolder, mapBtn;
    public MapRoom onRoom;
    PlayerPoint playerPoint;
    public Level level;
    List<MapRoom> testArr = new List<MapRoom>();
    public int testMax = 0;
    int rDistFromStart = 0;
    int rTestDone = 0;


    static MapManager _mapManager;


    void Awake()
    {
        if (_mapManager != null && _mapManager != this)
            Destroy(gameObject);

        _mapManager = this;
    }

    public static MapManager GetInstance()
    {
        return _mapManager;
    }

    public void Init()
    {
        //roomsList = new MapRoom[FindObjectsOfType<MapRoom>().Length];
        playerPoint = FindObjectOfType<PlayerPoint>();
        roomsList = new MapRoom[level.mapW, level.mapH];
        roomArr = new MapRoom[level.mapW * level.mapH];   
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.touchCount);
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    currPos = touch.position;
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    Vector2 vec = (touch.position + touch.deltaPosition) - touch.position;
                    mapHolder.transform.position = Vector2.MoveTowards(mapHolder.transform.position, vec + (Vector2)mapHolder.transform.position, (vec.magnitude * Time.deltaTime) * 1);
                    currPos = vec;
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    currPos = Vector2.zero;
                    break;
            }
            
        }
        
        else if (Input.touchCount == 2)
        {
            Vector2 vec = Input.GetTouch(1).position - Input.GetTouch(0).position;
            
            if (tmpMouseZoom == 0)
            {
                tmpMouseZoom = vec.magnitude;
            } else if (mapHolder.transform.localScale.x > 5f)
            {
                if (vec.magnitude < tmpMouseZoom - 1 && mapHolder.transform.localScale.x - 1.5f > 5f)
                {
                    mapHolder.transform.localScale = new Vector3(mapHolder.transform.localScale.x - 1.5f, mapHolder.transform.localScale.y - 1.5f, 1);
                    tmpMouseZoom = vec.magnitude;
                }                   
                else if (vec.magnitude > tmpMouseZoom + 1 && mapHolder.transform.localScale.x + 1.5f < 70f)
                {
                    mapHolder.transform.localScale = new Vector3(mapHolder.transform.localScale.x + 1.5f, mapHolder.transform.localScale.y + 1.5f, 1);
                    tmpMouseZoom = vec.magnitude;
                }
                    
            }
            else
            {
                tmpMouseZoom = 0;
            }
        }   
    }   


    public void GenerateMap()
    {
        Vector2 startPoint = SetStartPoint();
        CreateGrid(startPoint);
    }

    Vector2 SetStartPoint()
    {
        Vector2 startPoint = Vector2.zero;
        startPoint.x -= (level.mapW / 2) * 125;
        startPoint.y += (level.mapH / 2) * 90;
        /*if (level.mapH == 10)
            startPoint.y -= 25f;
        */
        return startPoint;
    }

    void CreateGrid(Vector2 startPoint)
    {
        int index = 0;
        Vector2 currPoint = startPoint;
        for (int i = 0; i < level.mapH; ++i)
        {
            for (int k = 0; k < level.mapW; ++k)
            {
                if (level.rooms[k, i] != null)
                {
                    GameObject room = Instantiate(mapRoom, currPoint, transform.rotation);
                    //Debug.Log(index);
                    roomArr[index] = room.GetComponent<MapRoom>();
                    room.transform.SetParent(map.transform, false);
                    room.GetComponent<MapRoom>().roomType = level.rooms[k, i].roomType;
                    roomsList[k, i] = room.GetComponent<MapRoom>();
                    room.GetComponent<MapRoom>().pos = new int[k, i];
                    room.GetComponent<MapRoom>().Init();
                    if (room.GetComponent<MapRoom>().roomType != RoomType.START)
                        room.SetActive(false);
                    /*else
                        room.GetComponent<MapRoom>().OnFinishRoom();*/
                    index++;
                }

                currPoint.x += 150;
            }
            currPoint.x = startPoint.x;
            currPoint.y -= 100;
        }
       
    }

    public void ShowMap()
    {
        mapHolder.SetActive(!mapHolder.activeSelf);
    }

    public void UpdateBtn()
    {
        //mapBtn.SetActive(!mapBtn.activeSelf);
    }

    public void InitPlayerPoint()
    {
        playerPoint.Init();
        return;
    }

    public void MapLinkRooms()
    {
        for (int i = 0; i < level.mapH; ++i)
        {
            for (int k = 0; k < level.mapW; ++k)
            {
                if (level.rooms[k, i] != null)
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        if (level.rooms[k, i].linkedRoom[j] != null)
                        {
                            MapRoom mapRoom = GetRoomAtIndex(level.rooms[k, i].linkedRoom[j].pos.GetLength(0), level.rooms[k, i].linkedRoom[j].pos.GetLength(1));
                            roomsList[k, i].LinkRoom(mapRoom);
                        }
                    }
                }
                
            }
        }
    }

    MapRoom GetRoomAtIndex(int x, int y)
    {
        return roomsList[x, y];
    }

    public void OnFinishRoom()
    {
        FindObjectOfType<PlayerPoint>().onRoom.OnFinishRoom();
    }

    public IEnumerator Test()
    {
        yield return StartCoroutine(StartTO(PlayerPoint._playerPoint.startRoom, 0, (isDone) => {
            LevelManager.GetInstance().Continue();
        }));
    }

    public IEnumerator StartTO(MapRoom room, int index, System.Action<bool> isDone)
    {
        room.SetNbr(index);
        testArr.Add(room);
        for (int i = 0; i < 4; ++i)
        {
            
            if (room.linkedRoom[i] != null && !testArr.Contains(room.linkedRoom[i]))
            {
                if (room.linkedRoom[i].roomType == RoomType.END)
                {
                    testArr.Add(room.linkedRoom[i]);
                    /*room.linkedRoom[i].roomNbr = index;
                    testMax = room.roomNbr;*/
                    room.linkedRoom[i].SetNbr(index + 1);
                    testMax = index + 1;
                    yield return new WaitForSeconds(0.01f);
                    isDone(true);
                    //StartCoroutine(StartTO(room.linkedRoom[i], index + 1, isDone));
                } else
                    room.linkedRoom[i].SetNbr(index + 1);
            }
        }
        index++;
        for (int i = 0; i < 4; ++i)
        {
            if (room.linkedRoom[i] != null && !testArr.Contains(room.linkedRoom[i]))
            {
                
                if (testMax == 0)
                {
                    yield return new WaitForSeconds(0.01f);
                    StartCoroutine(StartTO(room.linkedRoom[i], index, isDone));
                }   
                else if (index + 1 >= testMax && room.linkedRoom[i].roomType != RoomType.END)
                {
                    yield return new WaitForSeconds(0.01f);
                    StartCoroutine(StartTO(room.linkedRoom[i], index, isDone));
                }
            }
        }
    }

    public void StartToEnd(MapRoom room, int index)
    {
        //Debug.Log("Index: " + index);
        //room.roomNbr = index;
        //Debug.Log("Room nbr: " + room.roomNbr);
        room.SetNbr(index);
        
        rTestDone++;
        rDistFromStart++;
        //index++;
        //room.distFromStart = rDistFromStart;
        //Debug.Log(rTestDone);
        testArr.Add(room);
        bool result = false;
        for (int i = 0; i < 4; ++i)
        {
            if (room.linkedRoom[i] != null && !testArr.Contains(room.linkedRoom[i]))
            {
                room.linkedRoom[i].SetNbr(index + 1);
            }
        }
        index++;
        for (int i = 0; i < 4; ++i)
        {
            if (room.linkedRoom[i] != null && !testArr.Contains(room.linkedRoom[i]))
            {
                if(room.linkedRoom[i].roomType == RoomType.END)
                {
                    //Debug.Log("ARRIVED!!!");
                    //Debug.Log("Arrival at: " + index + 1);
                    room.linkedRoom[i].roomNbr = index;
                    testMax = room.roomNbr;
                    StartToEnd(room.linkedRoom[i], index + 1);
                    result = true;
                }
                if (testMax == 0)
                    StartToEnd(room.linkedRoom[i], index);
                else if (index + 1 >= testMax && room.linkedRoom[i].roomType != RoomType.END)
                    StartToEnd(room.linkedRoom[i], index);
            }
        }

        rTestDone--;

        if (rTestDone <= 1)
        {
            //Debug.Log(result);
        }
    }

    public void RandomizeShop()
    {
        List<MapRoom> tmp = new List<MapRoom>();
        foreach (MapRoom room in roomArr)
        {
            if (room != null && room.roomType != RoomType.START && room.roomType != RoomType.END && room.distFromStart != EnnemyManager._enemyManager.easyMax && room.distFromStart != EnnemyManager._enemyManager.middleMax && room.distFromStart > 2)
            {
                tmp.Add(room);
            }
        }
        MapRoom roomChosen = tmp[Random.Range(0, tmp.Count - 1)];
        //roomChosen.SetToShop();
    }
}
