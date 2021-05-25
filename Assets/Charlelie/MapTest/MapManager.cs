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
    int rDistFromStart = 0;
    int rTestDone = 0;
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
        startPoint.x -= (level.mapW / 2) * 50;
        startPoint.y += (level.mapH / 2) * 50;
        if (level.mapH == 10)
            startPoint.y -= 25f;

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
                    index++;
                }

                currPoint.x += 50;
            }
            currPoint.x = startPoint.x;
            currPoint.y -= 50;
        }
       
    }

    public void ShowMap()
    {
        mapHolder.SetActive(!mapHolder.activeSelf);
    }

    public void UpdateBtn()
    {
        mapBtn.SetActive(!mapBtn.activeSelf);
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


    public void StartToEnd(MapRoom room)
    {
        rTestDone++;
        rDistFromStart++;
        room.distFromStart = rDistFromStart;
        Debug.Log(rTestDone);
        testArr.Add(room);
        bool result = false;
        for (int i = 0; i < 4; ++i)
        {
            if (room.linkedRoom[i] != null && !testArr.Contains(room.linkedRoom[i]))
            {
                if (room.linkedRoom[i].roomType == RoomType.END)
                {
                    Debug.Log("ARRIVED!!!");
                    result = true;
                }
                StartToEnd(room.linkedRoom[i]);
            }
        }
        rTestDone--;

        if (rTestDone <= 1)
        {
            Debug.Log(result);
        }
    }

    public void RandomizeShop()
    {

        Debug.Log("Setting shop... ");
        foreach (MapRoom room in roomArr)
        {
            Debug.Log(".");
            if (room.roomType != RoomType.START && room.roomType != RoomType.END)
            {
                Debug.Log("..");
                float rand = Random.Range(0, 10);
                if (rand > 5)
                {
                    /*if (CheckIfNothingNear(room))
                    {

                    }*/
                    Debug.Log("...");
                    room.SetToShop();
                    Debug.Log("ShopSet " + room.pos.GetLength(0) + " " + room.pos.GetLength(1));
                    return;
                }

            }
        }
    }


}
