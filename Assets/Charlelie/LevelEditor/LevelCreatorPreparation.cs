using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCreatorPreparation : MonoBehaviour
{
    public GameObject panel;
    public Text mapWTxt, mapHTxt, roomNbrTxt;
    public Slider mapWSlider, mapHSlider, roomNbrSlider;

    LevelCreatorManager lCMana;
    // Start is called before the first frame update
    void Start()
    {
        lCMana = gameObject.GetComponent<LevelCreatorManager>();
        SetMapRoomNbr();
        SetTxt();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void SetTxt()
    {
        mapWTxt.text = mapWSlider.value.ToString();
        mapHTxt.text = mapHSlider.value.ToString();
        roomNbrTxt.text = roomNbrSlider.value.ToString();
    }

    public void SetMapRoomNbr()
    {
        roomNbrSlider.minValue = 10;
        roomNbrSlider.maxValue = mapWSlider.value * mapHSlider.value;
    }

    public void OnValueChange()
    {
        SetMapRoomNbr();
        SetTxt();
    }

    public void StartCreateLvl()
    {
        lCMana.Init((int)mapWSlider.value, (int)mapHSlider.value, (int)roomNbrSlider.value);
        panel.SetActive(false);
    }
}
