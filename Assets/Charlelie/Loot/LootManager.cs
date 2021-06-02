using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager lootManager;
    public AnimationCurve animCurve;

    [Header("Gold rate")]
    public float goldRatePart1;
    public float goldRatePart2;
    public float goldRatePart3;

    [Header("Gold Prefab")]
    public GameObject goldPrefab;

    [Header("Souls rate")]
    public float soulsRatePart1;
    public float soulsRatePart2;
    public float soulsRatePart3;

    [Header("Soul Prefab")]
    public GameObject soulPrefab;

    [Header("Items list")]
    public List<NItem.ItemScriptableObject> soDropList = new List<NItem.ItemScriptableObject>();

    [Header("Item Prefab")]
    public GameObject itemPrefab;

    [Header("Items rarity per dungeon part")]
    public NItem.ERarity itemRarityPart1;
    public NItem.ERarity itemRarityPart2;
    public NItem.ERarity itemRarityPart3;
    public NItem.ERarity itemRarityMiniBoss1;
    public NItem.ERarity itemRarityMiniBoss2;
    public NItem.ERarity itemRarityBoss;

    private void Awake()
    {

        if (lootManager != null && lootManager != this)
            Destroy(gameObject);

        lootManager = this;   
    }

    public void CreateLoot()
    {
        LootItem<NItem.ItemScriptableObject> go = GetLoot<NItem.ItemScriptableObject>(PlayerPoint._playerPoint.onRoom);
        go.InstantiateObject();
        //go.MoveTowards(new Vector2(100, 100));
        go.ApplyToPlayer();
    }

    public LootItem<T> GetLoot<T>(MapRoom room)
    {
        EPart edP = EnnemyManager._enemyManager.RoomDiffMult(room.distFromStart);
        if (typeof(T) == typeof(NItem.ItemScriptableObject))
        {
            if (EnnemyManager._enemyManager.CheckIfOnBossRoom(room))
            {
                LootItem<T> lootItem = null;
                if (room.distFromStart == EnnemyManager._enemyManager.easyMax) lootItem = new LootItem<T>(GetItemByRarity(itemRarityMiniBoss1));
                else if (room.distFromStart == EnnemyManager._enemyManager.middleMax) lootItem = new LootItem<T>(GetItemByRarity(itemRarityMiniBoss2));
                else if (room.roomType == RoomType.END) lootItem = new LootItem<T>(GetItemByRarity(itemRarityBoss));
                return lootItem;
            }
            
            List<NItem.ItemScriptableObject> tmp = GetItemsOfRarity(edP);
            LootItem<T> item = new LootItem<T>(tmp[Random.Range(0, tmp.Count)]);
            return item;
        } else if (typeof(T) == typeof(GoldPrefab))
        {
            LootItem<T> gold = new LootItem<T>(goldPrefab);
            gold.SetAmount(edP);
            return gold;
        }
        return null;
    }

    List<NItem.ItemScriptableObject> GetItemsOfRarity(EPart edP)
    {
        List<NItem.ItemScriptableObject> tmp = new List<NItem.ItemScriptableObject>();
        NItem.ERarity rarity;
        switch (edP)
        {
            case EPart.PART1:
                rarity = itemRarityPart1;
                break;

            case EPart.PART2:
                rarity = itemRarityPart2;
                break;

            case EPart.PART3:
                rarity = itemRarityPart3;
                break;

            default:
                rarity = itemRarityPart1;
                break;
        }

        foreach (NItem.ItemScriptableObject obj in soDropList)
        {
            if (obj.itemRarity == rarity)
                tmp.Add(obj);
        }

        return tmp;
    }

    NItem.ItemScriptableObject GetItemByRarity(NItem.ERarity rarity)
    {
        List<NItem.ItemScriptableObject> tmp = new List<NItem.ItemScriptableObject>();
        foreach (NItem.ItemScriptableObject obj in soDropList)
        {
            if (obj.itemRarity == rarity)
                tmp.Add(obj);
        }
        return tmp[Random.Range(0, tmp.Count)];
    }

    
}
