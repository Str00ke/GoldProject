using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesManager : MonoBehaviour
{
    public static AbilitiesManager abilitiesManager;
    public AbilityScript abilitySelected;
    public GameObject abilitiesUI;
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
    public void SetTargets(bool melee, bool ally) 
    {
        if (!ally && melee)
        {
            foreach(Enemy e in CombatManager.combatManager.enemies) 
            {
                
            }
            
        }
    }

    public void ActionAbility() 
    {
        if (abilitySelected.ability.isMelee) 
        {
            SetTargets(abilitySelected.ability.isMelee, abilitySelected.ability.onAlly);
        }
    }
}
