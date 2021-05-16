using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MapRoom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject roomSelect;
    RectTransform rT;
    bool isSelected = false;
    float roomWidth; //Same as height
    float selectTime = 0;
    [Range(0.01f, 0.9f)]
    public float selectSpeed;
    Vector2 startPos, currPos;

    //Remplacer GameObject par les types une foit ajoutés
    GameObject[] ennemies;
    GameObject[] chests;
    bool isShop;
    void Start()
    {
        rT = roomSelect.GetComponent<RectTransform>();
        roomWidth = GetComponent<RectTransform>().sizeDelta.x;
    }

    void Update()
    {
        if (isSelected)
        {
            currPos = transform.position;
            Vector2 vec = currPos - startPos;
            if (vec.magnitude > 1)
            {
                isSelected = false;
                rT.sizeDelta = new Vector2(0, 0);
                selectTime = 0;
            }
            rT.sizeDelta = new Vector2(rT.sizeDelta.x + selectSpeed, rT.sizeDelta.y + selectSpeed);
            selectTime += selectSpeed;
            if (selectTime >= roomWidth)
            {
                GoToRoom();
            }
        }

            
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (this != FindObjectOfType<PlayerPoint>().onRoom)
        {
            isSelected = true;
            startPos = transform.position;
        }
            
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isSelected = false;
        rT.sizeDelta = new Vector2(0, 0);
        selectTime = 0;
    }

    void GoToRoom()
    {
        isSelected = false;
        rT.sizeDelta = new Vector2(0, 0);
        selectTime = 0;
        FindObjectOfType<PlayerPoint>().GoToRoom(this);
    }

    public void CreateRoom()
    {

    }
}
