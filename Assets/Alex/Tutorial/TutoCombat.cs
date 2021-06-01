using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class TutoCombat : MonoBehaviour
{
    [Header("Objects")]
    public GameObject charPrefab;
    public GameObject enemyPrefab;
    public static TutoCombat tutoCombat = null;
    public List<AllyTuto> allies;
    public List<CharactersTuto> fightersList;
    public AllyTuto allySelected = null;

    [Header("Enemies")]
    public List<EnemyTuto> enemies;
    public EnemyTuto enemySelected = null;
    public float delay = 0.0f;
    public float attackDuration = 2.0f;

    [Header("FightVariable")]
    public AllyTuto allyPlaying;
    public int turnNumber = 0;
    public int currCharAttacking = 0;

    Coroutine nextCharCor;

    private void Awake()
    {
        if (tutoCombat == null)
        {
            tutoCombat = this;
        }
        else if (tutoCombat != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartCombat());
    }

    IEnumerator StartCombat()
    {
        yield return new WaitForSeconds(1.5f);
        FightBegins();
    }

    public void FightBegins()
    {
        //SORT BY INITIATIVE
        foreach (AllyTuto c in allies)
        {
            fightersList.Add(c);
        }
        foreach (EnemyTuto e in enemies)
        {
            fightersList.Add(e);
        }
        SortFightersInit();
        fightersList[currCharAttacking].CanAttack = true;
        if (fightersList[currCharAttacking].charType == CharactersTuto.CharType.ALLY)
        {
            allyPlaying = (AllyTuto)fightersList[currCharAttacking];
        }
        else
        {
            allyPlaying = null;
        }
    }
    public void SortFightersInit()
    {
        fightersList = fightersList.OrderBy(e => e.initiative).ToList();
        fightersList.Reverse();
    }
    public void NextCharAttack()
    {
        nextCharCor = StartCoroutine(NextCharAttackCor());
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
        }
        else
        {
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
                }
            }
            if (currCharAttacking < fightersList.Count)
            {
                fightersList[currCharAttacking].CanAttack = true;
                if (fightersList[currCharAttacking].charType == CharactersTuto.CharType.ALLY)
                {
                    allyPlaying = (AllyTuto)fightersList[currCharAttacking];
                    if (allyPlaying.inDefenceMode)
                    {
                        allyPlaying.inDefenceMode = false;
                    }
                }
                else
                {
                    allyPlaying = null;
                }
                //UPDATE STATUS ON CHARACTER PLAYING
                StatusTuto.statusTuto.UpdateStatus(fightersList[currCharAttacking]);
            }
        }
    }
    public void EnemyAttack()
    {
        StartCoroutine(EnemyAttackCor());
    }
    IEnumerator EnemyAttackCor()
    {
        int allyAttacked = Random.Range(0, allies.Count);
        while (allies[allyAttacked].isDead)
        {
            allyAttacked = Random.Range(0, allies.Count);
        }
        NextCharAttack();
        yield return null;
    }
    public void TurnPassed()
    {
        StopCoroutine(nextCharCor);
        nextCharCor = null;
        currCharAttacking = 0;
        turnNumber++;
        UIManager.uiManager.turnsText.text = "" + turnNumber;
        foreach (AllyTuto c in allies)
        {
            if (!c.isDead)
                c.hasPlayed = false;
        }
        foreach (EnemyTuto e in enemies)
        {
            if (!e.isDead)
                e.hasPlayed = false;
        }

        while (fightersList[currCharAttacking].stunned || fightersList[currCharAttacking].isDead || !fightersList[currCharAttacking])
        {
            fightersList[currCharAttacking].stunned = false;
            currCharAttacking++;
            if (currCharAttacking >= fightersList.Count)
            {
                TurnPassed();
                return;
            }
        }

        fightersList[currCharAttacking].CanAttack = true;
        if (fightersList[currCharAttacking].charType == CharactersTuto.CharType.ALLY)
        {
            allyPlaying = (AllyTuto)fightersList[currCharAttacking];
        }
        else
        {
            allyPlaying = null;
        }
    }
    public void RemoveAlly(AllyTuto a)
    {

        allies.Remove(a);
        fightersList.Remove(a);
    }
    void EndFight<T>()
    {
        if (typeof(T) == typeof(Enemy))
            FindObjectOfType<LevelManager>().WinFight();
        else if (typeof(T) == typeof(Ally))
            FindObjectOfType<LevelManager>().LoseFight();
    }

}
