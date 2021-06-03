using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int[,] pos;
    public GameObject roomHolder;
    public bool isPutDowm = false;
    public Room[] linkedRooms = new Room[4];
    public bool isStart, isEnd;
    public bool isOverHolder = false;
    public RoomType roomType = new RoomType();


    private void Start()
    {
        if (isStart)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            roomType = RoomType.START;
        }            
        else if (isEnd)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            roomType = RoomType.END;
        }
            
    }

    public void CheckForHolder(Vector2 mousePos)
    {
        GameObject near = CheckNearestHolder(mousePos);
        if (near)
        {
            isOverHolder = true;
            transform.position = near.transform.position;
            roomHolder = near;
        }
        else
            isOverHolder = false;
    }

    GameObject CheckNearestHolder(Vector2 mousePos)
    {
        float minDist = 9999;
        GameObject nearestHolder = null;
        Collider2D[] cols = Physics2D.OverlapCircleAll(mousePos, 1);
        foreach (Collider2D col in cols)
        {
            float dist = Vector2.Distance(mousePos, col.gameObject.transform.position);
            
            if (col.gameObject.CompareTag("RoomHolder") && dist < minDist)
            {
                minDist = dist;
                nearestHolder = col.gameObject;
            }
        }
        
        return nearestHolder;
    }

    
    private void OnDrawGizmos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(mousePos2D, 1);
    }

    public void AddlinkRoom(Room room)
    {
        //Debug.Log("Check");
        for (int i = 0; i < 4; ++i)
        {
            if (linkedRooms[i] == null)
            {
                linkedRooms[i] = room;
                Debug.Log(pos.GetLength(0) + " " + pos.GetLength(1) + " Link to: " + room.pos.GetLength(0) + " " + room.pos.GetLength(1));
                //Debug.Log(i);
                GameObject lineGO = new GameObject();
                LineRenderer lr = lineGO.AddComponent<LineRenderer>();
                //lr.SetColors(Color.green, Color.yellow);
                lr.startWidth = 0.1f;
                lr.endWidth = 0.1f;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, room.gameObject.transform.position);
                break;
            }
        }

        for (int i = 0; i < 4; ++i)
        {

            if (room.linkedRooms[i] == null)
            {
                room.linkedRooms[i] = this;
                break;
            }
        }

    }

}
