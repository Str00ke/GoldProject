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
    public Text statsCharRightModifs;
    public Text statsCharLeft;
    public Text statsCharLeftModifs;
    public Text turnsText;
    public Sprite[] stateIcons;
    public GameObject statusPanel;
    public GameObject posInitialStatusPanel;
    public Text statusPanelLabelChar;
    public GameObject buttonStatus;
    public List<GameObject> goStatus;
    public float offsetStatus;

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
        statusPanel = GameObject.Find("StatusPanel");
        posInitialStatusPanel = GameObject.Find("PosInitialStatus");
        buttonStatus = GameObject.Find("ButtonStatus");
        statusPanelLabelChar = GameObject.Find("StatusPanelLabelChar").GetComponent<Text>();
        statsUI = GameObject.Find("StatsUI");
        turnsText = GameObject.Find("TurnsNumber").GetComponent<Text>();
        labelChar = GameObject.Find("LabelCharSelected").GetComponent<Text>();
        statsCharRight = GameObject.Find("StatsCharSelectedRight").GetComponent<Text>();
        statsCharLeft = GameObject.Find("StatsCharSelectedLeft").GetComponent<Text>();
        statsCharRightModifs = GameObject.Find("StatsCharSelectedRightModifs").GetComponent<Text>();
        statsCharLeftModifs = GameObject.Find("StatsCharSelectedLeftModifs").GetComponent<Text>();
        statsUI.SetActive(false);
        buttonStatus.SetActive(false);
        statusPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //ChangeTexts();
        if(CombatManager.combatManager.charSelected)
            UpdateStatsUI(CombatManager.combatManager.charSelected);
    }
    public void ResetDisplayStatsUI()
    {
        statsUI.SetActive(false);
    }
    public void UpdateStatsUI(Characters c)
    {
        labelChar.text = c.charName;
        statsCharLeft.text = "Health" + Mathf.Round(c.health) + " / " + Mathf.Round(c.maxHealth);
        statsCharLeftModifs.text = c.healthDebuff < 0 ? "<color=red>" + c.healthDebuff + "</color> " : "<color=green>+" + c.healthDebuff + "</color> ";
        statsCharLeft.text += "\nArmor  " + c.armor + "    ";
        statsCharLeftModifs.text += c.armorBonus < 0 ? "\n<color=red>" + c.armorBonus + "</color> " : "\n<color=green>+" + c.armorBonus + "</color> ";
        statsCharLeft.text += "\nDodge  " + Mathf.Round(c.dodge) + "    ";
        statsCharLeftModifs.text += c.dodgeBonus < 0 ? "\n<color=red>" + c.dodgeBonus + "</color> " : "\n<color=green>+" + c.dodgeBonus + "</color> ";
        statsCharLeft.text += "\nInitiative  " + c.initiative;
        statsCharLeft.text += "\nCurrent Element  " + c.currentElement.ToString();

        statsCharRight.text = "Dot ";
        statsCharRightModifs.text = c.dotDamage > 0 ? "<color=red>-" + c.dotDamage + "</color> " : "    ";
        statsCharRight.text += "\nDamage  " + Mathf.Round(c.damageRange.x) + " - " + Mathf.Round(c.damageRange.y);
        statsCharRightModifs.text += c.damageBonus < 0 ? "\n<color=red>" + c.damageBonus + "</color> " : "\n<color=green>+" + c.damageBonus + "</color> ";
        statsCharRight.text += "\nCritic Chance  " + Mathf.Round(c.critChance * 100) + "%";
        statsCharRightModifs.text += c.critChanceBonus < 0 ? "\n<color=red>" + c.critChanceBonus + "</color> " : "\n<color=green>+" + c.critChanceBonus + "</color> ";
        statsCharRight.text += "\nCritic Damage" + Mathf.Round(c.critDamage * 100) + "%";
        statsCharRightModifs.text += c.critDamageBonus < 0 ? "\n<color=red>" + c.critDamageBonus + "</color> " : "\n<color=green>+" + c.critDamageBonus + "</color> ";
    }

    public void DisplayStatusPanel()
    {
        UpdateStatusPanel();
        statusPanel.SetActive(true);
        buttonStatus.SetActive(false);
    }
    public void HideStatusPanel()
    {
        DestroyStatusPanel();
        statusPanel.SetActive(false); 
        if (CombatManager.combatManager.charSelected)
            buttonStatus.SetActive(true);
    }

    public void UpdateStatusPanel()
    {
        var cm = CombatManager.combatManager;
        statusPanelLabelChar.text = cm.charSelected.charName;
        foreach (Status s in cm.charSelected.statusList)
        {
            GameObject NewObj = new GameObject(); 
            Image NewImage = NewObj.AddComponent<Image>(); 
            NewImage.sprite = s.statusSprite; 
            NewObj.GetComponent<RectTransform>().SetParent(posInitialStatusPanel.transform); 
            NewObj.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            NewObj.transform.localPosition = new Vector3(0, 0 - offsetStatus * cm.charSelected.statusList.IndexOf(s), 1);
            NewObj.SetActive(true);

            GameObject textObj = new GameObject();
            Text newText = textObj.AddComponent<Text>();
            newText.text = s.statusType.ToString();
            newText.text += "\nTurns : " + s.turnsActive;
            textObj.GetComponent<RectTransform>().SetParent(NewObj.transform);
            textObj.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            textObj.transform.localPosition = new Vector3(0 + offsetStatus, 0, 1);
            textObj.SetActive(true);
            goStatus.Add(NewObj);
        }
    }
    public void DestroyStatusPanel()
    {
        for (int i = goStatus.Count - 1; i >= 0; i--)
        {
            GameObject temp = goStatus[i];
            goStatus.Remove(temp);
            Destroy(temp);
        }
    }
}
