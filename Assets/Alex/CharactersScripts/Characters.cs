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
    public float speedSlider;
    public float healReceivedModif = 1.0f;
    public float bleedingDmg;
    public float armorModif = 0.3f;
    public float dodgeModif = 0.2f;

    [Header("StatusVariables")]
    public bool stunned;
    public bool confusion;
    public int turnsConfusionValue = 2;
    public int turnsConfusion;

    [Header("DotDamages")]
    public float dmgDotAsh;
    public float dmgDotIce;
    public float dmgDotMud;
    public float dmgDotPsy;

    [Header("CombatVariables")]
    public GameObject floatingHealth;
    public bool inDefenceMode = false;
    public bool isTargetable;
    public bool isMelee;
    public bool isDead;
    public bool hasPlayed;
    public bool CanAttack;
    public bool isSelected;
    [HideInInspector]
    public SpriteRenderer thisColorHead;
    public SpriteRenderer thisColorBody;
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
            thisColorBody.color = selectedColor;
            thisColorHead.color = selectedColor;
        }
        else if (!isSelected && !CanAttack && !hasPlayed)
        {
            thisColorBody.color = baseColor;
            thisColorHead.color = baseColor;
        }
        else if (CanAttack)
        {
            thisColorBody.color = AttackColor;
            thisColorHead.color = AttackColor;
        } else if(CanAttack && isSelected)
        {
            thisColorBody.color = selectedColor;
            thisColorHead.color = selectedColor;
        } else if (hasPlayed && !isSelected)
        {
            thisColorBody.color = hasPlayedColor;
            thisColorHead.color = hasPlayedColor;
        }
        else if (hasPlayed && isSelected)
        {
            thisColorBody.color = selectedColor;
            thisColorHead.color = selectedColor;
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
    //---------------------------SIMPLE ATTACK FUNCTION----------------------------
    public void LaunchAttack(Characters receiver, Ability ability)
    {
        float dmg = Mathf.Round(Random.Range(damageRange.x, damageRange.y));
        dmg *= (ability.multiplicator / 100);
        //-CRITIC DAMAGE-
        if (Random.Range(0.0f, 1.0f) < this.critChance)
        {
            Debug.Log("CRITIQUE");
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

        switch (ability.elementType)
        {
            case Ability.ElementType.ASH:
                if (ability.crHealType == Ability.CristalHealType.BOOST)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.DAMAGEBONUS);
                }
                else if (ability.crHealType == Ability.CristalHealType.DRINK)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.DAMAGEBONUS);
                }
                break;
            case Ability.ElementType.ICE:
                if (ability.crHealType == Ability.CristalHealType.BOOST)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.CRITDAMAGEBONUS);
                }
                else if (ability.crHealType == Ability.CristalHealType.DRINK)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.CRITRATEBONUS);
                }
                break;
            case Ability.ElementType.MUD:
                if (ability.crHealType == Ability.CristalHealType.BOOST)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.ARMORBONUSFLAT);
                }
                else if (ability.crHealType == Ability.CristalHealType.DRINK)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.DODGEBONUSFLAT);
                }
                break;
            case Ability.ElementType.PSY:
                if (ability.crHealType == Ability.CristalHealType.BOOST)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.DODGEBONUSFLAT);
                }else if(ability.crHealType == Ability.CristalHealType.DRINK)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.CRITDAMAGEBONUS);
                }
                break;
        }
    }
    public void LaunchDebuff(Characters receiver, Ability ability)
    {

    }
    public void LaunchDestruction(Characters receiver, Ability ability)
    {
        switch (ability.elementType)
        {
            case Ability.ElementType.ASH:
                Status s0 = new Status(receiver, ability.destruModif, ability.turnDuration, Status.StatusTypes.ARMORMALUS);
                Status s00 = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.DAMAGEMALUS);
                break;
            case Ability.ElementType.ICE:
                Status s1 = new Status(receiver, ability.destruModif, ability.turnDuration, Status.StatusTypes.ARMORMALUS);
                Status s01 = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.DODGEMALUS);
                break;
            case Ability.ElementType.MUD:
                Status s2 = new Status(receiver, ability.destruModif, ability.turnDuration, Status.StatusTypes.ARMORMALUS);
                Status s02 = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.HEALTHDEBUFF);
                break;
            case Ability.ElementType.PSY:
                Status s3 = new Status(receiver, ability.destruModif, ability.turnDuration, Status.StatusTypes.ARMORMALUS);
                Status s03 = new Status(receiver, ability.bonusmalus, ability.turnDuration, Status.StatusTypes.ARMORMALUS);
                break;
        }
    }
    public void PutDot(Characters receiver, Ability ability)
    {
        float dmg = Mathf.Round(Random.Range(damageRange.x, damageRange.y));
        switch (ability.elementType)
        {
            case Ability.ElementType.ASH:
                dmg *= (ability.dotMult / 100);
                Status s0 = new Status(receiver, dmg, ability.turnDuration, Status.StatusTypes.DOTASH);
                break;
            case Ability.ElementType.ICE:
                dmg *= (ability.dotMult / 100);
                Status s1 = new Status(receiver, dmg, ability.turnDuration, Status.StatusTypes.DOTICE);
                break;
            case Ability.ElementType.MUD:
                dmg *= (ability.dotMult / 100);
                Status s2 = new Status(receiver, dmg, ability.turnDuration, Status.StatusTypes.DOTMUD);
                break;
            case Ability.ElementType.PSY:
                dmg *= (ability.dotMult / 100);
                Status s3 = new Status(receiver, dmg, ability.turnDuration, Status.StatusTypes.DOTPSY);
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
                    Status s = new Status(this, 0.3f, 2, Status.StatusTypes.ARMORMALUS);
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.MUD)
                {
                    Debug.Log("Nothing happened..");
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.PSY)
                {
                    Status s = new Status(this, 0.3f, 2, Status.StatusTypes.ARMORBONUSPERC);
                    currentElement = CurrentElement.BASE;
                }
                break;
            case CurrentElement.MUD:
                if (ndElement == CurrentElement.ASH)
                {
                    Status s = new Status(this, 0.5f, 2, Status.StatusTypes.HEALBONUS);
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.ICE)
                {
                    Debug.Log("Nothing happened..");
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.PSY)
                {
                    Status s = new Status(this, 5.0f, 2, Status.StatusTypes.BLEEDING);
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
                    Status s = new Status(this, 0.5f, 2, Status.StatusTypes.PRECISIONMALUS);
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.MUD)
                {
                    Status s = new Status(this, 5.0f, 2, Status.StatusTypes.DODGEBONUSFLAT);
                    currentElement = CurrentElement.BASE;
                }
                break;
        }
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
        if(dmgDotAsh > 0)
        {
            TakeDamage(dmgDotAsh, 0.3f);
            ElementReactions(CurrentElement.ASH);
        }
        if (dmgDotIce > 0)
        {
            TakeDamage(dmgDotIce, 0.3f);
            ElementReactions(CurrentElement.ICE);
        }
        if(dmgDotMud > 0)
        {
            TakeDamage(dmgDotMud, 0.3f);
            ElementReactions(CurrentElement.MUD);
        }
        if(dmgDotPsy > 0)
        {
            TakeDamage(dmgDotPsy, 0.3f);
            ElementReactions(CurrentElement.PSY);
        }
        if(bleedingDmg > 0)
        {
            TakeDamage(bleedingDmg, 0.3f);
        }
        UpdateDisplayDots();
    }
    public void UpdateDisplayDots()
    {

    }

    public void ShowFloatingHealth(float value, bool red)
    {
        GameObject go = Instantiate(floatingHealth, transform.position, Quaternion.identity, transform);
        if (red)
        {
            go.GetComponentInChildren<TextMesh>().color = Color.red;
        }
        else
        {
            go.GetComponentInChildren<TextMesh>().color = Color.green;
        }
        go.GetComponentInChildren<TextMesh>().text = "" + value;
        go.GetComponentInChildren<FloatingHealthScript>().StartCoroutine(go.GetComponentInChildren<FloatingHealthScript>().AnimateFloatingTextCor(go.GetComponentInChildren<FloatingHealthScript>().destroyDelay));
    }
    public virtual void TakeDamage(float value, float duration)
    {
    }
    /*
    public virtual void TakeDamage(float value, float duration) 
    {
        StartCoroutine(TakeDamageCor(value, duration));
    }
    public virtual IEnumerator TakeDamageCor(float value, float duration)
    {
        yield return null;
    }
    */
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
