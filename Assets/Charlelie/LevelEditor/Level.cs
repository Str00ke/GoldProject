using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RoomType
{
    BASE,
    START,
    END,
    SHOP,
    HEAL
}

public enum LevelType
{
    EASY,
    MEDIUM,
    HARD
}

[System.Serializable]
public class LevelRoom
{
    public int[,] pos;
    public RoomType roomType;
    public LevelRoom[] linkedRoom = new LevelRoom[4];
    bool isDiscovered = false;
    Level level;
    

    public LevelRoom(int[,] _pos, RoomType _roomType, Level _level)
    {
        pos = _pos;
        roomType = _roomType;
        level = _level;
    }


    public void LinkRoom(int[,] otherPos)
    {
        for (int i = 0; i < 4; ++i)
        {
            if (linkedRoom[i] == null)
            {
                LevelRoom room = GetRoomAtPos(otherPos);
                linkedRoom[i] = room;
                return;
            }
        }
    }

    LevelRoom GetRoomAtPos(int[,] pos)
    {
        return level.rooms[pos.GetLength(0), pos.GetLength(1)];
    }
}

[System.Serializable]
public class Level
{
    public int mapW, mapH, roomNbr;
    public string name;
    public LevelRoom[,] rooms;
    public LevelRoom startRoom, endRoom;
    public LevelType levelType;
    public EElement levelElem;
    public Level(int _mapW, int _mapH, int _roomNbr, string _name, LevelType _levelType)
    {
        mapW = _mapW;
        mapH = _mapH;
        roomNbr = _roomNbr;
        name = _name;
        levelType = _levelType;
    }

    public void SetRoomArray()
    {
        rooms = new LevelRoom[mapW,mapH];
        for (int i = 0; i < mapH; ++i)
        {
            for (int k = 0; k < mapW; ++k)
            {
                rooms[k, i] = null;
            }
        }
    }

    public void AddRoomToArray(int[,] pos, RoomType roomType)
    {
        rooms[pos.GetLength(0), pos.GetLength(1)] = new LevelRoom(pos, roomType, this);
        if (roomType == RoomType.START)
            startRoom = rooms[pos.GetLength(0), pos.GetLength(1)];
        else if (roomType == RoomType.END)
            endRoom = rooms[pos.GetLength(0), pos.GetLength(1)];
    }
}
