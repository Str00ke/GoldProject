using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject charPrefab;
    public GameObject enemyPrefab;
    public static CombatManager combatManager = null;
    public List<Ally> allies;
    public List<Characters> fightersList;
    //public GameObject attackButton;
    public Characters charSelected = null;

    [Header("Enemies")]
    public List<Enemy> enemies;
    public float delay = 0.0f;
    public float attackDuration = 2.0f;

    [Header("FightVariable")]
    public Ally allyPlaying;
    public Ally allyCombo = null;
    public int turnNumber = 0;
    public int nbCharsPlayed = 0;
    public int currCharAttacking = 0;
    public int rosterSize;
    public bool hasDied, hasTookDamage, hasHeal = false;

    Coroutine nextCharCor;

    private void Awake()
    {
        if (combatManager == null)
        {
            combatManager = this;
        }
        else if (combatManager != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartCombat());
    }

    public void CreateRoster()
    {
        //GET CHARACTERS
        for (int i = 0; i < 3; ++i)
        {
            Character c = CharacterManager.characterManager.AskForCharacter(i);
            if (!c)
            {
                continue;
            }
            GameObject temp = Instantiate(charPrefab);
            temp.name = "Char" + i;
            temp.GetComponent<Ally>().CreateChar(c,i);
            temp.GetComponent<Ally>().ChangePos();
        }
    }
    public void CreateEnemies()
    {
        Level level = LevelManager.GetInstance().level;
        MapRoom mapRoom = PlayerPoint._playerPoint.onRoom;
        int index = EnnemyManager._enemyManager.GetMaxEnemiesInRoom(level, mapRoom);
        for (int i = 0; i < index; ++i)
        {
            Ennemy e = EnnemyManager._enemyManager.SetEnemyPool(level, mapRoom);
            if (!e)
            {
                continue;
            }
            GameObject go = Instantiate(enemyPrefab);
            go.GetComponent<Enemy>().CreateEnemy(e,i, level, mapRoom);
            go.GetComponent<Enemy>().ChangePos();
        }
    }
    IEnumerator StartCombat()
    {
        CreateEnemies();
        CreateRoster();
        
        yield return new WaitForSeconds(1.5f);
        FightBegins();
    }

    public void FightBegins()
    {
        foreach (Ally c in allies)
        {
            fightersList.Add(c);
        }
        foreach (Enemy e in enemies)
        {
            fightersList.Add(e);
        }
        //SORT BY INITIATIVE
        SortFightersInit();
        fightersList[currCharAttacking].CanAttack = true;
        fightersList[currCharAttacking].GetComponentInChildren<CursorEffectsScript>().ActivateCursor(fightersList[currCharAttacking].cursorPlaying);
        if (fightersList[currCharAttacking].charType == Characters.CharType.ALLY)
        {
            allyPlaying = (Ally)fightersList[currCharAttacking];
        }
        else 
        {
            allyPlaying = null;
        }
        CharAttack(currCharAttacking);
    }
    public void SortFightersInit()
    {
        fightersList = fightersList.OrderBy(e => e.initiative).ToList();
        fightersList.Reverse();
    }
    //---------------ATTACK FOR ENEMY------------------
    public void CharAttack(int ind)
    {
        if (fightersList[ind].charType == Characters.CharType.ENEMY && fightersList[ind].CanAttack)
        {
            EnemyAttack();
        }
    }
    public void NextCharAttack()
    {
        nextCharCor = StartCoroutine(NextCharAttackCor());
    }
   IEnumerator NextCharAttackCor()
    {
        if(fightersList[currCharAttacking] && currCharAttacking >= 0)
        {
            fightersList[currCharAttacking].CanAttack = false;
            fightersList[currCharAttacking].hasPlayed = true;
            fightersList[currCharAttacking].GetComponentInChildren<CursorEffectsScript>().DeactivateCursor(fightersList[currCharAttacking].cursorNotPlayedYet);
        }
        AbilitiesManager.abilitiesManager.ClearTargets();
        yield return new WaitForSeconds(fightersList[currCharAttacking].durationDecreaseHealth * 2);
        if (currCharAttacking >= 0)
        {
            if (fightersList[currCharAttacking].cursorPlaying)
                fightersList[currCharAttacking].GetComponentInChildren<CursorEffectsScript>().DeactivateCursor(fightersList[currCharAttacking].cursorPlaying);
        }
        currCharAttacking++;
        if (currCharAttacking >= fightersList.Count)
        {
            TurnPassed();
        }
        else
        {
            while (fightersList[currCharAttacking] == null || fightersList[currCharAttacking].stunned || fightersList[currCharAttacking].isDead)
            {
                if(fightersList[currCharAttacking] != null && !fightersList[currCharAttacking].isDead)
                {
                    StatusManager.statusManager.UpdateStatus(fightersList[currCharAttacking]);
                    if (fightersList[currCharAttacking].cursorPlaying)
                    {
                        fightersList[currCharAttacking].GetComponentInChildren<CursorEffectsScript>().ActivateCursor(fightersList[currCharAttacking].cursorPlaying);
                    }
                    yield return new WaitForSeconds(1.5f);
                    if (fightersList[currCharAttacking].cursorPlaying)
                    {
                        fightersList[currCharAttacking].GetComponentInChildren<CursorEffectsScript>().DeactivateCursor(fightersList[currCharAttacking].cursorPlaying);
                    }
                }
                currCharAttacking++;
                //---------------- ---IF EVERY FIGHTERS HAVE PLAYED -> NEXT TURN----------------------------
                if (currCharAttacking >= fightersList.Count)
                {
                    TurnPassed();
                }
            }
            if (currCharAttacking < fightersList.Count)
            {
                fightersList[currCharAttacking].CanAttack = true;
                fightersList[currCharAttacking].GetComponentInChildren<CursorEffectsScript>().ActivateCursor(fightersList[currCharAttacking].cursorPlaying);
                if (fightersList[currCharAttacking].charType == Characters.CharType.ALLY)
                {
                    allyPlaying = (Ally)fightersList[currCharAttacking];
                }
                else
                {
                    allyPlaying = null;
                }
                //UPDATE STATUS ON CHARACTER PLAYING
                yield return new WaitForSeconds(0.5f);
                StatusManager.statusManager.UpdateStatus(fightersList[currCharAttacking]);
                yield return new WaitForSeconds(fightersList[currCharAttacking].durationDecreaseHealth*1.5f);
                if (fightersList[currCharAttacking])
                    CharAttack(currCharAttacking);
                else
                    NextCharAttack();
            }
        }
    }
    public void EnemyAttack() 
    {
        StartCoroutine(EnemyAttackCor());
    }
    IEnumerator EnemyAttackCor()
    {
        fightersList[currCharAttacking].GetComponent<SpriteRenderer>().sprite = fightersList[currCharAttacking].enemySprites[1];
        Characters c = fightersList[currCharAttacking];
        Ally inDefence = null;
        int allyAttacked = Random.Range(0, allies.Count);
        foreach (Ally a in allies)
        {
            if (a.inDefenceMode && !a.isDead)
            {
                allyAttacked = allies.IndexOf(a);
                inDefence = a;
                break;
            }
        }
        if(allies.Count > 0)
        {
            List<Characters> targets;
            Ability ab = EnemyAbilityAttack(allies[allyAttacked], out targets);
            Debug.Log(targets + "" + ab);
        }
        yield return new WaitForSeconds(fightersList[currCharAttacking].durationDecreaseHealth + 0.1f);
        if(c)
            c.GetComponent<SpriteRenderer>().sprite = c.enemySprites[0];
        NextCharAttack();
    }
    //---------------Referenced in EnemyAttack()-------------
    public Ability EnemyAbilityAttack(Ally allyAtt, out List<Characters> enemyTargets)
    {
        Ability abilityUsed;
        List<Characters> targets = new List<Characters>();
        if (fightersList[currCharAttacking].abilitiesCristal.Length > 0)
        {
            int rand = Random.Range(0, 100);
            if(rand < 20)
            {
                abilityUsed = fightersList[currCharAttacking].abilities[Random.Range(0, fightersList[currCharAttacking].abilitiesCristal.Length)];
            }
            else
            {
                abilityUsed = fightersList[currCharAttacking].abilities[Random.Range(0, fightersList[currCharAttacking].abilities.Length)];
            }
        }
        else
        {
            abilityUsed = fightersList[currCharAttacking].abilities[Random.Range(0, fightersList[currCharAttacking].abilities.Length)];
        }
        if(abilityUsed.objectType == Ability.ObjectType.WEAPON)
        {
            switch (abilityUsed.weaponAbilityType)
            {
                case Ability.WeaponAbilityType.BASE:
                    fightersList[currCharAttacking].LaunchAttack(allyAtt, abilityUsed);
                    targets.Add(allyAtt);
                    break;
                case Ability.WeaponAbilityType.PIERCE:
                    if (allyAtt.inDefenceMode)
                    {
                        fightersList[currCharAttacking].LaunchAttack(allyAtt, abilityUsed);
                        targets.Add(allyAtt);
                    }
                    else
                    {
                        allies[0].isMelee = true;
                        fightersList[currCharAttacking].LaunchAttack(allies[0], abilityUsed);
                        targets.Add(allies[0]);
                        if (allies.Count > 1 && allies[1].teamPosition == allies[0].teamPosition + 1)
                        {
                            fightersList[currCharAttacking].LaunchAttack(allies[1], abilityUsed);
                            targets.Add(allies[1]);
                        }
                    }
                    break;
                case Ability.WeaponAbilityType.WAVE:
                    for (int i = allies.Count - 1; i >= 0; i--)
                    {
                        fightersList[currCharAttacking].LaunchAttack(allies[i], abilityUsed);
                        targets.Add(allies[i]);
                    }
                    break;
            }
        }else if (abilityUsed.objectType == Ability.ObjectType.CRISTAL)
        {
            if(abilityUsed.crType == Ability.CristalAbilityType.ATTACK)
            {
                if(abilityUsed.crAttackType == Ability.CristalAttackType.DOT)
                {
                    fightersList[currCharAttacking].LaunchAttack(allyAtt, abilityUsed);
                    fightersList[currCharAttacking].PutDot(allyAtt, abilityUsed);
                    targets.Add(allyAtt);
                    
                }
                else if(abilityUsed.crAttackType == Ability.CristalAttackType.NORMAL)
                {
                    if (allyAtt.inDefenceMode)
                    {
                        fightersList[currCharAttacking].LaunchAttack(allyAtt, abilityUsed);
                        Status s = null;
                        targets.Add(allyAtt);
                        foreach (Status s1 in allyAtt.statusList)
                        {
                            if (s1.statusType == Status.StatusTypes.Defence)
                                s1.RevertStatus();
                            else if (s1.statusType == Status.StatusTypes.Stun)
                            {
                                s = s1;
                                s1.turnsActive = 1;
                            }
                        }
                        if(s == null)
                            s = new Status(allyAtt, 0, 1, Status.StatusTypes.Stun);
                    }
                    else
                    {
                        fightersList[currCharAttacking].LaunchAttack(allyAtt, abilityUsed);
                        Status s = null;
                        foreach (Status s1 in allyAtt.statusList)
                        {
                            if (s1.statusType == Status.StatusTypes.Defence)
                                s1.RevertStatus();
                            else if (s1.statusType == Status.StatusTypes.Stun)
                            {
                                s = s1;
                                s1.turnsActive = 1;
                            }
                        }
                        if (s == null)
                            s = new Status(allyAtt, 0, 1, Status.StatusTypes.Stun);
                        targets.Add(allyAtt);
                    }
                }
            }
            else if(abilityUsed.crType == Ability.CristalAbilityType.HEAL)
            {
                Enemy randE = enemies[Random.Range(0, enemies.Count)];
                fightersList[currCharAttacking].LaunchHeal(randE, abilityUsed);
                targets.Add(randE);
            }
        }
        enemyTargets = targets;
        return abilityUsed;
        //ATTACK ANIM

    }
    public void TurnPassed()
    {
        StopCoroutine(nextCharCor);
        nextCharCor = null;
        currCharAttacking = 0;
        nbCharsPlayed = 0;
        turnNumber++;
        UIManager.uiManager.turnsText.text = "" + turnNumber;
        foreach (Ally c in allies)
        {
            if (!c.isDead)
            {
                c.hasPlayed = false;
                //fightersList[currCharAttacking].cursorNotPlayedYet.SetActive(true);
                c.GetComponentInChildren<CursorEffectsScript>().ActivateCursor(c.cursorNotPlayedYet);
            }
        }
        foreach (Enemy e in enemies)
        {   
            if (!e.isDead)
            {
                e.hasPlayed = false;
                //fightersList[currCharAttacking].cursorNotPlayedYet.SetActive(true);
                e.GetComponentInChildren<CursorEffectsScript>().ActivateCursor(e.cursorNotPlayedYet);
            }
        }
        while (fightersList[currCharAttacking].stunned || fightersList[currCharAttacking].isDead || !fightersList[currCharAttacking])
        {
            StatusManager.statusManager.UpdateStatus(fightersList[currCharAttacking]);
            currCharAttacking++;
            if (currCharAttacking >= fightersList.Count)
            {
                TurnPassed();
                return;
            }
        }

        StatusManager.statusManager.UpdateStatus(fightersList[currCharAttacking]);
        fightersList[currCharAttacking].CanAttack = true;
        //fightersList[currCharAttacking].cursorPlaying.SetActive(true);
        fightersList[currCharAttacking].GetComponentInChildren<CursorEffectsScript>().ActivateCursor(fightersList[currCharAttacking].cursorPlaying);
        if (fightersList[currCharAttacking].charType == Characters.CharType.ALLY)
        {
            allyPlaying = (Ally)fightersList[currCharAttacking];
        }
        else
        {
            allyPlaying = null;
        }
        CharAttack(currCharAttacking);
    }
    public void RemoveAlly(Ally a) 
    {

        allies.Remove(a);
        hasDied = true;
        LevelData.AddDeath();
        if (allies.Count <= 0)
        {
            FindObjectOfType<LevelManager>().losePanel.SetActive(true);
            EndFight<Ally>();
        }
    }
    public void RemoveEnemy(Enemy e)
    {
        int numPos = 0;
        LootManager.lootManager.SetLoot(e.transform.position);
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == e)
            {
                if (enemies[i] == charSelected)
                {
                    charSelected = null;
                }
                enemies.Remove(e);
                numPos = e.teamPosition;
                Destroy(e.gameObject);

                if (enemies.Count <= 0)
                    EndFight<Enemy>();
            }
        }
        if(e)
            UpdatePosEnemies(numPos);
    }
    public void UpdatePosEnemies(int numPos) 
    {
        foreach (Enemy e in enemies)
        {
           if (e.teamPosition > numPos)
           {
                e.teamPosition--;
                e.ChangePos();
           }
        }
    }
    void EndFight<T>()
    {
        if (typeof(T) == typeof(Enemy))
        {
            AchievementsManager.OnCombatEnd(this);
            LootManager.lootManager.GiveLootToPlayer();
            if (PlayerPoint._playerPoint.onRoom.distFromStart == EnnemyManager._enemyManager.easyMax)
                LevelManager.GetInstance().isFirstMiniBossDead = true;
            else if (PlayerPoint._playerPoint.onRoom.distFromStart == EnnemyManager._enemyManager.middleMax)
                LevelManager.GetInstance().isSecondMiniBossDead = true;
            //LootManager.lootManager.SpawnChest();
            /*if (LootManager.lootManager.lootOnGround.Count <= 0)
            {
                FindObjectOfType<LevelManager>().WinFight();
                foreach (Ally a in allies)
                {
                    CharacterManager.characterManager.AskForCharacter(a.teamPosition).health = (int)a.health;
                }
            }*/
        }          
        else if (typeof(T) == typeof(Ally))
            FindObjectOfType<LevelManager>().LoseFight();
    }

    public void EndFightAfterLoot()
    {
        FindObjectOfType<LevelManager>().WinFight();
        foreach (Ally a in allies)
        {
            CharacterManager.characterManager.AskForCharacter(a.teamPosition).health = (int)a.health;
            Debug.Log((int)a.health);
        }
    }

    public void Obliterate()
    {
        /*for (int i = 0; i < enemies.Count; ++i)
        {
            RemoveEnemy(enemies[i]);
        }*/
        RemoveEnemy(enemies[0]);
    }
}
