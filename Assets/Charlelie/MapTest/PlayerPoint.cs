using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoint : MonoBehaviour
{
    public MapRoom startRoom;
    public MapRoom onRoom;
    public static PlayerPoint _playerPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        if (_playerPoint == null)
        {
            _playerPoint = this;
        }
        else if (_playerPoint != this)
            Destroy(gameObject);
    }

    public void Init()
    {
        GoToRoom(startRoom);
        return;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!onRoom)
            GoToRoom(startRoom);*/
    }

    public void GoToRoom(MapRoom room)
    {
        transform.position = room.transform.position;
        onRoom = room;
    }
}
