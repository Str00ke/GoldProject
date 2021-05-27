using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPart
{
    PART1,
    PART2,
    PART3
}

[System.Serializable]
public struct DungeonPart
{
    public Ennemy[] ennemiesPool;
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

    public EPart RoomDiffMult(int roomNbr)
    {
        if (roomNbr >= easyMin && roomNbr <= easyMax)
            return EPart.PART1;
        else if (roomNbr >= middleMin && roomNbr <= middleMax)
            return EPart.PART2;
        else if (roomNbr >= hardMin && roomNbr <= hardMax)
            return EPart.PART3;

        return EPart.PART1;
    }

    public Ennemy SetEnemyPool(Level level, MapRoom mapRoom)
    {
        Ennemy ennemy = null;
        switch (level.levelType)
        {
            case LevelType.EASY:
                ennemy = GetRandEnnemy(easyDungeon, RoomDiffMult(/*mapRoom.distFromStart*/0));
                break;

            case LevelType.MEDIUM:
                ennemy = GetRandEnnemy(moyenDungeon, RoomDiffMult(mapRoom.distFromStart));
                break;

            case LevelType.HARD:
                ennemy = GetRandEnnemy(hardDungeon, RoomDiffMult(mapRoom.distFromStart));
                break;
        }
        return ennemy;
    }

    Ennemy GetRandEnnemy(DungeonDifficulty dD, EPart dP)
    {
        Ennemy ennemy = null;
        Debug.Log("GetRandEnnemy");
        switch (dP)
        {
            case EPart.PART1:
                ennemy = dD.part1.ennemiesPool[Random.Range(0, dD.part1.ennemiesPool.Length)];
                break;

            case EPart.PART2:
                ennemy = dD.part2.ennemiesPool[Random.Range(0, dD.part2.ennemiesPool.Length)];
                break;

            case EPart.PART3:
                ennemy = dD.part3.ennemiesPool[Random.Range(0, dD.part3.ennemiesPool.Length)];
                break;
        }

        return ennemy;
    }

    public void MultiplicateByValues(Enemy enemy, Level level, MapRoom mapRoom)
    {
        DungeonDifficulty dD = new DungeonDifficulty();
        switch (level.levelType)
        {
            case LevelType.EASY:
                dD = _enemyManager.easyDungeon;
                break;

            case LevelType.MEDIUM:
                dD = _enemyManager.moyenDungeon;
                break;

            case LevelType.HARD:
                dD = _enemyManager.hardDungeon;
                break;
        }
        EPart eDP = RoomDiffMult(mapRoom.distFromStart);
        DungeonPart dP = new DungeonPart();
        switch (eDP)
        {
            case EPart.PART1:
                dP = dD.part1;
                break;

            case EPart.PART2:
                dP = dD.part2;
                break;

            case EPart.PART3:
                dP = dD.part3;
                break;
        }

        enemy.maxHealth *= dP.maxHealthMultiplicator;
        enemy.damageRange *= dP.damageRangeMultiplicator;
        enemy.dodge *= dP.dodgeMultiplicator;
        enemy.critChance *= dP.critChanceMultiplicator;
        enemy.critDamage *= dP.critDamageMultiplicator;
        enemy.initiative *= dP.initiativeMultiplicator;
        enemy.armor *= dP.armorMultiplicator;
    }
}
