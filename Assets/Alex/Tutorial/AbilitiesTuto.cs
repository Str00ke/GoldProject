using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesTuto : MonoBehaviour
{
    public static AbilitiesTuto abilitiesTuto;
    public AbilitiesScriptTuto abilitySelected;
    public Ability lastAbilityLaunched;
    public GameObject abilitiesUI;
    public GameObject actionButton;
    public GameObject abilityUI;
    public Text abilityNameUI;
    public Text abilityDescription;
    public AbilitiesScriptTuto Ability01;
    public AbilitiesScriptTuto Ability02;
    public AbilitiesScriptTuto Ability03;
    public AbilitiesScriptTuto Ability04;
    //public Ability[] abilitiesWeaponsAllies;
    public Ability[] weaponAbilities;
    public Ability[] cristalsAsh;
    public Ability[] cristalsIce;
    public Ability[] cristalsMud;
    public Ability[] cristalsPsy;
    public Ability[] abilitiesEnemies;

    public Sprite abilityBlank;


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
        Ability01 = GameObject.Find("Ability01").GetComponent<AbilitiesScriptTuto>();
        Ability02 = GameObject.Find("Ability02").GetComponent<AbilitiesScriptTuto>();
        Ability03 = GameObject.Find("Ability03").GetComponent<AbilitiesScriptTuto>();
        Ability04 = GameObject.Find("Ability04").GetComponent<AbilitiesScriptTuto>();
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
                Debug.Log(TutoCombat.tutoCombat.allyPlaying.abilities[0].icon.name);
                Ability01.ability = TutoCombat.tutoCombat.allyPlaying.abilities[0];
            }
            else
            {
                Ability01.GetComponent<SpriteRenderer>().sprite = abilityBlank;
                Ability01.ability = null;
            }
            if (TutoCombat.tutoCombat.allyPlaying.abilities[1])
            {
                Ability02.GetComponent<SpriteRenderer>().sprite = TutoCombat.tutoCombat.allyPlaying.abilities[1].icon;
                Ability02.ability = TutoCombat.tutoCombat.allyPlaying.abilities[1];
            }
            else
            {
                Ability02.GetComponent<SpriteRenderer>().sprite = abilityBlank;
                Ability02.ability = null;
            }
            if (TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[0])
            {
                Ability03.GetComponent<SpriteRenderer>().sprite = TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[0].icon;
                Ability03.ability = TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[0];
            }
            else
            {
                Ability03.GetComponent<SpriteRenderer>().sprite = abilityBlank;
                Ability03.ability = null;
            }
            if (TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[1])
            {
                Ability04.GetComponent<SpriteRenderer>().sprite = TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[1].icon;
                Ability04.ability = TutoCombat.tutoCombat.allyPlaying.abilitiesCristal[1];
            }
            else
            {
                Ability04.GetComponent<SpriteRenderer>().sprite = abilityBlank;
                Ability04.ability = null;
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
        }
        else
        {
            if (ab.weaponAbilityType == Ability.WeaponAbilityType.DEFENCE)
            {
                foreach (AllyTuto a in TutoCombat.tutoCombat.allies)
                {
                    if (!a.isDead && a == TutoCombat.tutoCombat.allyPlaying)
                        a.isTargetable = true;
                    else if(!a.isDead)
                        a.isTargetable = false;
                }
                foreach (EnemyTuto e in TutoCombat.tutoCombat.enemies)
                {
                    e.isTargetable = false;
                }
            }
            else if (ab.targetType == Ability.TargetType.ALLIES)
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
            else if (ab.targetType == Ability.TargetType.RANGE)
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
            else if (ab.targetType == Ability.TargetType.MELEE)
            {
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
                if (TutoCombat.tutoCombat.charSelected && TutoCombat.tutoCombat.charSelected.isTargetable)
                {
                    actionButton.SetActive(true);
                }
                else
                {
                    actionButton.SetActive(false);
                }
            }
            else if (abilitySelected.ability.targetType == Ability.TargetType.RANGE || abilitySelected.ability.targetType == Ability.TargetType.MELEE
                || abilitySelected.ability.crType == Ability.CristalAbilityType.ATTACK)
            {
                if (TutoCombat.tutoCombat.charSelected && TutoCombat.tutoCombat.charSelected.isTargetable)
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
        TutoCombat.tutoCombat.GoNextStepTuto();
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
                if (cm.charSelected.isTargetable)
                {
                    CristalAction(abi);
                }
            }
            else
            {
                if (cm.charSelected.isTargetable)
                {
                    CristalAction(abi);
                }
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
                    cm.allyPlaying.inDefenceMode = true; 
                    new StatusTuto1(cm.allyPlaying, 0, abi, StatusTuto1.StatusTypes.DEFENCE);
                    new StatusTuto1(cm.allyPlaying, Mathf.Round(cm.allyPlaying.armor * 0.4f), abi, StatusTuto1.StatusTypes.ARMORBONUS);
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
                        cm.allyPlaying.LaunchBuff(cm.charSelected, a);
                        break;
                    case Ability.CristalHealType.BATH:
                        AbilityBath(a);
                        break;
                    case Ability.CristalHealType.DRINK:
                        cm.allyPlaying.LaunchHeal(cm.charSelected, a);
                        cm.allyPlaying.LaunchBuff(cm.charSelected, a);
                        break;
                }
                break;
            case Ability.CristalAbilityType.ATTACK:
                switch (a.crAttackType)
                {
                    case Ability.CristalAttackType.NORMAL:
                        cm.allyPlaying.LaunchAttack(cm.charSelected, a);
                        break;
                    case Ability.CristalAttackType.DOT:
                        cm.allyPlaying.LaunchAttack(cm.charSelected, a);
                        cm.allyPlaying.PutDot(cm.charSelected, a);
                        break;
                    case Ability.CristalAttackType.MARK:
                        cm.allyPlaying.PutMark(cm.charSelected, a);
                        break;
                    case Ability.CristalAttackType.DESTRUCTION:
                        cm.allyPlaying.LaunchDestruction(cm.charSelected, a);
                        cm.allyPlaying.LaunchAttack(cm.charSelected, a);
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
                cm.allyPlaying.LaunchHeal(cm.charSelected, a);
                if (cm.allies.Count > 1)
                {
                    int randA = Random.Range(0, cm.allies.Count - 1);
                    while (cm.allies[randA] == cm.charSelected)
                    {
                        randA = Random.Range(0, cm.allies.Count - 1);
                    }
                    cm.allyPlaying.LaunchHeal(cm.allies[randA], a);
                }
                break;
            case Ability.ElementType.ICE:
                cm.allyPlaying.LaunchHeal(cm.charSelected, a);
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
                        cm.allyPlaying.abilitiesCristal[i] = AbilitiesTuto.abilitiesTuto.cristalsAsh[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsAsh.Length)];
                        while (cm.allyPlaying.abilitiesCristal[0] == cm.allyPlaying.abilitiesCristal[1])
                        {
                            cm.allyPlaying.abilitiesCristal[i] = AbilitiesTuto.abilitiesTuto.cristalsAsh[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsAsh.Length)];
                        }
                    }
                }
                break;
            case AllyTuto.ItemElement.ICE:
                for (int i = 0; i < cm.allyPlaying.abilitiesCristal.Length; i++)
                {
                    if (abilitySelected.ability == cm.allyPlaying.abilitiesCristal[i])
                    {
                        cm.allyPlaying.abilitiesCristal[i] = AbilitiesTuto.abilitiesTuto.cristalsIce[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsIce.Length)];
                        while (cm.allyPlaying.abilitiesCristal[0] == cm.allyPlaying.abilitiesCristal[1])
                        {
                            cm.allyPlaying.abilitiesCristal[i] = AbilitiesTuto.abilitiesTuto.cristalsIce[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsIce.Length)];
                        }
                    }
                }
                break;
            case AllyTuto.ItemElement.MUD:
                for (int i = 0; i < cm.allyPlaying.abilitiesCristal.Length; i++)
                {
                    if (abilitySelected.ability == cm.allyPlaying.abilitiesCristal[i])
                    {
                        cm.allyPlaying.abilitiesCristal[i] = AbilitiesTuto.abilitiesTuto.cristalsMud[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsMud.Length)];
                        while (cm.allyPlaying.abilitiesCristal[0] == cm.allyPlaying.abilitiesCristal[1])
                        {
                            cm.allyPlaying.abilitiesCristal[i] = AbilitiesTuto.abilitiesTuto.cristalsMud[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsMud.Length)];
                        }
                    }
                }
                break;
            case AllyTuto.ItemElement.PSY:
                for (int i = 0; i < cm.allyPlaying.abilitiesCristal.Length; i++)
                {
                    if (abilitySelected.ability == cm.allyPlaying.abilitiesCristal[i])
                    {
                        cm.allyPlaying.abilitiesCristal[i] = AbilitiesTuto.abilitiesTuto.cristalsPsy[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsPsy.Length)];
                        while (cm.allyPlaying.abilitiesCristal[0] == cm.allyPlaying.abilitiesCristal[1])
                        {
                            cm.allyPlaying.abilitiesCristal[i] = AbilitiesTuto.abilitiesTuto.cristalsPsy[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsPsy.Length)];
                        }
                    }
                }
                break;
        }
    }
}
