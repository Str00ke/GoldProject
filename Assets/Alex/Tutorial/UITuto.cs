using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITuto : MonoBehaviour
{
    public static UITuto uiTuto = null;
    public GameObject enemyStatsUI;
    public GameObject allyStatsUI;
    public Text labelAlly;
    public Text statsAlly;
    public Text labelEnemy;
    public Text statsEnemy;
    public Text turnsText;
    public Sprite[] stateIcons;



    private void Awake()
    {
        if (uiTuto == null)
        {
            uiTuto = this;
        }
        else if (uiTuto != this)
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyStatsUI = GameObject.Find("EnemyUI");
        allyStatsUI = GameObject.Find("AllyUI");
        turnsText = GameObject.Find("TurnsNumber").GetComponent<Text>();
        labelAlly = GameObject.Find("LabelAllySelected").GetComponent<Text>();
        labelEnemy = GameObject.Find("LabelEnemySelected").GetComponent<Text>();
        statsAlly = GameObject.Find("StatsAllySelected").GetComponent<Text>();
        statsEnemy = GameObject.Find("StatsEnemySelected").GetComponent<Text>();
        enemyStatsUI.SetActive(false);
        allyStatsUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeTexts();
        if (TutoCombat.tutoCombat.enemySelected)
            UpdateEnemyUI(TutoCombat.tutoCombat.enemySelected);
    }


    public void ChangeTexts()
    {
        var cm = TutoCombat.tutoCombat;
        if (cm.allySelected)
        {
            labelAlly.text = cm.allySelected.charName;
            statsAlly.text = "Health  " + cm.allySelected.health + "/" + cm.allySelected.maxHealth + "\nArmor  " + cm.allySelected.armor + "\nInitiative   " + cm.allySelected.initiative
                + "\nDodge  " + cm.allySelected.dodge + "\nDamage  " + cm.allySelected.damageRange.x + " - " + cm.allySelected.damageRange.y
                + "\nCritic Chance  " + Mathf.Round(cm.allySelected.critChance * 100) + "%" + "\nCritic Damage  " + Mathf.Round(cm.allySelected.critDamage * 100) + "%" + "\nCurrent Element  " + cm.allySelected.currentElement.ToString();
        }
        else
        {
            labelAlly.text = "";
            statsAlly.text = "";
        }


    }
    public void UpdateEnemyUI(EnemyTuto e)
    {
        labelEnemy.text = e.charName;
        statsEnemy.text = "Health  " + e.health + "/" + e.maxHealth + "    ";
        statsEnemy.text += e.healthDebuff < 0 ? "<color=red>" + e.healthDebuff + "</color> " : "<color=green>" + e.healthDebuff + "</color> ";
        statsEnemy.text += e.dotDamage > 0 ? "\n<color=red> -" + e.dotDamage + "</color> " : "    ";
        //statsEnemy.text += "\nArmor  " + e.armor + "    ";
        //statsEnemy.text += e.armorBonus < 0 ? "<color=red>" + e.armorBonus + "</color> " : "<color=green>" + e.armorBonus + "</color> ";
        //statsEnemy.text += "\nInitiative   " + e.initiative;
        statsEnemy.text += "\nDodge  " + e.dodge + "    ";
        statsEnemy.text += e.dodgeBonus < 0 ? "<color=red>" + e.dodgeBonus + "</color> " : "<color=green>" + e.dodgeBonus + "</color> ";
        statsEnemy.text += "\nDamage  " + e.damageRange.x + " - " + e.damageRange.y + "    ";
        statsEnemy.text += e.damageBonus < 0 ? "<color=red>" + e.damageBonus + " </color> " : "<color=green>" + e.damageBonus + "</color> ";
        statsEnemy.text += "\nCritic Chance  " + Mathf.Round(e.critChance * 100) + "%" + "    ";
        statsEnemy.text += e.critChanceBonus < 0 ? "<color=red>" + e.critChanceBonus + " </color> " : "<color=green>" + e.critChanceBonus + "</color> ";
        //statsEnemy.text += "\nCritic Damage  " + Mathf.Round(e.critDamage * 100) + "%" + "    ";
        //statsEnemy.text += e.critDamageBonus < 0 ? "<color=red>" + e.critDamageBonus + " </color> " : "<color=green>" + e.critDamageBonus + "</color> ";
        statsEnemy.text += "\nCurrent Element  " + e.currentElement.ToString();
    }
    public void ResetEnemyDisplayUI()
    {
        enemyStatsUI.SetActive(false);
    }
    public void UpdateAllyUI(Ally a)
    {
        labelEnemy.text = a.charName;
        statsEnemy.text = "Health  " + a.health + "/" + a.maxHealth + "    ";
        statsEnemy.text += a.healthDebuff < 0 ? "<color=red> -" + a.healthDebuff + "</color> " : "<color=green> +" + a.healthDebuff + "</color> ";
        statsEnemy.text += a.dotDamage > 0 ? "\n<color=red> -" + a.dotDamage + "</color> " : "    ";
        statsEnemy.text += "\nArmor  " + a.armor + "    ";
        statsEnemy.text += a.armorBonus < 0 ? "<color=red> -" + a.armorBonus + "</color> " : "<color=green> +" + a.armorBonus + "</color> ";
        statsEnemy.text += "\nInitiative   " + a.initiative;
        statsEnemy.text += "\nDodge  " + a.dodge + "    ";
        statsEnemy.text += a.dodgeBonus < 0 ? "<color=red> -" + a.dodgeBonus + "</color> " : "<color=green> +" + a.dodgeBonus + "</color> ";
        statsEnemy.text += "\nDamage  " + a.damageRange.x + " - " + a.damageRange.y + "    ";
        statsEnemy.text += a.damageBonus < 0 ? "<color=red> -" + a.damageBonus + " </color> " : "<color=green> +" + a.damageBonus + "</color> ";
        statsEnemy.text += "\nCritic Chance  " + Mathf.Round(a.critChance * 100) + "%" + "    ";
        statsEnemy.text += a.critChanceBonus < 0 ? "<color=red> -" + a.critChanceBonus + " </color> " : "<color=green> +" + a.critChanceBonus + "</color> ";
        statsEnemy.text += "\nCritic Damage  " + Mathf.Round(a.critDamage * 100) + "%" + "    ";
        statsEnemy.text += a.critDamageBonus < 0 ? "<color=red> -" + a.critDamageBonus + " </color> " : "<color=green> +" + a.critDamageBonus + "</color> ";
        statsEnemy.text += "\nCurrent Element  " + a.currentElement.ToString();
    }
    public void ResetAllyDisplayUI()
    {
        allyStatsUI.SetActive(false);
    }
}
