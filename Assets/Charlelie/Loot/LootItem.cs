using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootItem<T>
{
    int amount;
    Object obj;
    GameObject instance;

    public LootItem(Object _obj)
    {
        obj = _obj;
        amount = 0;
    }

    public void SetAmount(EPart ePart)
    {
        if (typeof(T) == typeof(GoldPrefab))
        {
            switch (ePart)
            {
                case EPart.PART1:
                    amount = (int)(1 * LootManager.lootManager.goldRatePart1);
                    break;

                case EPart.PART2:
                    amount = (int)(1 * LootManager.lootManager.goldRatePart2);
                    break;

                case EPart.PART3:
                    amount = (int)(1 * LootManager.lootManager.goldRatePart3);
                    break;

                default:
                    amount = 1;
                    break;
            }
        } else if (typeof(T) == typeof(SoulGO))
        {
            switch (ePart)
            {
                case EPart.PART1:
                    amount = (int)(1 * LootManager.lootManager.soulsRatePart1);
                    break;

                case EPart.PART2:
                    amount = (int)(1 * LootManager.lootManager.soulsRatePart2);
                    break;

                case EPart.PART3:
                    amount = (int)(1 * LootManager.lootManager.soulsRatePart3);
                    break;

                default:
                    amount = 1;
                    break;
            }
        }
        
    }

    public void InstantiateObject()
    {
        Debug.Log(amount);      
        if (typeof(T) == typeof(NItem.ItemScriptableObject))
        {
            var itemSO = obj as NItem.ItemScriptableObject;
            Sprite[] spr = itemSO.itemSprites;
            GameObject go = MonoBehaviour.Instantiate(LootManager.lootManager.itemPrefab);
            go.GetComponent<SpriteRenderer>().sprite = itemSO.itemUiSprite;
            go.GetComponent<SOLoot>().so = itemSO;
        }
        else
          instance = (GameObject)MonoBehaviour.Instantiate(obj);
    }

    public void MoveTowards(Vector2 vec)
    {
        Debug.Log("Start moving");
        //MonoBehaviour.StartCoroutine(MoveC(new Vector2(0, 0), new Vector2(100, 100), 5.0f);
    }

    public void ApplyToPlayer()
    {
        float timer = 2500;
        /*while(timer > 0)
        {
            timer -= Time.deltaTime;
            Debug.Log(timer);
        }*/

        if (typeof(T) == typeof(GoldPrefab)) LevelData.AddGold(amount);
        else if (typeof(T) == typeof(SoulGO)) LevelData.AddSouls(amount);
        else if (typeof(T) == typeof(NItem.ItemScriptableObject)) LevelData.AddSoToList(obj as NItem.ItemScriptableObject);
        MonoBehaviour.Destroy(instance, 10.0f);
    }   

    
}
