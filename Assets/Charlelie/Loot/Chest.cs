using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : MonoBehaviour, IPointerClickHandler
{
    public Sprite chestOpen;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        LootManager.lootManager.SetLootItem(transform.position);
        AudioManager.audioManager.Play("ChestOpen");
        gameObject.GetComponent<SpriteRenderer>().sprite = chestOpen;
        //Destroy(gameObject, 1.5f);
    }

}
