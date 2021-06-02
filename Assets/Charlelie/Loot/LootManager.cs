using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    Vector2 goldPlace = new Vector2(-6, -2);
    public static LootManager lootManager;
    public AnimationCurve animCurveGold;
    public AnimationCurve animCurveSoul;
    public AnimationCurve animCurveItem;
    public AnimationCurve animCurveFalling;
    List<object> lootOnGround = new List<object>();

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


    public void SetLoot()
    {
        LootItem<GoldPrefab> gold = GetLoot<GoldPrefab>(PlayerPoint._playerPoint.onRoom);
        LootItem<SoulGO> soul = GetLoot<SoulGO>(PlayerPoint._playerPoint.onRoom);
        gold.InstantiateObject();
        soul.InstantiateObject();
        lootOnGround.Add(gold);
        lootOnGround.Add(soul);

        StartCoroutine(ItemFall(gold.instance, (isDone) =>
        {
            if (isDone)
            {
                gold.isAtGround = true;
            }
        }));

        StartCoroutine(ItemFall(soul.instance, (isDone) =>
        {
            if (isDone)
            {
                soul.isAtGround = true;
            }
        }));
    }

    public void SetLootItem()
    {
        LootItem<NItem.ItemScriptableObject> item = GetLoot<NItem.ItemScriptableObject>(PlayerPoint._playerPoint.onRoom);
        item.InstantiateObject();
        lootOnGround.Add(item);
        StartCoroutine(ItemFall(item.instance, (isDone) =>
        {
            if (isDone)
            {
                item.isAtGround = true;
            }
        }));
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
        } else if (typeof(T) == typeof(SoulGO))
        {
            LootItem<T> soul = new LootItem<T>(soulPrefab);
            soul.SetAmount(edP);
            return soul;
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


    public void GiveLootToPlayer()
    {
        foreach(object obj in lootOnGround)
        {
            if (obj as LootItem<GoldPrefab> != null)
            {
                StartCoroutine(MoveGoldToPlayer((obj as LootItem<GoldPrefab>).instance, (obj as LootItem<GoldPrefab>)));
            }
            else if (obj as LootItem<SoulGO> != null)
            {
                StartCoroutine(MoveSoulToCounter((obj as LootItem<SoulGO>).instance, (obj as LootItem<SoulGO>)));
            }
        }
    }

    IEnumerator MoveGoldToPlayer(GameObject go, LootItem<GoldPrefab> obj)
    {
        Vector3 startPos = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
        Vector3 endPos = goldPlace;
        float elapsed = 0.0f;
        float ratio = 0.0f;
        float duration = 5.0f;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            go.transform.position = Vector3.Lerp(startPos, endPos, ratio);
            elapsed += Time.deltaTime;
            duration -= 0.05f;
            yield return null;
        }
        obj.ApplyToPlayer();
    }

    IEnumerator MoveSoulToCounter(GameObject go, LootItem<SoulGO> obj)
    {
        Vector3 startPos = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
        Vector3 endPos = goldPlace;
        float elapsed = 0.0f;
        float ratio = 0.0f;
        float duration = 5.0f;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            go.transform.position = Vector3.Lerp(startPos, endPos, ratio);
            elapsed += Time.deltaTime;
            duration -= 0.05f;
            yield return null;
        }
        obj.ApplyToPlayer();
    }

    IEnumerator ItemFall(GameObject obj, System.Action<bool> isDone)
    {
        float prevValue = 0;
        float progress = 0;
        float curve = 0;
        while (curve < 1)
        {
            progress = animCurveFalling.Evaluate(curve);
            if (Mathf.Abs(progress) > prevValue)
                obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.x + progress);
            else
                obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.x - progress);
            curve += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        isDone(true);
    }
}
