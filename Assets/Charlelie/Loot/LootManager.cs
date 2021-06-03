using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    Vector2 goldPlace = new Vector2(-6, 0);
    Vector2 chestPlace = new Vector2(5.5f, 0);
    //public Vector2 soulPlace = new Vector2(-1, 4);
    public static LootManager lootManager;
    public AnimationCurve animCurveGold;
    public AnimationCurve animCurveSoul;
    public AnimationCurve animCurveSoulFloating;
    public AnimationCurve animCurveItem;
    public AnimationCurve animCurveFalling;
    public List<object> lootOnGround = new List<object>();
    public GameObject chest;
    public int goldValue = 50;

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


    public void SetLoot(Vector2 pos)
    {
        LootItem<GoldPrefab> gold = GetLoot<GoldPrefab>(PlayerPoint._playerPoint.onRoom, pos);
        LootItem<SoulGO> soul = GetLoot<SoulGO>(PlayerPoint._playerPoint.onRoom, pos);
        gold.InstantiateObject(pos);
        soul.InstantiateObject(pos);
        lootOnGround.Add(gold);
        lootOnGround.Add(soul);

        StartCoroutine(ItemFall(gold.instance, (isDone) =>
        {
            if (isDone)
            {
                gold.isAtGround = true;
            }
        }));

        SoulFloat(soul.instance);
        /*StartCoroutine(ItemFall(soul.instance, (isDone) =>
        {
            if (isDone)
            {
                soul.isAtGround = true;
            }
        }));*/
    }

    public void SetLootItem(Vector2 pos)
    {
        Debug.Log(pos);
        LootItem<NItem.ItemScriptableObject> item = GetLoot<NItem.ItemScriptableObject>(PlayerPoint._playerPoint.onRoom, pos);
        item.InstantiateObject(pos);
        lootOnGround.Add(item);
        StartCoroutine(ItemFall(item.instance, (isDone) =>
        {
            if (isDone)
            {
                item.isAtGround = true;
                GiveLootToPlayer();
            }
        }));
    }

    public LootItem<T> GetLoot<T>(MapRoom room, Vector2 pos)
    {
        EPart edP = EnnemyManager._enemyManager.RoomDiffMult(room.distFromStart);
        if (typeof(T) == typeof(NItem.ItemScriptableObject))
        {
            if (EnnemyManager._enemyManager.CheckIfOnBossRoom(room))
            {
                LootItem<T> lootItem = null;
                if (room.distFromStart == EnnemyManager._enemyManager.easyMax) lootItem = new LootItem<T>(GetItemByRarity(itemRarityMiniBoss1), pos);
                else if (room.distFromStart == EnnemyManager._enemyManager.middleMax) lootItem = new LootItem<T>(GetItemByRarity(itemRarityMiniBoss2), pos);
                else if (room.roomType == RoomType.END) lootItem = new LootItem<T>(GetItemByRarity(itemRarityBoss), pos);
                return lootItem;
            }
            
            List<NItem.ItemScriptableObject> tmp = GetItemsOfRarity(edP);
            LootItem<T> item = new LootItem<T>(tmp[Random.Range(0, tmp.Count)], pos);
            return item;
        } else if (typeof(T) == typeof(GoldPrefab))
        {
            LootItem<T> gold = new LootItem<T>(goldPrefab, pos);
            gold.SetAmount(edP);
            return gold;
        } else if (typeof(T) == typeof(SoulGO))
        {
            LootItem<T> soul = new LootItem<T>(soulPrefab, pos);
            soul.SetAmount(edP);
            return soul;
        }
        return null;
    }

    public void SpawnChest()
    {
        Instantiate(chest);
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
        Debug.Log(lootOnGround.Count);
        foreach(object obj in lootOnGround)
        {
            if (obj as LootItem<GoldPrefab> != null)
            {
                StartCoroutine(MoveGoldToPlayer((obj as LootItem<GoldPrefab>).instance, (obj as LootItem<GoldPrefab>)));
            }
            else if (obj as LootItem<SoulGO> != null)
            {
                StartCoroutine(MoveSoulToCounter((obj as LootItem<SoulGO>).instance, (obj as LootItem<SoulGO>)));
            } else if (obj as LootItem<NItem.ItemScriptableObject> != null)
            {
                GiveItem(obj as LootItem<NItem.ItemScriptableObject>);
            }

            if (lootOnGround.Count <= 0) break;
        }
    }

    void GiveItem(LootItem<NItem.ItemScriptableObject> obj)
    {
        lootOnGround.Remove(obj);
        obj.ApplyToPlayer();
        Invoke("InvokeEndFight", 1.0f);
    }

    void InvokeEndFight()
    {
        CombatManager.combatManager.EndFightAfterLoot();
    }

    IEnumerator MoveGoldToPlayer(GameObject go, LootItem<GoldPrefab> obj)
    {
        yield return new WaitForSeconds(2.0f);
        float progress = 0;
        float curve = 0;
        while (curve < 1)
        {
            progress = animCurveGold.Evaluate(curve);
            go.transform.position = Vector2.MoveTowards(go.transform.position, goldPlace, progress);
            curve += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        lootOnGround.Remove(obj);
        obj.ApplyToPlayer();
    }

    void SoulFloat(GameObject go)
    {
        /*float progress = 0;
        Vector2 bound1 = new Vector2(go.transform.position.x, go.transform.position.y - 1);
        Vector2 bound2 = new Vector2(go.transform.position.x, go.transform.position.y + 3);
        float curve = 0;
        while (curve < 1)
        {
            //progress = Mathf.PingPong(Time.time, 1);
            progress = animCurveSoul.Evaluate(curve);
            go.transform.position = Vector2.Lerp(bound1, bound2, progress);
            curve += 0.0025f;
            Debug.Log(progress);
        }*/
        
        
    }

    IEnumerator MoveSoulToCounter(GameObject go, LootItem<SoulGO> obj)
    {
        yield return new WaitForSeconds(2.0f);
        Vector3 startPos = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
        Vector3 endPos = new Vector2(go.transform.position.x, go.transform.position.y + 10);//soulPlace;
        float progress = 0;
        float curve = 0;
        while (curve < 1)
        {
            progress = animCurveSoul.Evaluate(curve);
            go.transform.position = Vector3.Lerp(startPos, endPos, progress);
            curve += 0.0025f;
            yield return null;
        }
        lootOnGround.Remove(obj);
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
                obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + (progress / 10));
            else
                obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y - (progress / 10));
            curve += 0.01f;
            //Debug.Log(progress);
            yield return new WaitForSeconds(0.01f);
        }
        isDone(true);
    }
}
