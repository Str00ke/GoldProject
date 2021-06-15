using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LevelCreatorManager : MonoBehaviour
{
    Vector2 startPoint = new Vector2(0, 0);
    Vector2 currPoint;
    int mapW, mapH, roomNbr;
    public GameObject roomHolder, room;
    GameObject roomInHand;
    bool isTracingLine = false;
    int[,] firstRoomTracedPos;
    Room firstRoom;
    Room[] roomsList;
    int index = 0;
    bool levelGotStart, levelGotEnd;
    public Button baseRoom, startRoom, endRoom, saveBtn;
    public GameObject levelNamePanel;
    public InputField levelNameInputField;
    bool isOnLevelNamePanel = false;
    

    void Start()
    {       
        baseRoom.gameObject.SetActive(false);
        startRoom.gameObject.SetActive(false);
        endRoom.gameObject.SetActive(false);
        saveBtn.gameObject.SetActive(false);
        levelNamePanel.gameObject.SetActive(false);
        levelNameInputField.onEndEdit.AddListener(SetLevelName);
        roomsList = new Room[roomNbr];
    }

    public void Init(int _mapW, int _mapH, int _roomNbr)
    {
        mapW = _mapW;
        mapH = _mapH;
        roomNbr = _roomNbr;
        baseRoom.gameObject.SetActive(true);
        startRoom.gameObject.SetActive(true);
        endRoom.gameObject.SetActive(true);
        
        SetStartPoint();
        currPoint = startPoint;
        CreateGrid();
    }

    void SetStartPoint()
    {
        startPoint.x -= mapW / 2;
        startPoint.y += mapH / 2;
        if (mapH == 10)
            startPoint.y -= 0.5f;
    }

    void Update()
    {
        if (isOnLevelNamePanel)
            return;

        if (levelGotStart && levelGotEnd && !saveBtn.IsActive())
            saveBtn.gameObject.SetActive(true);

        if (roomInHand)
        {
            MoveRoomInHand();
            if (Input.GetMouseButtonUp(0) && !CheckIfHolderFull())
            {
                if (roomInHand.GetComponent<Room>().isOverHolder)
                    PutRoom();
                else
                    DestroyRoom();
            }
            return;   
        } else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                Collider2D[] checkForRoom = Physics2D.OverlapPointAll(mousePos2D);
                foreach (Collider2D col in checkForRoom)
                {
                    if (col.gameObject.CompareTag("Room"))
                    {
                        TakeRoom(col.gameObject);
                    }
                }
            } else if (Input.GetMouseButtonUp(1))
            {
                //Debug.Log(isTracingLine);
                if (isTracingLine)
                {
                    CheckForLink();
                }
                else 
                {
                    CreateLine();
                }
            }
             

            if (GetComponent<LineRenderer>() && isTracingLine)
            {
                DrawLine();
            }
        }
            
    }

    void CreateGrid()
    {
        for (int i = 0; i < mapH; ++i)
        {
            for (int k = 0; k < mapW; ++k)
            {
                GameObject roomHolderGO = Instantiate(roomHolder, currPoint, transform.rotation);
                roomHolderGO.GetComponent<RoomHolder>().SetPos(new int[k, i]);
                currPoint.x += 1;
            }
            currPoint.x = startPoint.x;
            currPoint.y -= 1;
        }
    }

    public void CreateRoom(int roomType)
    {

        roomInHand = Instantiate(room, Input.mousePosition, transform.rotation);

        switch (roomType)
        {
            case 0:
                break;

            case 1:
                roomInHand.GetComponent<Room>().isStart = true;
                break;

            case 2:
                roomInHand.GetComponent<Room>().isEnd = true;
                break;
        }
    }

    void DestroyRoom()
    {
        Destroy(roomInHand);
        roomInHand = null;
    }

    void MoveRoomInHand()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        roomInHand.transform.position = mousePos2D;
        roomInHand.GetComponent<Room>().CheckForHolder(mousePos2D);
    }

    public void PutRoom()
    {
        if (roomInHand.GetComponent<Room>().isStart)
        {
            levelGotStart = true;
            startRoom.gameObject.SetActive(false);
        }           
        else if (roomInHand.GetComponent<Room>().isEnd)
        {
            levelGotEnd = true;
            endRoom.gameObject.SetActive(false);
        }
            

        roomInHand.GetComponent<Room>().pos = roomInHand.GetComponent<Room>().roomHolder.GetComponent<RoomHolder>().pos;
        roomInHand.GetComponent<Room>().roomHolder.GetComponent<RoomHolder>().SetContainedRoom(roomInHand.GetComponent<Room>());
        roomInHand.GetComponent<Room>().isPutDowm = true;
        roomInHand.AddComponent<BoxCollider2D>();
        roomInHand = null;
    }

    void TakeRoom(GameObject room)
    {
        roomInHand = room;
        room.GetComponent<Room>().isPutDowm = false;
    }

    void CreateLine()
    {
        //Debug.Log("Create");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        Collider2D[] checkForRoom = Physics2D.OverlapPointAll(mousePos2D);
        foreach (Collider2D col in checkForRoom)
        {
            if (col.gameObject.CompareTag("Room"))
            {
                //DrawLine(col.gameObject);
                LineRenderer lr = gameObject.AddComponent<LineRenderer>();
                isTracingLine = true;
                //lr.SetColors(Color.green, Color.yellow);
                lr.startWidth = 0.1f;
                lr.endWidth = 0.1f;
                lr.SetPosition(0, col.gameObject.transform.position);
                lr.SetPosition(1, mousePos2D);
                firstRoomTracedPos = col.gameObject.GetComponent<Room>().pos;
                firstRoom = col.gameObject.GetComponent<Room>();
            }
        }
    }

    void DrawLine()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        GetComponent<LineRenderer>().SetPosition(1, mousePos2D);
    }

    void CheckForLink()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        GetComponent<LineRenderer>().SetPosition(1, mousePos2D);
        isTracingLine = false;

        Collider2D[] checkForRoom = Physics2D.OverlapPointAll(mousePos2D);
        if (checkForRoom.Length > 0)
        {
            foreach (Collider2D col in checkForRoom)
            {
                if (col.gameObject.CompareTag("Room"))
                {
                    if (CheckIfRoomIsNear(firstRoomTracedPos, col.gameObject.GetComponent<Room>().pos) && !CheckIfAlreadyLinked(firstRoom, col.gameObject.GetComponent<Room>()))
                    {
                        //Debug.Log("Good");
                        firstRoom.AddlinkRoom(col.gameObject.GetComponent<Room>());
                        Destroy(GetComponent<LineRenderer>());
                        return;
                    }
                    else
                    {
                        //Debug.Log("false");
                        Destroy(GetComponent<LineRenderer>());
                        return;
                    }

                }
            }
        } else
        {
            Debug.Log("false");
            Destroy(GetComponent<LineRenderer>());
            return;
        }
        
    }

    bool CheckIfRoomIsNear(int[,] firstPos, int[,] secondPos)
    {
        int x1 = firstPos.GetLength(0);
        int y1 = firstPos.GetLength(1);

        int x2 = secondPos.GetLength(0);
        int y2 = secondPos.GetLength(1);

        if ((x1 == x2 && y1 == y2 + 1) || (x1 == x2 - 1 && y1 == y2) || (x1 == x2 && y1 == y2 - 1) || (x1 == x2 + 1 && y1 == y2))
        {
            return true;
        }
        else
            return false;
    }

    bool CheckIfAlreadyLinked(Room firstRoom, Room secondRoom)
    {
        for (int i = 0; i < 4; ++i)
        {
            if (firstRoom.linkedRooms[i] == secondRoom)
                return true;
        }

        for (int i = 0; i < 4; ++i)
        {
            if (secondRoom.linkedRooms[i] == firstRoom)
                return true;
        }

        return false;
    }

    bool CheckIfHolderFull()
    {
        return roomInHand.GetComponent<Room>().roomHolder.GetComponent<RoomHolder>().GetContainedRoom() != null;
    }

    public void EnterLevelName()
    {
        levelNamePanel.gameObject.SetActive(true);
        isOnLevelNamePanel = true;
    }

    public void SetLevelName(string name)
    {
        SaveLevel(name);
        Debug.Log("ByEnter");
    }

    public void SaveLevel(string name)
    {
        Room[] rooms = FindObjectsOfType<Room>();
        SaveSystem.Save(rooms, mapW, mapH, roomNbr, name);
    }

    
}
