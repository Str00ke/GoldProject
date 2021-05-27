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
    public GameObject comboAbilitiesUI;
    public GameObject actionButton;
    public GameObject abilityUI;
    public Text abilityNameUI;
    public Text abilityDescription;
    public AbilityScript Ability01;
    public AbilityScript Ability02;
    public AbilityScript Ability03;
    public AbilityScript Ability04;
    //public Ability[] abilitiesWeaponsAllies;
    public Ability[] cristalsAsh;
    public Ability[] cristalsIce;
    public Ability[] cristalsMud;
    public Ability[] cristalsPsy;
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
        //cristalsAsh = Assets / Alex / Abilities / CristalsAbilities / Ash / CAAttackDotAsh.asset
        actionButton = GameObject.Find("ActionButton");
        abilityNameUI = GameObject.Find("AbilityName").GetComponent<Text>();
        abilityDescription = GameObject.Find("AbilityDescription").GetComponent<Text>();
        abilitiesUI = GameObject.Find("AbilitiesUI");
        abilityUI = GameObject.Find("AbilityUI");
        comboAbilitiesUI = GameObject.Find("AbilitiesCombo");
        Ability01 = GameObject.Find("Ability01").GetComponent<AbilityScript>();
        Ability02 = GameObject.Find("Ability02").GetComponent<AbilityScript>();
        Ability03 = GameObject.Find("Ability03").GetComponent<AbilityScript>();
        Ability04 = GameObject.Find("Ability04").GetComponent<AbilityScript>();
        abilitiesUI.SetActive(false);
        abilityUI.SetActive(false);
        comboAbilitiesUI.SetActive(false);
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
            comboAbilitiesUI.SetActive(false);
            ChangeUIAbilities();
        }else if(CombatManager.combatManager.allyPlaying && CombatManager.combatManager.allyCombo)
        {
            abilitiesUI.SetActive(false);
            comboAbilitiesUI.SetActive(true);
        }
        else
        {
            comboAbilitiesUI.SetActive(false);
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
            Ability03.GetComponent<SpriteRenderer>().sprite = CombatManager.combatManager.allyPlaying.abilitiesCristal[0].icon;
            Ability04.GetComponent<SpriteRenderer>().sprite = CombatManager.combatManager.allyPlaying.abilitiesCristal[1].icon;

            Ability01.ability = CombatManager.combatManager.allyPlaying.abilities[0];
            Ability02.ability = CombatManager.combatManager.allyPlaying.abilities[1];
            Ability03.ability = CombatManager.combatManager.allyPlaying.abilitiesCristal[0];
            Ability04.ability = CombatManager.combatManager.allyPlaying.abilitiesCristal[1];
            if (abilitySelected)
            {
                abilityNameUI.text = abilitySelected.ability.name;
                abilityDescription.text += "\n" + abilitySelected.ability.type;
                //abilityDescription.text += "\n" + abilitySelected.ability.objectType.ToString();
                abilityDescription.text += "\n" + abilitySelected.ability.targetType.ToString();
                abilityDescription.text = "Multiplicator" + abilitySelected.ability.multiplicator + "\n";
                if(abilitySelected.ability.objectType == Ability.ObjectType.CRISTAL)
                {
                    abilityDescription.text += "\n" + abilitySelected.ability.crType.ToString();
                }
                else
                {
                    abilityDescription.text += "\n" + abilitySelected.ability.weaponAbilityType.ToString();
                }
                abilityDescription.text += "\n" + abilitySelected.ability.elementType.ToString();
            }
        }
    }
    public void SetTargets(Ability ab) 
    {
        if (ab.objectType == Ability.ObjectType.CRISTAL)
        {
            if (ab.crType == Ability.CristalAbilityType.HEAL)
            {
                foreach (Ally a in CombatManager.combatManager.allies)
                {
                    if (!a.isDead)
                        a.isTargetable = true;
                    else
                        a.isTargetable = false;
                }
                foreach (Enemy e in CombatManager.combatManager.enemies)
                {
                    e.isTargetable = false;
                }
            }
            else if (ab.crType == Ability.CristalAbilityType.ATTACK)
            {
                foreach (Enemy e in CombatManager.combatManager.enemies)
                {
                    e.isTargetable = true;
                }
                foreach (Ally a in CombatManager.combatManager.allies)
                {
                    a.isTargetable = false;
                }
            }
            else if (ab.crType == Ability.CristalAbilityType.OTHERS)
            {
                foreach (Enemy e in CombatManager.combatManager.enemies)
                {
                    e.isTargetable = true;
                }
                foreach (Ally a in CombatManager.combatManager.allies)
                {
                    a.isTargetable = false;
                }
            }
        }
        else
        {
            if (ab.targetType == Ability.TargetType.RANGE)
                switch (ab.targetType)
                {
                    case Ability.TargetType.ALLIES:
                        foreach (Ally a in CombatManager.combatManager.allies)
                        {
                            if (!a.isDead)
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
                        foreach (Ally a in CombatManager.combatManager.allies)
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
                        foreach (Ally a in CombatManager.combatManager.allies)
                        {
                            a.isTargetable = false;
                        }
                        break;
                }
        }
    }
    public void ClearTargets()
    {
        if(abilitySelected == null)
        {
            foreach (Enemy e in CombatManager.combatManager.enemies)
            {
                e.isTargetable = false;
            }
            foreach (Ally a in CombatManager.combatManager.allies)
            {
                a.isTargetable = false;
            }
        }
    }
    public void DisplayActionButton() 
    {
        if (abilitySelected && CombatManager.combatManager.allyPlaying)
        {
            if(abilitySelected.ability.targetType == Ability.TargetType.ALLIES || abilitySelected.ability.crType == Ability.CristalAbilityType.HEAL)
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
            else if(abilitySelected.ability.targetType == Ability.TargetType.RANGE || abilitySelected.ability.targetType == Ability.TargetType.MELEE 
                || abilitySelected.ability.crType == Ability.CristalAbilityType.ATTACK || abilitySelected.ability.crType == Ability.CristalAbilityType.OTHERS)
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
        AbilityAction(abilitySelected.ability);
        CombatManager.combatManager.NextCharAttack();
        lastAbilityLaunched = abilitySelected.ability;
        abilitySelected.isSelected = false;
        abilitySelected = null;
    }
    public void AbilityAction(Ability abi)
    {
        var cm = CombatManager.combatManager;
        //-----------------------CHANGE ABILITY IN FUNCTION OF TARGETTYPE---------------------------------
        switch (abi.objectType)
        {
            //----------------------------------------------------ABILITIES ON CRISTAL----------------------------------------
            case Ability.ObjectType.CRISTAL:
                if(abi.crType == Ability.CristalAbilityType.HEAL)
                {
                    if (cm.allySelected.isTargetable)
                    {
                        CristalAction(abi);
                    }
                }
                else
                {
                    if (cm.enemySelected.isTargetable)
                    {
                        CristalAction(abi);
                    }
                }
                break;
            //------------------------------------------------------------ABILITY ON WEAPONS----------------------------------------------------
            case Ability.ObjectType.WEAPON:
                switch (abi.targetType)
                {
                    case Ability.TargetType.RANGE:

                        if (cm.enemySelected.isTargetable)
                        {
                            if (abi.weaponAbilityType == Ability.WeaponAbilityType.PIERCE)
                            {
                                if(cm.enemies.Count <= 1)
                                {
                                    cm.allyPlaying.LaunchAttack(cm.enemySelected, abi);
                                }
                                else
                                {
                                    cm.allyPlaying.LaunchAttack(cm.enemySelected, abi);
                                    for (int i = cm.enemies.Count - 1; i >= 0; i--)
                                    {
                                        if(cm.enemies[i].teamPosition == cm.enemySelected.teamPosition + 1)
                                        {
                                            Enemy ndEnemy = cm.enemies[i];
                                            cm.allyPlaying.LaunchAttack(ndEnemy, abi);
                                        }
                                    }
                                }
                            }
                            else if (abi.weaponAbilityType == Ability.WeaponAbilityType.WAVE)
                            {
                                for (int i = cm.enemies.Count - 1; i >= 0; i--)
                                {
                                    cm.allyPlaying.LaunchAttack(cm.enemies[i], abi);
                                }
                            }
                            else
                            {
                                cm.allyPlaying.LaunchAttack(cm.enemySelected, abi);
                            }
                        }
                        break;
                    case Ability.TargetType.MELEE:
                        if (cm.enemySelected.isTargetable)
                        {
                            cm.allyPlaying.LaunchAttack(cm.enemySelected, abi);
                        }
                        break;
                }
                break;
        }
    }
    public void CristalAction(Ability a)
    {
        var cm = CombatManager.combatManager;
            switch (a.crType)
            {
                case Ability.CristalAbilityType.HEAL:
                    switch (a.crHealType)
                    {
                        case Ability.CristalHealType.BOOST:
                            cm.allyPlaying.LaunchBuff(cm.allySelected, a);
                            break;
                        case Ability.CristalHealType.BATH:
                            AbilityBath(a);
                    break;
                        case Ability.CristalHealType.DRINK:
                            cm.allyPlaying.LaunchHeal(cm.allySelected, a);
                            cm.allyPlaying.LaunchBuff(cm.allySelected, a);
                            break;
                    }
                    break;
                case Ability.CristalAbilityType.ATTACK:
                    switch (a.crAttackType)
                    {
                        case Ability.CristalAttackType.NORMAL:
                            cm.allyPlaying.LaunchAttack(cm.enemySelected, a);
                            break;
                        case Ability.CristalAttackType.DOT:
                            cm.allyPlaying.LaunchAttack(cm.enemySelected, a);
                            cm.allyPlaying.PutDot(cm.enemySelected, a);
                        break;
                        case Ability.CristalAttackType.MARK:
                            cm.allyPlaying.PutMark(cm.enemySelected, a);
                            break;
                    }
                    break;
                case Ability.CristalAbilityType.OTHERS:
                    switch (a.crSpecialType)
                    {
                        case Ability.CristalSpecialType.DESTRUCTION:
                            cm.allyPlaying.LaunchDestruction(cm.enemySelected, a);
                            cm.allyPlaying.LaunchAttack(cm.enemySelected, a);
                        break;
                        case Ability.CristalSpecialType.COPY:
                            break;
                    }
                    break;
        }
        ChangeAbilities();
    }
    public void AbilityBath(Ability a)
    {
        var cm = CombatManager.combatManager;
        switch (a.elementType)
        {
            case Ability.ElementType.ASH:
                cm.allyPlaying.LaunchHeal(cm.allySelected, a);
                if (cm.allies.Count > 1)
                {
                    int randA = Random.Range(0, cm.allies.Count - 1);
                    while (cm.allies[randA] == cm.allySelected)
                    {
                        randA = Random.Range(0, cm.allies.Count - 1);
                    }
                    cm.allyPlaying.LaunchHeal(cm.allies[randA], a);
                }
                break;
            case Ability.ElementType.ICE:
                cm.allyPlaying.LaunchHeal(cm.allySelected, a);
                break;
            case Ability.ElementType.MUD:

                foreach (Ally al in CombatManager.combatManager.allies)
                {
                    cm.allyPlaying.LaunchHeal(al, a);
                }
                break;
            case Ability.ElementType.PSY:
                foreach (Ally al in CombatManager.combatManager.allies)
                {
                    cm.allyPlaying.LaunchHeal(al, a);
                }
                foreach (Enemy e in CombatManager.combatManager.enemies)
                {
                    cm.allyPlaying.LaunchHeal(e, a);
                }
                break;
        }
    }
    public void ChangeAbilities()
    {
        var cm = CombatManager.combatManager;
        switch (cm.allyPlaying.itemElement)
        {
            case Ally.ItemElement.ASH:
                for(int i = 0; i < cm.allyPlaying.abilitiesCristal.Length; i++)
                {
                    if(abilitySelected.ability == cm.allyPlaying.abilitiesCristal[i])
                    {
                        cm.allyPlaying.abilitiesCristal[i] = AbilitiesManager.abilitiesManager.cristalsAsh[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsAsh.Length)];
                        while (cm.allyPlaying.abilitiesCristal[0] == cm.allyPlaying.abilitiesCristal[1])
                        {
                            cm.allyPlaying.abilitiesCristal[i] = AbilitiesManager.abilitiesManager.cristalsAsh[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsAsh.Length)];
                        }
                    }
                }
                break;
            case Ally.ItemElement.ICE:
                for (int i = 0; i < cm.allyPlaying.abilitiesCristal.Length; i++)
                {
                    if (abilitySelected.ability == cm.allyPlaying.abilitiesCristal[i])
                    {
                        cm.allyPlaying.abilitiesCristal[i] = AbilitiesManager.abilitiesManager.cristalsIce[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsIce.Length)];
                        while (cm.allyPlaying.abilitiesCristal[0] == cm.allyPlaying.abilitiesCristal[1])
                        {
                            cm.allyPlaying.abilitiesCristal[i] = AbilitiesManager.abilitiesManager.cristalsIce[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsIce.Length)];
                        }
                    }
                }
                break;
            case Ally.ItemElement.MUD:
                for (int i = 0; i < cm.allyPlaying.abilitiesCristal.Length; i++)
                {
                    if (abilitySelected.ability == cm.allyPlaying.abilitiesCristal[i])
                    {
                        cm.allyPlaying.abilitiesCristal[i] = AbilitiesManager.abilitiesManager.cristalsMud[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsMud.Length)];
                        while (cm.allyPlaying.abilitiesCristal[0] == cm.allyPlaying.abilitiesCristal[1])
                        {
                            cm.allyPlaying.abilitiesCristal[i] = AbilitiesManager.abilitiesManager.cristalsMud[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsMud.Length)];
                        }
                    }
                }
                break;
            case Ally.ItemElement.PSY:
                for (int i = 0; i < cm.allyPlaying.abilitiesCristal.Length; i++)
                {
                    if (abilitySelected.ability == cm.allyPlaying.abilitiesCristal[i])
                    {
                        cm.allyPlaying.abilitiesCristal[i] = AbilitiesManager.abilitiesManager.cristalsPsy[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsPsy.Length)];
                        while (cm.allyPlaying.abilitiesCristal[0] == cm.allyPlaying.abilitiesCristal[1])
                        {
                            cm.allyPlaying.abilitiesCristal[i] = AbilitiesManager.abilitiesManager.cristalsPsy[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsPsy.Length)];
                        }
                    }
                }
                break;
        }
    }
}
