using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    static LootManager lootManager;

    public float GoldRatePart1, GoldRatePart2, GoldRatePart3;
    public float SoulsRatePart1, SoulsRatePart2, SoulsRatePart3;
    public List<NItem.ItemScriptableObject> soDropList = new List<NItem.ItemScriptableObject>();
    public NItem.ERarity itemRarityPart1, itemRarityPart2, itemRarityPart3, itemRarityMiniBoss1, itemRarityMiniBoss2, itemRarityBoss;

    private void Awake()
    {

        if (lootManager != null && lootManager != this)
            Destroy(gameObject);

        lootManager = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropLoot<T>(MapRoom room)
    {
        if (typeof(T) == typeof(NItem.ItemScriptableObject))
        {
            List<NItem.ItemScriptableObject> tmp = new List<NItem.ItemScriptableObject>();
            foreach(NItem.ItemScriptableObject obj in soDropList)
            {
                //if (obj.itemRarity = )
            }
        }
    }
}
