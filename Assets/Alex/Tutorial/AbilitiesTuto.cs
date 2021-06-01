using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesTuto : MonoBehaviour
{
    public static AbilitiesTuto abilitiesTuto;
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
    //public Ability[] abilitiesWeaponsAllies;
    public Ability[] weaponAbilities;
    public Ability[] cristalsAsh;
    public Ability[] cristalsIce;
    public Ability[] cristalsMud;
    public Ability[] cristalsPsy;
    public Ability[] abilitiesEnemies;



    private void Awake()
    {
        if (abilitiesTuto == null)
        {
            abilitiesTuto = this;
        }
        else if (abilitiesTuto != this)
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
        if (TutoCombat.tutoCombat.allyPlaying && !TutoCombat.tutoCombat.allyPlaying.hasPlayed)
        {
            abilitiesUI.SetActive(true);
            ChangeUIAbilities();
        }
        else
        {
            abilitiesUI.SetActive(false);
        }

        if (abilitySelected)
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
            if(TutoCombat.tutoCombat.allyPlaying.abilities[0])
            {
                Ability01.GetComponent<SpriteRenderer>().sprite = TutoCombat.tutoCombat.allyPlaying.abilities[0].icon;
                Ability01.ability = TutoCombat.tutoCombat.allyPlaying.abilities[0];
            }
            if (TutoCombat.tutoCombat.allyPlaying.abilities[1])
            {
                Ability02.GetComponent<SpriteRenderer>().sprite = TutoCombat.tutoCombat.allyPlaying.abilities[1].icon;
                Ability02.ability = TutoCombat.tutoCombat.allyPlaying.abilities[1];
            }
            if (TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[0])
            {
                Ability03.GetComponent<SpriteRenderer>().sprite = TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[0].icon;
                Ability03.ability = TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[0];
            }
            if (TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[1])
            {
                Ability04.GetComponent<SpriteRenderer>().sprite = TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[1].icon;
                Ability04.ability = TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[1];
            }

            if (abilitySelected)
            {
                abilityNameUI.text = abilitySelected.ability.name;
                abilityDescription.text += "\n" + abilitySelected.ability.type;
                //abilityDescription.text += "\n" + abilitySelected.ability.objectType.ToString();
                abilityDescription.text += "\n" + abilitySelected.ability.targetType.ToString();
                abilityDescription.text = "Multiplicator" + abilitySelected.ability.multiplicator + "\n";
                if (abilitySelected.ability.objectType == Ability.ObjectType.CRISTAL)
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
                foreach (AllyTuto a in TutoCombat.tutoCombat.allies)
                {
                    if (!a.isDead)
                        a.isTargetable = true;
                    else
                        a.isTargetable = false;
                }
                foreach (EnemyTuto e in TutoCombat.tutoCombat.enemies)
                {
                    e.isTargetable = false;
                }
            }
            else if (ab.crType == Ability.CristalAbilityType.ATTACK)
            {
                foreach (EnemyTuto e in TutoCombat.tutoCombat.enemies)
                {
                    e.isTargetable = true;
                }
                foreach (AllyTuto a in TutoCombat.tutoCombat.allies)
                {
                    a.isTargetable = false;
                }
            }
            else if (ab.crType == Ability.CristalAbilityType.OTHERS)
            {
                foreach (EnemyTuto e in TutoCombat.tutoCombat.enemies)
                {
                    e.isTargetable = true;
                }
                foreach (AllyTuto a in TutoCombat.tutoCombat.allies)
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
                    foreach (AllyTuto a in TutoCombat.tutoCombat.allies)
                    {
                        if (!a.isDead)
                            a.isTargetable = true;
                        else
                            a.isTargetable = false;
                    }
                    foreach (EnemyTuto e in TutoCombat.tutoCombat.enemies)
                    {
                        e.isTargetable = false;
                    }
                    break;
                case Ability.TargetType.RANGE:
                    foreach (EnemyTuto e in TutoCombat.tutoCombat.enemies)
                    {
                        e.isTargetable = true;
                    }
                    foreach (AllyTuto a in TutoCombat.tutoCombat.allies)
                    {
                        a.isTargetable = false;
                    }
                    break;
                case Ability.TargetType.MELEE:
                    foreach (EnemyTuto e in TutoCombat.tutoCombat.enemies)
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
                    foreach (AllyTuto a in TutoCombat.tutoCombat.allies)
                    {
                        a.isTargetable = false;
                    }
                    break;
            }
        }
    }
    public void ClearTargets()
    {
        if (abilitySelected == null)
        {
            foreach (EnemyTuto e in TutoCombat.tutoCombat.enemies)
            {
                e.isTargetable = false;
            }
            foreach (AllyTuto a in TutoCombat.tutoCombat.allies)
            {
                a.isTargetable = false;
            }
        }
    }
    public void DisplayActionButton()
    {
        if (abilitySelected && TutoCombat.tutoCombat.allyPlaying && !TutoCombat.tutoCombat.allyPlaying.hasPlayed)
        {
            if (abilitySelected.ability.targetType == Ability.TargetType.ALLIES || abilitySelected.ability.crType == Ability.CristalAbilityType.HEAL)
            {
                if (TutoCombat.tutoCombat.allySelected && TutoCombat.tutoCombat.allySelected.isTargetable)
                {
                    actionButton.SetActive(true);
                }
                else
                {
                    actionButton.SetActive(false);
                }
            }
            else if (abilitySelected.ability.targetType == Ability.TargetType.RANGE || abilitySelected.ability.targetType == Ability.TargetType.MELEE
                || abilitySelected.ability.crType == Ability.CristalAbilityType.ATTACK || abilitySelected.ability.crType == Ability.CristalAbilityType.OTHERS)
            {
                if (TutoCombat.tutoCombat.enemySelected && TutoCombat.tutoCombat.enemySelected.isTargetable)
                {
                    actionButton.SetActive(true);
                }
                else
                {
                    actionButton.SetActive(false);
                }
            }
        }
        else
        {
            actionButton.SetActive(false);
        }
    }
    public void AllyActionAbility()
    {
        AbilityAction(abilitySelected.ability);
        TutoCombat.tutoCombat.NextCharAttack();
        lastAbilityLaunched = abilitySelected.ability;
        abilitySelected.isSelected = false;
        abilitySelected = null;
    }
    public void AbilityAction(Ability abi)
    {
        var cm = TutoCombat.tutoCombat;
        //-----------------------CHANGE ABILITY IN FUNCTION OF TARGETTYPE---------------------------------
        if (abi.objectType == Ability.ObjectType.CRISTAL)
        {
            //----------------------------------------------------ABILITIES ON CRISTAL----------------------------------------
            if (abi.crType == Ability.CristalAbilityType.HEAL)
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
            //------------------------------------------------------------ABILITY ON WEAPONS----------------------------------------------------
        }
        else if (abi.objectType == Ability.ObjectType.WEAPON)
        {
            if (abi.weaponAbilityType != Ability.WeaponAbilityType.DEFENSE)
            {
                switch (abi.targetType)
                {
                    case Ability.TargetType.RANGE:

                        if (cm.enemySelected.isTargetable)
                        {
                            if (abi.weaponAbilityType == Ability.WeaponAbilityType.PIERCE)
                            {
                                if (cm.enemies.Count <= 1)
                                {
                                    cm.allyPlaying.LaunchAttack(cm.enemySelected, abi);
                                }
                                else
                                {
                                    cm.allyPlaying.LaunchAttack(cm.enemySelected, abi);
                                    for (int i = cm.enemies.Count - 1; i >= 0; i--)
                                    {
                                        if (cm.enemies[i].teamPosition == cm.enemySelected.teamPosition + 1)
                                        {
                                            EnemyTuto ndEnemyTuto = cm.enemies[i];
                                            cm.allyPlaying.LaunchAttack(ndEnemyTuto, abi);
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
            }
            else
            {
                if (cm.allySelected.isTargetable)
                {
                    cm.allyPlaying.inDefenceMode = true;
                    StatusTuto1 s = new StatusTuto1(cm.allyPlaying, 0.6f, abi, StatusTuto1.StatusTypes.ARMORBONUSPERC);
                }
            }
        }

    }
    public void CristalAction(Ability a)
    {
        var cm = TutoCombat.tutoCombat;
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
        var cm = TutoCombat.tutoCombat;
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

                foreach (AllyTuto al in TutoCombat.tutoCombat.allies)
                {
                    cm.allyPlaying.LaunchHeal(al, a);
                }
                break;
            case Ability.ElementType.PSY:
                foreach (AllyTuto al in TutoCombat.tutoCombat.allies)
                {
                    cm.allyPlaying.LaunchHeal(al, a);
                }
                foreach (EnemyTuto e in TutoCombat.tutoCombat.enemies)
                {
                    cm.allyPlaying.LaunchHeal(e, a);
                }
                break;
        }
    }
    public void ChangeAbilities()
    {
        var cm = TutoCombat.tutoCombat;
        switch (cm.allyPlaying.itemElement)
        {
            case AllyTuto.ItemElement.ASH:
                for (int i = 0; i < cm.allyPlaying.abilitiesCristal.Length; i++)
                {
                    if (abilitySelected.ability == cm.allyPlaying.abilitiesCristal[i])
                    {
                        cm.allyPlaying.abilitiesCristal[i] = AbilitiesManager.abilitiesManager.cristalsAsh[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsAsh.Length)];
                        while (cm.allyPlaying.abilitiesCristal[0] == cm.allyPlaying.abilitiesCristal[1])
                        {
                            cm.allyPlaying.abilitiesCristal[i] = AbilitiesManager.abilitiesManager.cristalsAsh[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsAsh.Length)];
                        }
                    }
                }
                break;
            case AllyTuto.ItemElement.ICE:
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
            case AllyTuto.ItemElement.MUD:
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
            case AllyTuto.ItemElement.PSY:
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
