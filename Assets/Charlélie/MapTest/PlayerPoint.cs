using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoint : MonoBehaviour
{
    public MapRoom startRoom;
    public MapRoom onRoom;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        GoToRoom(startRoom);
        return;
    }

    // Update is called once per frame
    void Update()
    {
        if (!onRoom)
            GoToRoom(startRoom);
    }

    public void GoToRoom(MapRoom room)
    {
        transform.position = room.transform.position;
        onRoom = room;
    }
}
