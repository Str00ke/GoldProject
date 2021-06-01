using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager = null;
    public GameObject statsUI;
    public Text labelChar;
    public Text statsCharRight;
    public Text statsCharLeft;
    public Text turnsText;
    public Sprite[] stateIcons;



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
        statsUI = GameObject.Find("StatsUI");
        turnsText = GameObject.Find("TurnsNumber").GetComponent<Text>();
        labelChar = GameObject.Find("LabelCharSelected").GetComponent<Text>();
        statsCharRight = GameObject.Find("StatsCharSelectedRight").GetComponent<Text>();
        statsCharLeft = GameObject.Find("StatsCharSelectedLeft").GetComponent<Text>();
        statsUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //ChangeTexts();
        if(CombatManager.combatManager.charSelected)
            UpdateStatsUI(CombatManager.combatManager.charSelected);
    }


    /*public void ChangeTexts()
    {
        var cm = CombatManager.combatManager;
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


    }*/
    public void UpdatePosCursorSelect()
    {

    }
    public void ResetDisplayStatsUI()
    {
        statsUI.SetActive(false);
    }
    public void UpdateStatsUI(Characters c)
    {
        labelChar.text = c.charName;
        statsCharLeft.text = "Health  " + Mathf.Round(c.health) + "/" + Mathf.Round(c.maxHealth) + "    ";
        statsCharLeft.text += c.healthDebuff < 0 ? "<color=red>" + c.healthDebuff + "</color> " : "<color=green>" + c.healthDebuff + "</color> ";
        statsCharLeft.text += "\nArmor  " + c.armor + "    ";
        statsCharLeft.text += c.armorBonus < 0 ? "<color=red>" + c.armorBonus + "</color> " : "<color=green>" + c.armorBonus + "</color> ";
        statsCharLeft.text += "\nDodge  " + Mathf.Round(c.dodge) + "    ";
        statsCharLeft.text += c.dodgeBonus < 0 ? "<color=red>" + c.dodgeBonus + "</color> " : "<color=green>" + c.dodgeBonus + "</color> ";
        statsCharLeft.text += "\nInitiative   " + c.initiative;
        statsCharLeft.text += "\nCurrent Element  " + c.currentElement.ToString();


        statsCharRight.text = c.dotDamage > 0 ? "Dot : <color=red> -" + c.dotDamage + "</color> " : "    ";
        statsCharRight.text += "\nDamage  " + Mathf.Round(c.damageRange.x) + " - " + Mathf.Round(c.damageRange.y)  + "    ";
        statsCharRight.text += c.damageBonus < 0 ? "<color=red>" + c.damageBonus + " </color> " : "<color=green>" + c.damageBonus + "</color> ";
        statsCharRight.text += "\nCritic Chance  " + Mathf.Round(c.critChance * 100) + "%" + "    ";
        statsCharRight.text += c.critChanceBonus < 0 ? "<color=red>" + c.critChanceBonus + " </color> " : "<color=green>" + c.critChanceBonus + "</color> ";
        statsCharRight.text += "\nCritic Damage  " + Mathf.Round(c.critDamage * 100) + "%" + "    ";
        statsCharRight.text += c.critDamageBonus < 0 ? "<color=red>" + c.critDamageBonus + " </color> " : "<color=green>" + c.critDamageBonus + "</color> ";
    }
}
