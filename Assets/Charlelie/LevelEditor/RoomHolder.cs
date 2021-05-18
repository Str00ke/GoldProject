using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHolder : MonoBehaviour
{
    Room roomContained = null;
    public int[,] pos;


    public void SetContainedRoom(Room room)
    {
        roomContained = room;
    }

    public Room GetContainedRoom()
    {
        return roomContained;
    }

    public void SetPos(int[,] _pos)
    {
        pos = _pos;
    }
}
