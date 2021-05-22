using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject CharPrefab;
    public static CombatManager combatManager = null;
    public List<Ally> allies;
    public List<Characters> fightersList;
    //public GameObject attackButton;
    public Ally allySelected = null;

    [Header("Enemies")]
    public List<Enemy> enemies;
    public Enemy enemySelected = null;
    public float delay = 0.0f;
    public float attackDuration = 1.0f;

    [Header("FightVariable")]
    public Ally allyPlaying;
    public int turnNumber = 0;
    bool fightBegun;
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


    public void CreateRoster()
    {
        for (int i = 0; i < rosterSize; ++i)
        {
            GameObject temp = Instantiate(CharPrefab);
            temp.GetComponent<Ally>().teamPosition = i;
            temp.GetComponent<Ally>().CreateChar("Character0" + i);
            temp.GetComponent<Ally>().ChangePos();
        }
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
        fightBegun = true;
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
        //Remove preceding fighter 
            fightersList[currCharAttacking].CanAttack = false;
            fightersList[currCharAttacking].hasPlayed = true;
            currCharAttacking++;

        if (currCharAttacking >= fightersList.Count)
        {
            TurnPassed();
            UIManager.uiManager.turnsText.text = "" + turnNumber;
            return;
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
                return;
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
            fightersList[currCharAttacking].TakeDamageDots();
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
        yield return new WaitForSeconds(1.0f);
        if(allies.Count > 0)
            EnemyAbilityAttack(allies[allyAttacked], inMelee, inDefence);
        yield return new WaitForSeconds(attackDuration);
        NextCharAttack();
        yield return null;
    }
    //---------------Referenced in EnemyAttack()-------------
    public void EnemyAbilityAttack(Ally allyAtt, Ally allyMel, Ally allyDef)
    {
        //ENEMY ATTACK ANIMATION
        Ability abilityUsed = fightersList[currCharAttacking].abilities[Random.Range(0, fightersList[currCharAttacking].abilities.Length)];
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
                    if(allies[allyDef.teamPosition+1])
                        fightersList[currCharAttacking].LaunchAttack(allies[allyDef.teamPosition + 1], abilityUsed);
                }
                else
                {
                    if (allyMel)
                    {
                        fightersList[currCharAttacking].LaunchAttack(allyMel, abilityUsed);
                        if (allies[allyMel.teamPosition + 1])
                            fightersList[currCharAttacking].LaunchAttack(allies[allyMel.teamPosition + 1], abilityUsed);
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
    /*void EndFight<T>(List<T> list)
    {
        if (typeof(T) == typeof(Enemy))
            FindObjectOfType<LevelManager>().WinFight();
        else if (typeof(T) == typeof(Ally))
            FindObjectOfType<LevelManager>().LoseFight();
    }*/

    public void Obliterate()
    {
        for (int i = 0; i < enemies.Count; ++i)
        {
            RemoveEnemy(i);
        }
    }
}
