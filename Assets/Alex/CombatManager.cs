using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject CharPrefab;
    public static CombatManager combatManager = null;
    public List<Ally> chars;
    public List<Characters> fightersList;
    public GameObject attackButton;
    public Ally charSelected = null;
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

        if (fightBegun && fightersList[currCharAttacking].charType == Characters.CharType.ALLY && enemySelected && AbilitiesManager.abilitiesManager.abilitySelected)
        {
            attackButton.SetActive(true);
        }
        else
        {
            attackButton.SetActive(false);
        }
        ChangeTexts();
    }
    public void DisplayAttackButton() 
    {
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
        foreach (Ally c in chars)
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
        if (currCharAttacking >= fightersList.Count)
        {
            TurnPassed();
            turnsText.text = "" + turnNumber;
            return;
        }
        while (fightersList[currCharAttacking].isDead || !fightersList[currCharAttacking])
        {
            currCharAttacking++;
            if (currCharAttacking >= fightersList.Count)
            {
                TurnPassed();
                turnsText.text = "" + turnNumber;
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
            CharAttack(currCharAttacking);
        }
    }
    public void MoveAttackCursor()
    {
        attackerCursor.transform.position = new Vector3(fightersList[currCharAttacking].gameObject.transform.position.x, fightersList[currCharAttacking].gameObject.transform.position.y + 0.7f, fightersList[currCharAttacking].gameObject.transform.position.z);
        attackerCursor.transform.SetParent(fightersList[currCharAttacking].gameObject.transform);
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
        currCharAttacking = 0;
        nbCharsPlayed = 0;
        turnNumber++;
        foreach (Ally c in chars)
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
    public void RemoveAlly(Ally a) 
    {

        chars.Remove(a);
        if (chars.Count <= 0)
        {
            EndFight<Ally>(chars);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void RemoveEnemy(int numPos) 
    {
        foreach (Enemy e in enemies) 
        {
            if(e.teamPosition == numPos)
            {
                if(e == enemySelected) 
                {
                    enemySelected = null;
                }
                enemies.Remove(e);
                Destroy(e.gameObject);

                if (enemies.Count <= 0)
                    EndFight<Enemy>(enemies);

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

    void EndFight<T>(List<T> list)
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
