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
    public Ally allySelected = null;

    [Header("Enemies")]
    public List<Enemy> enemies;
    public Enemy enemySelected = null;
    public float delay = 0.0f;
    public float attackDuration = 2.0f;

    [Header("FightVariable")]
    public Ally allyPlaying;
    public Ally allyCombo = null;
    public int turnNumber = 0;
    public int nbCharsPlayed = 0;
    public int currCharAttacking = 0;
    public int rosterSize;

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
        for (int i = 0; i < 3; ++i)
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
        //CreateRoster();
        
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
        StartCoroutine(NextCharAttackCor());
    }
   IEnumerator NextCharAttackCor()
    {
        fightersList[currCharAttacking].CanAttack = false;
        fightersList[currCharAttacking].hasPlayed = true;
        yield return new WaitForSeconds(3.0f);
            currCharAttacking++;

        if (currCharAttacking >= fightersList.Count)
        {
            TurnPassed();
            UIManager.uiManager.turnsText.text = "" + turnNumber;
            yield return null;
        }
        while (!fightersList[currCharAttacking] || fightersList[currCharAttacking].stunned || fightersList[currCharAttacking].isDead)
        {
            //--------------------STUN STATUS----------------------------
            if (fightersList[currCharAttacking].stunned)
            {
                fightersList[currCharAttacking].stunned = false;
            }
            currCharAttacking++;
            //---------------- ---IF EVERY FIGHTERS HAVE PLAYED -> NEXT TURN----------------------------
            if (currCharAttacking >= fightersList.Count)
            {
                TurnPassed();
                UIManager.uiManager.turnsText.text = "" + turnNumber;
                yield return null;
            }
        }
        if (currCharAttacking < fightersList.Count)
        {
            fightersList[currCharAttacking].CanAttack = true;
            if(fightersList[currCharAttacking].charType == Characters.CharType.ALLY) 
            {
                allyPlaying = (Ally)fightersList[currCharAttacking];
            }
            else
            {
                allyPlaying = null;
            }
            //UPDATE STATUS ON CHARACTER PLAYING
            StatusManager.statusManager.UpdateStatus(fightersList[currCharAttacking]);
            CharAttack(currCharAttacking);
        }
    }
    public void EnemyAttack() 
    {
        StartCoroutine(EnemyAttackCor());
    }
    IEnumerator EnemyAttackCor() 
    {
        Ally inMelee = null;
        Ally inDefence = null;
        int allyAttacked = Random.Range(0, allies.Count);
        while (allies[allyAttacked].isDead) 
        {
            allyAttacked = Random.Range(0, allies.Count);
        }
        foreach (Ally a in allies)
        {
            if (a.isMelee) 
            {
                inMelee = a;
            }
            if (a.inDefenceMode)
            {
                allyAttacked = a.teamPosition;
                inDefence = a;
            }
        }
        yield return new WaitForSeconds(attackDuration);
        if(allies.Count > 0)
            EnemyAbilityAttack(allies[allyAttacked], inDefence);
        yield return new WaitForSeconds(attackDuration);
        NextCharAttack();
        yield return null;
    }
    //---------------Referenced in EnemyAttack()-------------
    public void EnemyAbilityAttack(Ally allyAtt, Ally allyDef)
    {
        if (allies.Count > 0)
        {
            Ability abilityUsed;
            //ENEMY ATTACK ANIMATION
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
                        if (allyDef)
                            fightersList[currCharAttacking].LaunchAttack(allyDef, abilityUsed);
                        else
                            fightersList[currCharAttacking].LaunchAttack(allyAtt, abilityUsed);
                        break;
                    case Ability.WeaponAbilityType.PIERCE:
                        if (allyDef)
                        {
                            fightersList[currCharAttacking].LaunchAttack(allyDef, abilityUsed);
                        }
                        else
                        {
                            allies[0].isMelee = true;
                            fightersList[currCharAttacking].LaunchAttack(allies[0], abilityUsed);
                            if (allies.Count > 1 && allies[1].teamPosition == allies[0].teamPosition + 1)
                            {
                                fightersList[currCharAttacking].LaunchAttack(allies[1], abilityUsed);
                            }
                        }
                        break;
                    case Ability.WeaponAbilityType.WAVE:
                        for (int i = allies.Count - 1; i >= 0; i--)
                        {
                            fightersList[currCharAttacking].LaunchAttack(allies[i], abilityUsed);
                        }
                        break;
                }
            }else if (abilityUsed.objectType == Ability.ObjectType.CRISTAL)
            {
                if(abilityUsed.crType == Ability.CristalAbilityType.ATTACK)
                {
                    if(abilityUsed.crAttackType == Ability.CristalAttackType.DOT)
                    {
                        if (allyDef)
                        {
                            fightersList[currCharAttacking].LaunchAttack(allyDef, abilityUsed);
                            fightersList[currCharAttacking].PutDot(allyDef, abilityUsed);
                        }
                        else
                        {
                            fightersList[currCharAttacking].LaunchAttack(allyAtt, abilityUsed);
                            fightersList[currCharAttacking].PutDot(allyAtt, abilityUsed);
                        }
                    }
                    else if(abilityUsed.crAttackType == Ability.CristalAttackType.NORMAL)
                    {
                        if (allyDef)
                        {
                            fightersList[currCharAttacking].LaunchAttack(allyDef, abilityUsed);
                            allyDef.stunned = true;
                            allyDef.inDefenceMode = false;
                        }
                        else
                        {
                            fightersList[currCharAttacking].LaunchAttack(allyAtt, abilityUsed);
                            allyAtt.stunned = true;
                        }
                    }
                }
                else if(abilityUsed.crType == Ability.CristalAbilityType.HEAL)
                {
                    fightersList[currCharAttacking].LaunchHeal(enemies[Random.Range(0, enemies.Count)], abilityUsed);
                }
            }
        }
    }
    public void TurnPassed()
    {
        currCharAttacking = 0;
        nbCharsPlayed = 0;
        turnNumber++;
        foreach (Ally c in allies)
        {
            if(!c.isDead)
                c.hasPlayed = false;
        }
        foreach (Enemy e in enemies)
        {   
            if (!e.isDead)
                e.hasPlayed = false;
        }

        while (fightersList[currCharAttacking].isDead || !fightersList[currCharAttacking])
        {
            currCharAttacking++;
            if (currCharAttacking >= fightersList.Count)
            {
                TurnPassed();
                UIManager.uiManager.turnsText.text = "" + turnNumber;
                return;
            }
        }

        fightersList[currCharAttacking].CanAttack = true;
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
        if (allies.Count <= 0)
        {
            FindObjectOfType<LevelManager>().losePanel.SetActive(true);
            //EndFight(false);
            //EndFight<Ally>(allies);
        }
    }
    public void RemoveEnemy(int numPos) 
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].teamPosition == numPos)
            {
                if (enemies[i] == enemySelected)
                {
                    enemySelected = null;
                }
                Enemy e = enemies[i];
                enemies.Remove(enemies[i]);
                Destroy(e.gameObject);
                if (enemies.Count <= 0)
                    EndFight<Enemy>();
            }
        }
        UpdatePosEnemies(numPos);

    }
    public void UpdatePosEnemies(int numPos) 
    {
        foreach (Enemy e in enemies)
        {
            if (e.teamPosition < numPos)
            {
            }
            else if (e.teamPosition > numPos)
            {
                e.teamPosition--;
                e.ChangePos();
            }
        }
    }

    /*void EndFight(bool win)
    {
        if (win)
            FindObjectOfType<LevelManager>().WinFight();
        else if (!win)
            FindObjectOfType<LevelManager>().LoseFight();
    }*/
    void EndFight<T>()
    {
        if (typeof(T) == typeof(Enemy))
            FindObjectOfType<LevelManager>().WinFight();
        else if (typeof(T) == typeof(Ally))
            FindObjectOfType<LevelManager>().LoseFight();
    }

    public void Obliterate()
    {
        for (int i = 0; i < enemies.Count; ++i)
        {
            RemoveEnemy(i);
        }
    }
}
