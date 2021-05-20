using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager = null;
    public Text labelAlly;
    public Text statsAlly;
    public Text labelEnemy;
    public Text statsEnemy;
    public Text turnsText;

    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = this;
        }
        else if (uiManager != this)
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        turnsText = GameObject.Find("TurnsNumber").GetComponent<Text>();
        labelAlly = GameObject.Find("LabelAllySelected").GetComponent<Text>();
        labelEnemy = GameObject.Find("LabelEnemySelected").GetComponent<Text>();
        statsAlly = GameObject.Find("StatsAllySelected").GetComponent<Text>();
        statsEnemy = GameObject.Find("StatsEnemySelected").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeTexts();
    }


    public void ChangeTexts()
    {
        var cm = CombatManager.combatManager;
        if (cm.allySelected)
        {
            labelAlly.text = cm.allySelected.charName;
            statsAlly.text = "Health  " + cm.allySelected.health + "/" + cm.allySelected.maxHealth + "\nArmor  " + cm.allySelected.armor + "\nInitiative   " + cm.allySelected.initiative
                + "\nDodge  " + cm.allySelected.dodge + "\nDamage  " + cm.allySelected.damageRange.x + " - " + cm.allySelected.damageRange.y
                + "\nCritic Chance  " + cm.allySelected.critChance * 100 + "%" + "\nCritic Damage  " + cm.allySelected.critDamage * 100 + "%";
        }
        else
        {
            labelAlly.text = "";
            statsAlly.text = "";
        }
        if (cm.enemySelected)
        {
            labelEnemy.text = cm.enemySelected.charName;
            statsEnemy.text = "Health  " + cm.enemySelected.health + "/" + cm.enemySelected.maxHealth + "\nArmor  " + cm.enemySelected.armor + "\nInitiative   " + cm.enemySelected.initiative
                + "\nDodge  " + cm.enemySelected.dodge + "\nDamage  " + cm.enemySelected.damageRange.x + " - " + cm.enemySelected.damageRange.y
                + "\nCritic Chance  " + cm.enemySelected.critChance * 100 + "%" + "\nCritic Damage  " + cm.enemySelected.critDamage * 100 + "%";
        }
        else
        {
            labelEnemy.text = "";
            statsEnemy.text = "";
        }
    }
}
