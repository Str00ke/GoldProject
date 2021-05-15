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

    [Header("Enemies")]
    public List<Enemy> enemies;
    public Text labelEnemy;
    public Text statsEnemy;
    public Enemy enemySelected = null;
    public bool enemAttacking = false;
    public float delay = 0.0f;
    public float attackDuration = 1.0f;
    public Text labelAlly;
    public Text statsAlly;

    [Header("FightVariable")]
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
    // Start is called before the first frame update
    void Start()
    {
        turnsText = GameObject.Find("TurnsNumber").GetComponent<Text>();
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
        if(charSelected && enemySelected)
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
    public void CharAttack() 
    {
        if(fightersList[currCharAttacking].charType == Characters.CharType.ENEMY && fightersList[currCharAttacking].CanAttack)
        {
        }
        else if(fightersList[currCharAttacking].charType == Characters.CharType.ALLY && fightersList[currCharAttacking].CanAttack) 
        {
        }
    }
    public void NextCharAttack() 
    {
        fightersList[currCharAttacking].CanAttack = false;
        currCharAttacking++;
        fightersList[currCharAttacking].CanAttack = true;
    }
    public void FightBegins() 
    {
        foreach(Character c in chars) 
        {
            fightersList.Add(c);
        }
        foreach(Enemy e in enemies) 
        {
            fightersList.Add(e);
        }
        //SORT BY INITIATIVE
        fightersList = fightersList.OrderBy(e => e.initiative).ToList();
        fightersList.Reverse();
        fightersList[currCharAttacking].CanAttack = true;
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

    public void AttackEnemy()
    {
        enemySelected.TakeDamageFrom(charSelected);
        charSelected.hasPlayed = true;
        NextCharAttack();
        charSelected.isSelected = false;
        charSelected = null;
    }
    public void EnemiesAttack()
    {
        enemAttacking = true;
        int[] order = new int[enemies.Count];
        int tmpInt;
        for (int i = 0; i < order.Length - 1; i++)
        {
            order[i] = enemies[i].teamPosition;
        }
        for (int j = 0; j < order.Length - 1; j++)
        {
            int rnd = Random.Range(j, order.Length);
            tmpInt = order[rnd];
            order[rnd] = order[j];
            order[j] = tmpInt;
        }
        StartCoroutine(EnemyAttack(order));
        TurnPassed();
    }
    IEnumerator EnemyAttack(int[] order) 
    {   
        int k = 0;
        while (k < order.Length)
        {
            int randAllyAttacked = Random.Range(0, chars.Count);
            while (chars[randAllyAttacked].isDead) 
            {
                randAllyAttacked = Random.Range(0, chars.Count);
            }
            enemies[k].CanAttack = true;
            AttackAlly(enemies[k], chars[randAllyAttacked]);
            yield return new WaitForSeconds(2.0f);
            enemies[k].CanAttack = false;
            ++k;
        }
        turnsText.text = "" + turnNumber;
        yield return null;
    }
    public void AttackAlly(Enemy attacker, Character attacked)
    {
        attacked.TakeDamageFrom(attacker);
        attacker.hasPlayed = true;
    }
    public void TurnPassed() 
    {
        nbCharsPlayed = 0;
        turnNumber++;
        enemAttacking = false;
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
    }
    public void RemoveEnemy(int numPos) 
    {
        foreach(Enemy e in enemies) 
        {
            if(e.teamPosition == numPos)
            {
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
