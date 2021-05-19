using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesManager : MonoBehaviour
{
    public static AbilitiesManager abilitiesManager;
    public AbilityScript abilitySelected;
    public Ability lastAbilityLaunched;
    public GameObject abilitiesUI;
    public GameObject actionButton;
    public GameObject abilityUI;
    public Text abilityNameUI;
    public Text abilityDescription;
    public AbilityScript Ability01;
    public AbilityScript Ability02;
    public AbilityScript Ability03;
    public AbilityScript Ability04;
    public Ability[] abilitiesAllies;
    public Ability[] abilitiesEnemies;
    private void Awake()
    {
        if (abilitiesManager == null)
        {
            abilitiesManager = this;
        }
        else if (abilitiesManager != this)
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        actionButton = GameObject.Find("ActionButton");
        abilityNameUI = GameObject.Find("AbilityName").GetComponent<Text>();
        abilityDescription = GameObject.Find("AbilityDescription").GetComponent<Text>();
        abilitiesUI = GameObject.Find("AbilitiesUI");
        abilityUI = GameObject.Find("AbilityUI");
        Ability01 = GameObject.Find("Ability01").GetComponent<AbilityScript>();
        Ability02 = GameObject.Find("Ability02").GetComponent<AbilityScript>();
        Ability03 = GameObject.Find("Ability03").GetComponent<AbilityScript>();
        Ability04 = GameObject.Find("Ability04").GetComponent<AbilityScript>();
        abilitiesUI.SetActive(false);
        abilityUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        DisplayAbilities();
        DisplayActionButton();
    }
    

    public void DisplayAbilities() 
    {
        if (CombatManager.combatManager.allyPlaying) 
        {
            abilitiesUI.SetActive(true);
            ChangeUIAbilities();
        }
        else
        {
            abilitiesUI.SetActive(false);
        }

        if(abilitySelected) 
        {
            abilityUI.SetActive(true);
        }
        else 
        {
            abilityUI.SetActive(false);
        }
    }
    public void ChangeUIAbilities()
    {
        if (abilitiesUI.activeSelf)
        {
            Ability01.GetComponent<SpriteRenderer>().sprite = CombatManager.combatManager.allyPlaying.abilities[0].icon;
            Ability02.GetComponent<SpriteRenderer>().sprite = CombatManager.combatManager.allyPlaying.abilities[1].icon;
            Ability03.GetComponent<SpriteRenderer>().sprite = CombatManager.combatManager.allyPlaying.abilities[2].icon;
            Ability04.GetComponent<SpriteRenderer>().sprite = CombatManager.combatManager.allyPlaying.abilities[3].icon;

            Ability01.ability = CombatManager.combatManager.allyPlaying.abilities[0];
            Ability02.ability = CombatManager.combatManager.allyPlaying.abilities[1];
            Ability03.ability = CombatManager.combatManager.allyPlaying.abilities[2];
            Ability04.ability = CombatManager.combatManager.allyPlaying.abilities[3];
            if (abilitySelected)
            {
                abilityNameUI.text = abilitySelected.ability.name;
                abilityDescription.text = "Multiplicator" + abilitySelected.ability.multiplicator;
            }
        }
    }
    public void SetTargets(Ability.TargetType targetType) 
    {
        switch (targetType) 
        {
            case Ability.TargetType.TEAM:
                foreach (Ally a in CombatManager.combatManager.chars)
                {
                    if(!a.isDead)
                        a.isTargetable = true;
                    else
                        a.isTargetable = false;
                }
                foreach (Enemy e in CombatManager.combatManager.enemies)
                {
                    e.isTargetable = false;
                }
                break;
            case Ability.TargetType.RANGE:
                foreach (Enemy e in CombatManager.combatManager.enemies)
                {
                    e.isTargetable = true;
                }
                foreach (Ally a in CombatManager.combatManager.chars)
                {
                    a.isTargetable = false;
                }
                break;
            case Ability.TargetType.MELEE:
                foreach (Enemy e in CombatManager.combatManager.enemies)
                {
                    if (e.isMelee)
                    {
                        e.isTargetable = true;
                    }
                    else
                    {
                        e.isTargetable = false;
                    }
                }
                foreach (Ally a in CombatManager.combatManager.chars)
                {
                    a.isTargetable = false;
                }
                break;
        }
    }
    public void DisplayActionButton() 
    {
        if (abilitySelected && CombatManager.combatManager.allyPlaying)
        {
            if(abilitySelected.ability.targetType == Ability.TargetType.TEAM)
            {
                if (CombatManager.combatManager.allySelected && CombatManager.combatManager.allySelected.isTargetable)
                {
                    actionButton.SetActive(true);
                }
                else
                {
                    actionButton.SetActive(false);
                }
            }
            else if(abilitySelected.ability.targetType == Ability.TargetType.RANGE || abilitySelected.ability.targetType == Ability.TargetType.MELEE)
            {
                if (CombatManager.combatManager.enemySelected && CombatManager.combatManager.enemySelected.isTargetable)
                {
                    actionButton.SetActive(true);
                }
                else
                {
                    actionButton.SetActive(false);
                }
            }
        }else
        {
            actionButton.SetActive(false);
        }
    }
    public void AllyActionAbility() 
    {
        switch (abilitySelected.ability.targetType)
        {
            case Ability.TargetType.TEAM:
                if (CombatManager.combatManager.allySelected.isTargetable)
                {
                    CombatManager.combatManager.allyPlaying.InteractWith(CombatManager.combatManager.allySelected, abilitySelected.ability);
                }
            break;
            case Ability.TargetType.RANGE:
                if (CombatManager.combatManager.enemySelected.isTargetable)
                {
                    CombatManager.combatManager.allyPlaying.InteractWith(CombatManager.combatManager.enemySelected, abilitySelected.ability);
                }
                break;
            case Ability.TargetType.MELEE:
                if (CombatManager.combatManager.enemySelected.isTargetable)
                {
                    CombatManager.combatManager.allyPlaying.InteractWith(CombatManager.combatManager.enemySelected, abilitySelected.ability);
                }
                break;
        }
        CombatManager.combatManager.NextCharAttack();
        /*CombatManager.combatManager.fightersList[CombatManager.combatManager.currCharAttacking].isSelected = false;
        CombatManager.combatManager.allySelected = null;*/
    }

}
