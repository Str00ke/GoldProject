using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct DungeonPart
{
    public GameObject[] ennemiesPool;
    public int maxHealthMultiplicator;
    public int damageRangeMultiplicator;
    public int dodgeMultiplicator;
    public int critChanceMultiplicator;
    public int critDamageMultiplicator;
    public int initiativeMultiplicator;
    public int armorMultiplicator;
}

[System.Serializable]
public struct DungeonDifficulty
{
    public DungeonPart part1;
    public DungeonPart part2;
    public DungeonPart part3;
    public Sprite background;
}

[System.Serializable]
public class EnnemyManager : MonoBehaviour
{
    public DungeonDifficulty easyDungeon;
    public DungeonDifficulty moyenDungeon;
    public DungeonDifficulty hardDungeon;
    public static EnnemyManager _enemyManager;

    int easyMin, easyMax, middleMin, middleMax, hardMin, hardMax;

    private void Awake()
    {
        if (_enemyManager != null && _enemyManager != this)
            Destroy(gameObject);

        _enemyManager = this;
    }

    public void SetRoomsDiff(int maxNbr)
    {
        easyMin = 0;
        easyMax = maxNbr / 3;

        middleMin = easyMax + 1;
        middleMax = maxNbr - (maxNbr / 3);

        hardMin = middleMax + 1;
        hardMax = maxNbr;
    }

    public DungeonPart RoomDiffMult(int roomNbr)
    {
        DungeonDifficulty dD = new DungeonDifficulty();
        if (roomNbr >= easyMin && roomNbr <= easyMax)
            return dD.part1;
        else if (roomNbr >= middleMin && roomNbr <= middleMax)
            return dD.part2;
        else if (roomNbr >= hardMin && roomNbr <= hardMax)
            return dD.part3;

        return dD.part1;
    }
}
