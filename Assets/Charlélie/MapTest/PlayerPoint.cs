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
        GoToRoom(startRoom);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToRoom(MapRoom room)
    {
        transform.position = room.transform.position;
        onRoom = room;
    }
}
