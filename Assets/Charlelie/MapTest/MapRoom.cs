using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MapRoom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int[,] pos;
    public GameObject roomSelect;
    RectTransform rT;
    bool isSelected = false;
    float roomWidth; //Same as height
    float selectTime = 0;
    [Range(0.01f, 0.9f)]
    public float selectSpeed;
    Vector2 startPos, currPos;
    public MapRoom[] linkedRoom = new MapRoom[4];
    public RoomType roomType = new RoomType();
    public bool isDiscovered = false;
    public bool isFinished = false;
    public int distFromStart = 0;
    public Text textNbr;
    public int roomNbr;
    public Sprite[] hallwayImgs;

    //Remplacer GameObject par les types une foit ajoutés
    GameObject[] ennemies;
    GameObject[] chests;
    bool isShop;

    public void SetNbr()
    {
        textNbr.text = roomNbr.ToString();
    }

    public void Init()
    {
        for (int i = 0; i < 4; ++i)
            linkedRoom[i] = null;

        rT = roomSelect.GetComponent<RectTransform>();
        roomWidth = GetComponent<RectTransform>().sizeDelta.x;
        if (roomType == RoomType.START)
        {
            GetComponent<Image>().color = Color.green;
            if (FindObjectOfType<PlayerPoint>())
                FindObjectOfType<PlayerPoint>().startRoom = this;
            isDiscovered = true;
        }
        else if (roomType == RoomType.END)
        {
            GetComponent<Image>().color = Color.red;
        }

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
                FindObjectOfType<LevelManager>().StartRoom();
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

    public void OnFinishRoom()
    {
        if (isFinished)
            return;

        isFinished = true;
        //Debug.Log("Discovering");
        if (roomType == RoomType.END)
        {
            Debug.Log("Level finished!");
        }
        for (int i = 0; i < 4; ++i)
        {
            if (linkedRoom[i] != null)
            {
                if (!linkedRoom[i].isDiscovered)
                {
                    linkedRoom[i].gameObject.SetActive(true);
                    linkedRoom[i].isDiscovered = true;


                    GameObject go = new GameObject();
                    go.transform.parent = this.transform;
                    Image img = go.AddComponent<Image>();
                    img.sprite = hallwayImgs[Random.Range(0, hallwayImgs.Length)];
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(0.64f, 0.64f);
                    if (linkedRoom[i].pos.GetLength(0) == pos.GetLength(0))
                    {
                        if (linkedRoom[i].pos.GetLength(1) == pos.GetLength(1) + 1)
                        {
                            go.GetComponent<RectTransform>().localPosition = new Vector2(0, -25);
                        }
                        else
                        {
                            go.GetComponent<RectTransform>().localPosition = new Vector2(0, 25);
                        }
                    }
                    else if (linkedRoom[i].pos.GetLength(1) == pos.GetLength(1))
                    {
                        if (linkedRoom[i].pos.GetLength(0) == pos.GetLength(0) + 1)
                        {
                            go.GetComponent<RectTransform>().localPosition = new Vector2(25, 0);
                            go.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
                        }
                        else
                        {
                            go.GetComponent<RectTransform>().localPosition = new Vector2(-25, 0);
                            go.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90);
                        }
                    }
                }

                
                
                /*LineRenderer lr = go.AddComponent<LineRenderer>();
                Material mat = new Material(Shader.Find("Unlit/Texture"));
                lr.material = mat;
                lr.startColor = Color.gray;
                lr.endColor = Color.gray;
                lr.startWidth = 0.3f;
                lr.endWidth = 0.3f;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, linkedRoom[i].gameObject.transform.position);*/
            }
        }
    }

    public void LinkRoom(MapRoom other)
    {
        for (int i = 0; i < 4; ++i)
        {
            if (linkedRoom[i] == null)
            {
                linkedRoom[i] = other;
                return;
            }
        }
    }

    public void SetToShop()
    {
        GetComponent<Image>().color = Color.blue;
        roomType = RoomType.SHOP;
    }
}
