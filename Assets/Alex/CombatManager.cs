using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject CharPrefab;
    public static CombatManager combatManager = null;
    public List<Character> chars;
    public List<Characters> fightersList;
    public GameObject attackButton;
    public Character charSelected = null;
    public Text turnsText;
    public GameObject attackerCursor;

    [Header("Enemies")]
    public List<Enemy> enemies;
    public Text labelEnemy;
    public Text statsEnemy;
    public Enemy enemySelected = null;
    public float delay = 0.0f;
    public float attackDuration = 1.0f;
    public Text labelAlly;
    public Text statsAlly;

    [Header("FightVariable")]
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
    // Start is called before the first frame update
    void Start()
    {
        turnsText = GameObject.Find("TurnsNumber").GetComponent<Text>();
        attackerCursor = GameObject.Find("AttackerCursor");
        attackButton = GameObject.Find("AttackButton");
        attackButton.SetActive(false);
        labelAlly = GameObject.Find("LabelAllySelected").GetComponent<Text>();
        labelEnemy = GameObject.Find("LabelEnemySelected").GetComponent<Text>();
        statsAlly = GameObject.Find("StatsAllySelected").GetComponent<Text>();
        statsEnemy = GameObject.Find("StatsEnemySelected").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fightBegun && fightersList[currCharAttacking].charType == Characters.CharType.ALLY && enemySelected)
        {
            attackButton.SetActive(true);
        }
        else
        {
            attackButton.SetActive(false);
        }
        ChangeTexts();
    }


    public void CreateRoster()
    {
        for (int i = 0; i < rosterSize; ++i)
        {
            GameObject temp = Instantiate(CharPrefab);
            temp.GetComponent<Character>().teamPosition = i;
            temp.GetComponent<Character>().CreateChar("Character0" + i);
            temp.GetComponent<Character>().ChangePos();
        }
    }

    public void FightBegins()
    {
        foreach (Character c in chars)
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
        MoveAttackCursor();
        fightBegun = true;
        CharAttack(currCharAttacking);
    }
    public void SortFightersInit()
    {
        fightersList = fightersList.OrderBy(e => e.initiative).ToList();
        fightersList.Reverse();
    }
    public void CharAttack(int ind)
    {
        MoveAttackCursor();
        if (fightersList[ind].charType == Characters.CharType.ENEMY && fightersList[ind].CanAttack)
        {
            EnemyAttack();
        }
    }
    public void NextCharAttack() 
    {
        fightersList[currCharAttacking].CanAttack = false;
        currCharAttacking++;
        if(currCharAttacking < fightersList.Count)
        {
            fightersList[currCharAttacking].CanAttack = true;
            CharAttack(currCharAttacking);
        }
        else 
        {
            TurnPassed();
            turnsText.text = "" + turnNumber;
        }
    }
    public void MoveAttackCursor()
    {
        attackerCursor.transform.position = new Vector3(fightersList[currCharAttacking].gameObject.transform.position.x, fightersList[currCharAttacking].gameObject.transform.position.y + 0.7f, fightersList[currCharAttacking].gameObject.transform.position.z);
    }
    public void ChangeTexts() 
    {
        if (charSelected)
        {
            labelAlly.text = charSelected.charName;
            statsAlly.text = "Health  " + charSelected.health + "\nArmor  " + charSelected.armor + "\nInitiative   " + charSelected.initiative
                + "\nDodge  " + charSelected.dodge + "\nDamage  " + charSelected.damageRange.x + " - " + charSelected.damageRange.y
                + "\nCritic Chance  " + charSelected.critChance* 100 + "%" + "\nCritic Damage  " + charSelected.critDamage * 100 + "%";
        }
        else
        {
            labelAlly.text = "";
            statsAlly.text = "";
        }
        if (enemySelected)
        {
            labelEnemy.text = enemySelected.charName;
            statsEnemy.text = "Health  " + enemySelected.health + "\nArmor  " + enemySelected.armor + "\nInitiative   " + enemySelected.initiative
                + "\nDodge  " + enemySelected.dodge + "\nDamage  " + enemySelected.damageRange.x + " - " + enemySelected.damageRange.y
                + "\nCritic Chance  " + enemySelected.critChance * 100 + "%" + "\nCritic Damage  " + enemySelected.critDamage * 100 + "%";
        }else 
        {
            labelEnemy.text = "";
            statsEnemy.text = "";
        }
    }

    public void AllyAttack()
    {
        enemySelected.TakeDamageFrom(fightersList[currCharAttacking]);
        fightersList[currCharAttacking].hasPlayed = true;
        NextCharAttack();
        fightersList[currCharAttacking].isSelected = false;
        charSelected = null;
    }
    public void EnemyAttack() 
    {
        StartCoroutine(EnemyAttackCor());
    }
    IEnumerator EnemyAttackCor() 
    {   
        int randAllyAttacked = Random.Range(0, chars.Count);
        while (chars[randAllyAttacked].isDead) 
        {
            randAllyAttacked = Random.Range(0, chars.Count);
        }
        yield return new WaitForSeconds(1.0f);
        //ENEMY ATTACK ANIMATION
        chars[randAllyAttacked].TakeDamageFrom(fightersList[currCharAttacking]);
        fightersList[currCharAttacking].hasPlayed = true;
        yield return new WaitForSeconds(2.0f);
        fightersList[currCharAttacking].CanAttack = false;
        NextCharAttack();
        yield return null;
    }
    public void TurnPassed() 
    {
        nbCharsPlayed = 0;
        currCharAttacking = 0;
        turnNumber++;
        foreach (Character c in chars)
        {
            if(!c.isDead)
                c.hasPlayed = false;
        }
        foreach (Enemy e in enemies)
        {
            if (!e.isDead)
                e.hasPlayed = false;
        }
        fightersList[currCharAttacking].CanAttack = true;
        CharAttack(currCharAttacking);
    }
    public void RemoveEnemy(int numPos) 
    {
        foreach(Enemy e in enemies) 
        {
            if(e.teamPosition == numPos)
            {
                if(e == enemySelected) 
                {
                    enemySelected = null;
                }
                enemies.Remove(e);
                Destroy(e.gameObject);
                break;
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
                Debug.Log("Lol");
            }
            else if (e.teamPosition > numPos)
            {
                Debug.Log(e.teamPosition);
                e.teamPosition--;
                e.ChangePos();
            }
        }
    }

}
