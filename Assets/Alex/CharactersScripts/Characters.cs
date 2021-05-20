using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Characters : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector]
    public GameObject healthBarOutline;
    [Header("Labels")]
    public string charName;
    [Header("Abilities")]
    public Ability[] abilities = new Ability[2];
    public Ability[] abilitiesCristal = new Ability[2];
    public enum CharType
    {
        ENEMY,
        ALLY
    }
    public CharType charType;

    public enum CurrentElement
    {
        BASE,
        MUD,
        ASH,
        ICE,
        PSY
    }
    public CurrentElement currentElement;

    [Header("Stats")]
    public float health;
    public float maxHealth;
    public Vector2 damageRange;
    public float initiative;
    [HideInInspector]
    public float dodge;
    public float dodgeValue;
    public float critChance;
    public float critDamage;
    [HideInInspector]
    public float armor;
    public float armorValue;
    [Range(0.0f, 1.0f)]
    public float precision = 1;
    [HideInInspector]
    public Slider healthBar;


    [Header("Modificators")]
    public float healReceivedModif = 1.0f;
    public float bleedingDmg;
    public float armorModif = 0.3f;
    public float dodgeModif = 0.2f;



    [Header("StatusVariables")]
    public bool hemorrhage;
    public int turnsHemoValue = 2;
    public int turnsHemo;

    public bool armorboost;
    public int turnsArmorboostValue = 2;
    public int turnsArmorboost;

    public bool armormalus;
    public int turnsArmormalusValue = 2;
    public int turnsArmormalus;

    public bool healbonus;
    public int turnsHealbonusValue = 2;
    public int turnsHealbonus;

    public bool stunned;
   // public int turnsStunnedValue = 2;
    public int turnsStunned;

    public bool precisionmalus;
    public int turnsPrecisionmalusValue = 2;
    public int turnsPrecisionmalus;

    public bool dodgeboost;
    public int turnsDodgeboostValue = 2;
    public int turnsDodgeboost;

    public bool confusion;
    public int turnsConfusionValue = 2;
    public int turnsConfusion;



    [Header("CombatVariables")]
    public bool inDefenceMode = false;
    public bool isTargetable;
    public bool isMelee;
    public bool isDead;
    public bool hasPlayed;
    public bool CanAttack;
    public bool isSelected;
    [HideInInspector]
    public SpriteRenderer thisColor;
    public Color selectedColor;
    public Color hasPlayedColor;
    public Color AttackColor;
    public Color baseColor;
    [HideInInspector]
    public float durationDecreaseHealth; //animation time in seconds

    [Header("Position")]
    public int teamPosition;
    public Transform posInitial;
    public float offsetPos = 1.0f;
    public float speedMove = 1.0f;
    public float durationMove = 1.0f;


    [Header("Animation")]
    public Animator anim;


    public void ChangePos()
    {
        UpdateMeleeState();
        StartCoroutine(ChangePosCoroutine(durationMove));
    }
    public void UpdateMeleeState()
    {
        if (teamPosition >= 1)
        {
            isMelee = false;
        }
        else
        {
            isMelee = true;
        }
    }
    public void ChangeColor()
    {
        if (isSelected && !CanAttack && !hasPlayed)
        {
            thisColor.color = selectedColor;
        }
        else if (!isSelected && !CanAttack && !hasPlayed)
        {
            thisColor.color = baseColor;
        }
        else if (CanAttack)
        {
            thisColor.color = AttackColor;
        } else if(CanAttack && isSelected)
        {
            thisColor.color = selectedColor;
        } else if (hasPlayed && !isSelected)
        {
            thisColor.color = hasPlayedColor;
        }
        else if (hasPlayed && isSelected)
        {
            thisColor.color = selectedColor;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData) 
    {
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
    }
    public void ChangeStats(string name, float maxHP, Vector2 dmgRange, int dodg, float critCh, float critDmg, int armr, int position)
    {
        charName = name;
        maxHealth = maxHP;
        damageRange = dmgRange;
        dodge = dodg;
        critChance = critCh;
        critDamage = critDmg;
        armor = armr;
        teamPosition = position;
    }
    public void StatusConsequences() 
    {
        if (hemorrhage)
        {
            Debug.Log("Bleeding");
        }
        if (healbonus) 
        {
            healReceivedModif = 1.5f;
        }
        if (armormalus)
        {
            armor = Mathf.Round(armorValue - armorValue * armorModif);
        }
        if (armorboost)
        {
            armor = Mathf.Round(armorValue + armorValue * armorModif);
        }
        if (precisionmalus)
        {
            precision = 0.5f;
        }
        if (dodgeboost)
        {
            dodge = Mathf.Round(dodgeValue + dodgeValue * dodgeModif);
        }
        if (confusion)
        {
            Debug.Log("Confus");
        }
    }
    public void ReduceStatusTurn() 
    {
        turnsArmorboost--;
        turnsArmormalus--;
        turnsConfusion--;
        turnsDodgeboost--;
        turnsHealbonus--;
        turnsHemo--;
        turnsPrecisionmalus--;
        StatusEnd();
    }
    public void StatusEnd() 
    {
        if(turnsArmorboost <= 0) 
        {
            armorboost = false;
            armor = armorValue;
        }
        if (turnsArmormalus <= 0)
        {
            armormalus = false;
            armor = armorValue;
        }
        if (turnsConfusion <= 0)
        {
            confusion = false;
        }
        if (turnsDodgeboost <= 0)
        {
            dodge = dodgeValue;
        }
        if (turnsHealbonus <= 0)
        {
            healReceivedModif = 1.0f;
        }
        if (turnsHemo <= 0)
        {
            hemorrhage = false;
        }
        if (turnsStunned <= 0)
        {
            stunned = false;
        }
        if (turnsPrecisionmalus <= 0)
        {
            precisionmalus = false;
            precision = 1.0f;
        }
    }
    //---------------------------SIMPLE ATTACK FUNCTION----------------------------
    public void LaunchAttack(Characters receiver, Ability ability)
    {
        float dmg = Mathf.Round(Random.Range(damageRange.x, damageRange.y));
        dmg *= (ability.multiplicator / 100);
        //-CRITIC DAMAGE-
        if (Random.Range(0.0f, 1.0f) < this.critChance)
        {
            dmg += dmg * this.critDamage;
        }
        //-ARMOR MODIF ON DAMAGE-
        dmg -= armor / 100;
        //-ELEMENTAL REACTIONS-
        receiver.TakeDamage(dmg, durationDecreaseHealth);
        receiver.ElementReactions((CurrentElement)System.Enum.Parse(typeof(CurrentElement), ability.elementType.ToString()));
    }
    //--------------------------------HEAL FUNCTION------------------------------------------------
    public void LaunchHeal(Characters receiver, Ability ability)
    {
        float healing = Mathf.Round(Random.Range(damageRange.x, damageRange.y));
        healing *= (ability.multiplicator / 100);
        healing *= healReceivedModif;
        healing = Mathf.Round(healing);
        receiver.ElementReactions((CurrentElement)System.Enum.Parse(typeof(CurrentElement), ability.elementType.ToString()));
        receiver.TakeHealing(healing, durationDecreaseHealth);
    }
    public void LaunchBuff(Characters receiver, Ability ability)
    {   
    }
    public void LaunchDebuff(Characters receiver, Ability ability)
    {

    }
    public void LaunchSpecial(Characters receiver, Ability ability) 
    {

    }
    public void LaunchDestruction(Characters receiver, Ability ability)
    {

    }
    public void PutDot(Characters receiver, Ability ability)
    {
        float dmg = Mathf.Round(Random.Range(damageRange.x, damageRange.y));
        switch (ability.elementType)
        {
            case Ability.ElementType.ASH:
                dmg *= (ability.multiplicator / 100);
                //ADD DOT
                break;
            case Ability.ElementType.ICE:
                dmg *= (ability.multiplicator / 100);
                //ADD DOT
                break;
            case Ability.ElementType.MUD:
                dmg *= (ability.multiplicator / 100);
                //ADD DOT
                break;
            case Ability.ElementType.PSY:
                dmg *= (ability.multiplicator / 100);
                //ADD DOT
                break;
        }
    }
    public void ElementReactions(CurrentElement ndElement)
    {
        switch (currentElement)
        {
            case CurrentElement.BASE:
                currentElement = ndElement;
                break;
            case CurrentElement.ASH:
                if (ndElement == CurrentElement.ICE)
                {
                    stunned = true;
                    //turnsStunned = turnsStunnedValue;
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.MUD)
                {
                    confusion = true;
                    turnsConfusion = turnsConfusionValue;
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.PSY)
                {
                    Debug.Log("Nothing happened..");
                    currentElement = CurrentElement.BASE;
                }
                break;
            case CurrentElement.ICE:
                if (ndElement == CurrentElement.ASH)
                {
                    if (armorboost)
                    {
                        armorboost = false;
                        armor = armorValue;
                    }
                    else
                    {
                        armormalus = true;
                    }
                    turnsArmormalus = turnsArmormalusValue;
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.MUD)
                {
                    Debug.Log("Nothing happened..");
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.PSY)
                {
                    if (armormalus)
                    {
                        armormalus = false;
                        armor = armorValue;
                    }
                    else
                    {
                        armorboost = true;
                    }
                    turnsArmorboost = turnsArmorboostValue;
                    currentElement = CurrentElement.BASE;
                }
                break;
            case CurrentElement.MUD:
                if (ndElement == CurrentElement.ASH)
                {
                    healbonus = true;
                    turnsHealbonus = turnsHealbonusValue;
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.ICE)
                {
                    Debug.Log("Nothing happened..");
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.PSY)
                {
                    currentElement = CurrentElement.BASE;
                }
                break;
            case CurrentElement.PSY:
                if (ndElement == CurrentElement.ASH)
                {
                    currentElement = CurrentElement.BASE;
                    Debug.Log("Nothing happened..");
                }
                else if (ndElement == CurrentElement.ICE)
                {
                    precisionmalus = true;
                    turnsPrecisionmalus = turnsPrecisionmalusValue;
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.MUD)
                {
                    dodgeboost = true;
                    turnsDodgeboost = turnsDodgeboostValue;
                    currentElement = CurrentElement.BASE;
                }
                break;
        }
        StatusConsequences();
    }
    public virtual void TakeHealing(float value, float duration) 
    {
    }
    public virtual IEnumerator TakeHealingCor(float value, float duration)
    {
        yield return null;
    }

    public void TakeDamageDots()
    {
        /*if(turnsDotAsh > 0)
        {
            TakeDamage(dmgDotAsh, durationDecreaseHealth);
            ElementReactions(CurrentElement.ASH);
            turnsDotAsh--;
        }
        if (turnsDotIce > 0)
        {
            TakeDamage(dmgDotIce, durationDecreaseHealth);
            ElementReactions(CurrentElement.ICE);
            turnsDotIce--;
        }
        if(turnsDotMud > 0)
        {
            TakeDamage(dmgDotMud, durationDecreaseHealth);
            ElementReactions(CurrentElement.MUD);
            turnsDotMud--;
        }
        if(turnsDotPsy > 0)
        {
            TakeDamage(dmgDotPsy, durationDecreaseHealth);
            ElementReactions(CurrentElement.PSY);
            turnsDotPsy--;
        }
        UpdateDisplayDots();*/
    }
    public void UpdateDisplayDots()
    {

    }
    public virtual void TakeDamage(float value, float duration) 
    {
        StartCoroutine(TakeDamageCor(value, duration));
    }
    public virtual IEnumerator TakeDamageCor(float value, float duration)
    {
        yield return null;
    }
    IEnumerator ChangePosCoroutine(float duration)
    {
        Vector3 startPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        Vector3 endPos = new Vector3(posInitial.position.x + offsetPos * teamPosition, posInitial.position.y, posInitial.position.z);
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            this.transform.position = Vector3.Lerp(startPos, endPos, ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void IsTargetable()
    {
        if(isTargetable)
            healthBarOutline.SetActive(true);
        else
            healthBarOutline.SetActive(false);
    }
}
