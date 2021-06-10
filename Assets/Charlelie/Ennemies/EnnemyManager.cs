using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EnemiesImgPath
{
    public const string Snake_Spr = "Snake";
    public const string Giraffe_Spr = "Giraffe";
    public const string Death_Spr = "Death";

    public static Sprite GetSprite(string enemy)
    {
        return Resources.Load<Sprite>(enemy);
    }
}


public enum EPart
{
    PART1,
    PART2,
    PART3
}

[System.Serializable]
public struct DungeonPart
{
    [Range(1, 3)]
    public int maxEnemiesInRoom;
    public Ennemy[] ennemiesPool;
    public float maxHealthMultiplicator;
    public float damageRangeMultiplicator;
    public float dodgeMultiplicator;
    public float critChanceMultiplicator;
    public float critDamageMultiplicator;
    public float initiativeMultiplicator;
    public float armorMultiplicator;
}

[System.Serializable]
public struct DungeonDifficulty
{
    public EElement dungeonType;
    public Ennemy firstMiniBoss, secondMiniBoss, boss;
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

    public int easyMin, easyMax, middleMin, middleMax, hardMin, hardMax;


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
        Debug.Log(easyMax + " " + middleMax + " " + hardMax);
    }

    public EPart RoomDiffMult(int roomNbr)
    {
        if (roomNbr >= easyMin && roomNbr <= easyMax)
            return EPart.PART1;
        else if (roomNbr >= middleMin && roomNbr <= middleMax)
            return EPart.PART2;
        else
            return EPart.PART3;

    }

    public Ennemy SetEnemyPool(Level level, MapRoom mapRoom)
    {
        if (mapRoom.distFromStart == easyMax && !LevelManager.GetInstance().fightFMiniBoss)
        {
            LevelManager.GetInstance().firstMiniBossRoom = mapRoom;
            Debug.Log("First mini boss");
            LevelManager.GetInstance().fightFMiniBoss = true;
            switch (level.levelType)
            {
                case LevelType.EASY:
                    return easyDungeon.firstMiniBoss;

                case LevelType.MEDIUM:
                    return moyenDungeon.firstMiniBoss;

                case LevelType.HARD:
                    return hardDungeon.firstMiniBoss;
            }

        } 
        else if (mapRoom.distFromStart == middleMax && !LevelManager.GetInstance().fightSMiniBoss)
        {
            LevelManager.GetInstance().secondMiniBossRoom = mapRoom;
            LevelManager.GetInstance().fightSMiniBoss = true;
            switch (level.levelType)
            {
                case LevelType.EASY:
                    return easyDungeon.secondMiniBoss;

                case LevelType.MEDIUM:
                    return moyenDungeon.secondMiniBoss;

                case LevelType.HARD:
                    return hardDungeon.secondMiniBoss;
            }
        } 
        else if (mapRoom.roomType == RoomType.END)
        {
            switch (level.levelType)
            {
                case LevelType.EASY:
                    return easyDungeon.boss;

                case LevelType.MEDIUM:
                    return moyenDungeon.boss;

                case LevelType.HARD:
                    return hardDungeon.boss;
            }
        }


        Ennemy ennemy = null;
        switch (level.levelType)
        {
            case LevelType.EASY:
                ennemy = GetRandEnnemy(easyDungeon, RoomDiffMult(mapRoom.distFromStart));
                break;

            case LevelType.MEDIUM:
                ennemy = GetRandEnnemy(moyenDungeon, RoomDiffMult(mapRoom.distFromStart));
                break;

            case LevelType.HARD:
                ennemy = GetRandEnnemy(hardDungeon, RoomDiffMult(mapRoom.distFromStart));
                break;
        }
        ennemy.element = SetEnemyElem();
        return ennemy;
    }

    EElement SetEnemyElem()
    {
        int rand = UnityEngine.Random.Range(0, 10);
        if (rand <= 7)
        {
            return LevelManager.GetInstance().level.levelElem;
        }
        else
        {
            Array values = Enum.GetValues(typeof(EElement));
            System.Random random = new System.Random();
            EElement randomElem = (EElement)values.GetValue(random.Next(values.Length));
            return randomElem;
        }
    }

    Ennemy GetRandEnnemy(DungeonDifficulty dD, EPart dP)
    {
        Ennemy ennemy = null;
        switch (dP)
        {
            case EPart.PART1:
                ennemy = dD.part1.ennemiesPool[UnityEngine.Random.Range(0, dD.part1.ennemiesPool.Length)];
                break;

            case EPart.PART2:
                ennemy = dD.part2.ennemiesPool[UnityEngine.Random.Range(0, dD.part2.ennemiesPool.Length)];
                break;

            case EPart.PART3:
                ennemy = dD.part3.ennemiesPool[UnityEngine.Random.Range(0, dD.part3.ennemiesPool.Length)];
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

    public int GetMaxEnemiesInRoom(Level level, MapRoom room)
    {
        if ((room.distFromStart == easyMax && !LevelManager.GetInstance().fightFMiniBoss) || (room.distFromStart == middleMax && !LevelManager.GetInstance().fightSMiniBoss) || room.roomType == RoomType.END) 
        {
            return 1;
        }

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
        EPart eDP = RoomDiffMult(room.distFromStart);
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

        return dP.maxEnemiesInRoom;
    }

    public bool CheckIfOnBossRoom(MapRoom room)
    {
        return ((room.distFromStart == easyMax) || (room.distFromStart == middleMax) || room.roomType == RoomType.END);
    }
}
