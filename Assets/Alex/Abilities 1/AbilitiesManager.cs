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
    public Ability[] weaponAbilities;
    public Ability[] cristalsAsh;
    public Ability[] cristalsIce;
    public Ability[] cristalsMud;
    public Ability[] cristalsPsy;
    public Ability[] abilitiesEnemies;

    public bool canAttack = false;
    public bool isAttackFinished = false;
    private bool abilitiesUiShow = true;


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
        if (!abilitiesUiShow) return;


        if (CombatManager.combatManager.allyPlaying && !CombatManager.combatManager.allyPlaying.hasPlayed && CombatManager.combatManager.enemies.Count > 0) 
        {
            abilitiesUI.SetActive(true);
            ChangeUIAbilities();
        }else if(CombatManager.combatManager.allyPlaying && CombatManager.combatManager.allyCombo && !CombatManager.combatManager.allyPlaying.hasPlayed && CombatManager.combatManager.enemies.Count <= 0)
        {
            abilitiesUI.SetActive(false);
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

    private void HideUI()
    {
        abilitiesUI.SetActive(false);
        actionButton.SetActive(false);
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
                var asa = abilitySelected.ability;
                abilityNameUI.text = asa.name;
                abilityDescription.text = asa.objectType.ToString();
                abilityDescription.text += "\n" + asa.targetType.ToString();
                abilityDescription.text += "\n" + "Mult " + asa.multiplicator;
                if(asa.objectType == Ability.ObjectType.CRISTAL)
                {
                    abilityDescription.text += "\n" + asa.elementType.ToString();
                    abilityDescription.text += "\n" + asa.crType.ToString();
                    if(asa.crAttackType == Ability.CristalAttackType.DESTRUCTION || asa.crHealType == Ability.CristalHealType.BOOST || asa.crHealType == Ability.CristalHealType.DRINK)
                    {
                        abilityDescription.text += "\n" + "Duration " + asa.turnDuration;
                    }else if (asa.crAttackType == Ability.CristalAttackType.DOT)
                    {
                        abilityDescription.text += "\n" + "Duration " + asa.turnDuration;
                        abilityDescription.text += "\n" + "Dot mult " + asa.dotMult;
                    }
                    else if(asa.crAttackType == Ability.CristalAttackType.MARK)
                    {
                        abilityDescription.text += "\n" + "Duration " + asa.turnDuration;
                        abilityDescription.text += "\n" + "Mark mult" + asa.markMult;
                    }
                }
                else if(asa.objectType == Ability.ObjectType.WEAPON)
                {
                    abilityDescription.text += "\n" + "Type " + asa.weaponAbilityType.ToString();
                    if (asa.weaponAbilityType == Ability.WeaponAbilityType.DEFENCE)
                    {
                        abilityDescription.text += "\n" + "Duration " +  asa.turnDuration;
                    }
                }
            }
        }
    }
    public void SetTargets(Ability ab) 
    {
        if (ab.objectType == Ability.ObjectType.CRISTAL)
        {
            if (ab.crType == Ability.CristalAbilityType.HEAL)
            {
                if (ab.crHealType == Ability.CristalHealType.BATH && ab.elementType == Ability.ElementType.PSY)
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
                        e.isTargetable = true;
                    }
                }
                else
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
        }
        else
        {
            //if (ab.targetType == Ability.TargetType.RANGE)
            switch (ab.targetType)
            {
                case Ability.TargetType.ALLIES:
                    
                        foreach (Ally a in CombatManager.combatManager.allies)
                        {
                            if (!a.isDead && a != CombatManager.combatManager.allyPlaying)
                                a.isTargetable = false;
                            else
                                a.isTargetable = true;
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
        if (!abilitiesUiShow) return;

        if (abilitySelected && CombatManager.combatManager.allyPlaying && !CombatManager.combatManager.allyPlaying.hasPlayed)
        {
            if(abilitySelected.ability.targetType == Ability.TargetType.ALLIES || abilitySelected.ability.crType == Ability.CristalAbilityType.HEAL)
            {
                if (CombatManager.combatManager.charSelected && CombatManager.combatManager.charSelected.isTargetable)
                {
                    actionButton.SetActive(true);
                }
                else
                {
                    actionButton.SetActive(false);
                }
            }
            else if(abilitySelected.ability.targetType == Ability.TargetType.RANGE || abilitySelected.ability.targetType == Ability.TargetType.MELEE 
                || abilitySelected.ability.crType == Ability.CristalAbilityType.ATTACK)
            {
                if (CombatManager.combatManager.charSelected && CombatManager.combatManager.charSelected.isTargetable)
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
        StartCoroutine(AllyActionAbilityCor());
    }
    public IEnumerator AllyActionAbilityCor()
    {
        Ally ally = CombatManager.combatManager.allyPlaying;

        if ((abilitySelected.ability.objectType == Ability.ObjectType.WEAPON && abilitySelected.ability.weaponAbilityType != Ability.WeaponAbilityType.DEFENCE) || abilitySelected.ability.crType == Ability.CristalAbilityType.ATTACK)
        {
            if (ally.weaponType == NItem.EWeaponType.Staff)
                ally.GetComponent<Animator>().SetTrigger("AttackStaff");
            else if (ally.weaponType == NItem.EWeaponType.Sword)
                ally.GetComponent<Animator>().SetTrigger("AttackSword");
            else if (ally.weaponType == NItem.EWeaponType.Bow)
                ally.GetComponent<Animator>().SetTrigger("AttackBow");
        }
        else if (abilitySelected.ability.crType == Ability.CristalAbilityType.HEAL || abilitySelected.ability.weaponAbilityType == Ability.WeaponAbilityType.DEFENCE)
        {
            canAttack = true;
            isAttackFinished = true;
        }

        abilitiesUiShow = false;
        HideUI();

        while (!canAttack)
            yield return null;

        AbilityAction(abilitySelected.ability);

        while (!isAttackFinished)
            yield return null;  

        //WAIT FOR NEXT CHAR ATTACK
        CombatManager.combatManager.NextCharAttack();
        abilitiesUiShow = true;
        abilitySelected.isSelected = false;
        abilitySelected = null;
        canAttack = false;
        isAttackFinished = false;
        ClearTargets();
        yield return null;
    }



    public void AbilityAction(Ability abi)
    {
        var cm = CombatManager.combatManager;
        //-----------------------CHANGE ABILITY IN FUNCTION OF TARGETTYPE---------------------------------
        if (abi.objectType == Ability.ObjectType.CRISTAL)
        {
            //----------------------------------------------------ABILITIES ON CRISTAL----------------------------------------
            if (cm.charSelected.isTargetable)
            {
                CristalAction(abi);
            }
            //------------------------------------------------------------ABILITY ON WEAPONS----------------------------------------------------
        }
        else if (abi.objectType == Ability.ObjectType.WEAPON)
        {
            if (abi.weaponAbilityType != Ability.WeaponAbilityType.DEFENCE)
            {
                switch (abi.targetType)
                {
                    case Ability.TargetType.RANGE:

                        if (cm.charSelected.isTargetable)
                        {
                            if (abi.weaponAbilityType == Ability.WeaponAbilityType.PIERCE)
                            {
                                if (cm.enemies.Count <= 1)
                                {
                                    cm.allyPlaying.LaunchAttack(cm.charSelected, abi);
                                }
                                else
                                {
                                    cm.allyPlaying.LaunchAttack(cm.charSelected, abi);
                                    for (int i = cm.enemies.Count - 1; i >= 0; i--)
                                    {
                                        if (cm.enemies[i].teamPosition == cm.charSelected.teamPosition + 1)
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
                                cm.allyPlaying.LaunchAttack(cm.charSelected, abi);
                            }
                        }
                        break;
                    case Ability.TargetType.MELEE:
                        if (cm.charSelected.isTargetable)
                        {
                            cm.allyPlaying.LaunchAttack(cm.charSelected, abi);
                        }
                        break;
                }
            }
            else
            {
                if (cm.charSelected.isTargetable)
                {
                    Status s1 = null;
                    foreach (Status s in cm.allyPlaying.statusList)
                    {
                        if (s.statusType == Status.StatusTypes.Defence)
                        {
                            s1 = s;
                            s.turnsActive = 2;
                        }
                    }
                    if(s1 == null)
                    {
                        s1 = new Status(cm.allyPlaying, 0, abi, Status.StatusTypes.Defence);
                    }
                    AudioManager.audioManager.Play("AllyDefence");
                    new Status(cm.allyPlaying, Mathf.Round(cm.allyPlaying.armor * 0.6f), abi, Status.StatusTypes.ArmorBonus);
                }
            }
        }
         
    }
    public void CristalAction(Ability a)
    {
        var cm = CombatManager.combatManager;
        if (a.crType == Ability.CristalAbilityType.HEAL)
        {
            switch (a.crHealType)
            {
                case Ability.CristalHealType.BOOST:
                    AudioManager.audioManager.Play("CrystalBoost");
                    cm.allyPlaying.LaunchBuff(cm.charSelected, a);
                    break;
                case Ability.CristalHealType.BATH:
                    AudioManager.audioManager.Play("CrystalBath");
                    AbilityBath(a);
                    break;
                case Ability.CristalHealType.DRINK:
                    AudioManager.audioManager.Play("CrystalDrink");
                    cm.allyPlaying.LaunchHeal(cm.charSelected, a);
                    cm.allyPlaying.LaunchBuff(cm.charSelected, a);
                    break;
            }
        }
        else if (a.crType == Ability.CristalAbilityType.ATTACK)
        {
            if(a.crAttackType == Ability.CristalAttackType.MARK)
            {
                cm.allyPlaying.PutMark(cm.charSelected, a);
            }
            else
            {
                //DOT AND DESTRUCTION INSIDE LAUNCH ATTACK
                cm.allyPlaying.LaunchAttack(cm.charSelected, a);
            }
        }
        ChangeAbilities();
    }
    public void AbilityBath(Ability a)
    {
        var cm = CombatManager.combatManager;
        switch (a.elementType)
        {
            case Ability.ElementType.ASH:
                Characters tempC = cm.charSelected;
                cm.allyPlaying.LaunchHeal(tempC, a);
                if (cm.allies.Count > 1)
                {
                    int randA = Random.Range(0, cm.allies.Count);
                    while (cm.allies[randA] == tempC)
                    {
                        randA = Random.Range(0, cm.allies.Count);
                    }
                    cm.allyPlaying.LaunchHeal(cm.allies[randA], a);
                }
                break;
            case Ability.ElementType.ICE:
                cm.allyPlaying.LaunchHeal(cm.charSelected, a);
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
