using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Characters : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool onPointerHold;
    public GameObject healthBarOutline;
    public GameObject canvasChar;
    [Header("Labels")]
    public GameObject cursorNotPlayedYet;
    public GameObject cursorSelected;
    public GameObject cursorPlaying;
    public string charName;
    public Image stateIcon;
    public Sprite[] stateIcons;
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
        ICE,
        ASH,
        MUD,
        PSY
    }
    public CurrentElement currentElement;
    public enum ItemElement
    {
        BASE,
        ICE,
        ASH,
        MUD,
        PSY
    }
    public ItemElement itemElement;
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public Vector2 damageRange;
    public float initiative;
    public float dodge;
    public float dodgeValue;
    public float critChance;
    public float critDamage;
    public float armor;
    public float armorValue;
    [Range(0.0f, 1.0f)]
    public float precision = 1;
    public Slider healthBar;

    [Header("Modificators")]
    public float speedSlider;
    public float healReceivedModif = 1.0f;
    public float bleedingDmg;
    public float armorModif = 0.3f;
    public float dodgeModif = 0.2f;

    [Header("StatusVariables")]
    public GameObject statusLayoutGroup;
    public List<Status> statusList = new List<Status>();
    public Vector2 debuffsInitialPos = new Vector2(-40, -10.5f);
    public List<GameObject> prefabsIconStatus;
    public int statusPerLine = 0;
    public int statusPerLineMax = 5;
    public int statusLines = 0; 
    public bool stunned;
    public bool confusion;
    public int turnsConfusionValue = 2;
    public int turnsConfusion;
    public float holdCharacValue;
    public float holdCharac;

    [Header("Status Attributes")]
    public float armorBonus = 0;
    public float precisionMalus = 0;
    public float healBonus = 0;
    public float dodgeBonus = 0;
    public float critDamageBonus = 0;
    public float critChanceBonus = 0;
    public float damageBonus = 0;
    public float healthDebuff = 0;
    public int dotDamage = 0;
    public int markDamage = 0;




    [Header("CombatVariables")]
    public GameObject floatingHealth;
    public bool inDefenceMode = false;
    public bool isTargetable;
    public bool isMelee;
    public bool isDead;
    public bool hasPlayed;
    public bool CanAttack;
    public bool isSelected;
    public bool canBeSelected = true;
    [HideInInspector]
    public float durationDecreaseHealth; //animation time in seconds

    [Header("Position")]
    public int teamPosition;
    public Vector2 posInitial;
    public float offsetPos = 1.0f;
    public float speedMove = 1.0f;
    public float durationMove = 1.0f;


    [Header("Animation")]
    public Animator anim;
    protected Characters charToAttack;
    protected float dmgToDeal;


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
    public void InteractiveHoldToSelect()
    {
        if (canBeSelected)
        {
            if (holdCharac < holdCharacValue && onPointerHold)
            {
                transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime / 5, transform.localScale.y - Time.deltaTime / 5, transform.localScale.z - Time.deltaTime / 5);
            }
            else if (holdCharac > holdCharacValue || !onPointerHold)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    public void ChangeColor()
    {
        if (isSelected)
        {
            GetComponentInChildren<CursorEffectsScript>().ActivateCursor(cursorSelected);
            //cursorSelected.SetActive(true);
        }
        else
        {
            //cursorSelected.SetActive(false);
            GetComponentInChildren<CursorEffectsScript>().DeactivateCursor(cursorSelected);
        }
        //Cursor playing display in NextCharAttack in CombatManager
    }
    public void UpdateStateIcon()
    {
        switch (currentElement)
        {
            case CurrentElement.BASE:
                stateIcon.sprite = UIManager.uiManager.stateIcons[0]; ;
                break;
            case CurrentElement.ICE:
                stateIcon.sprite = UIManager.uiManager.stateIcons[1];
                break;
            case CurrentElement.ASH:
                stateIcon.sprite = UIManager.uiManager.stateIcons[2];
                break;
            case CurrentElement.MUD:
                stateIcon.sprite = UIManager.uiManager.stateIcons[3];
                break;
            case CurrentElement.PSY:
                stateIcon.sprite = UIManager.uiManager.stateIcons[4];
                break;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (canBeSelected)
        {
            onPointerHold = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (canBeSelected)
            onPointerHold = false;
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
        if(Random.Range(0,100) < receiver.dodge)
        {
            receiver.ShowFloatingHealth("Dodge", true);
        }
        else
        {
            //-CRITIC DAMAGE-
            if (Random.Range(0.0f, 1.0f) < this.critChance)
            {
                FindObjectOfType<CameraScript>().CamShake(0.4f, 0.3f);
                dmg += dmg * this.critDamage;
            }
            else
            {
                FindObjectOfType<CameraScript>().CamShake(0.2f, 0.05f);
            }

            //-ARMOR MODIF ON DAMAGE-
            dmg -= receiver.armor;
            dmg = dmg < 0 ? 0 : dmg;

            //-ELEMENTAL REACTIONS-
           
            receiver.TakeDamage(dmg, durationDecreaseHealth);

            receiver.ElementReactions((CurrentElement)System.Enum.Parse(typeof(CurrentElement), ability.elementType.ToString()));

            if (receiver.GetType() == typeof(Enemy))
            {
                AchievementsManager.DamageDeal(dmg);
            }
        }
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
                Status s0 = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.DamageBonus);
                break;
            case Ability.ElementType.ICE:

                if (ability.crHealType == Ability.CristalHealType.BOOST)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.CriticDamageBonus);
                }
                else if (ability.crHealType == Ability.CristalHealType.DRINK)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.CriticRateBonus);
                }
                break;
            case Ability.ElementType.MUD:
                if (ability.crHealType == Ability.CristalHealType.BOOST)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.ArmorBonus);
                }
                else if (ability.crHealType == Ability.CristalHealType.DRINK)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.DodgeBonus);
                }
                break;
            case Ability.ElementType.PSY:
                if (ability.crHealType == Ability.CristalHealType.BOOST)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.DodgeBonus);
                }
                else if (ability.crHealType == Ability.CristalHealType.DRINK)
                {
                    Status s = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.CriticDamageBonus);
                }
                break;
        }
        receiver.ElementReactions((CurrentElement)System.Enum.Parse(typeof(CurrentElement), ability.elementType.ToString()));
    }
    public void LaunchDestruction(Characters receiver, Ability ability)
    {
        switch (ability.elementType)
        {
            case Ability.ElementType.ASH:
                Status s0 = new Status(receiver, ability.destruModif, ability, Status.StatusTypes.ArmorMalus);
                Status s00 = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.DamageMalus);
                break;
            case Ability.ElementType.ICE:
                Status s1 = new Status(receiver, ability.destruModif, ability, Status.StatusTypes.ArmorMalus);
                Status s01 = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.DodgeMalus);
                break;
            case Ability.ElementType.MUD:
                Status s2 = new Status(receiver, ability.destruModif, ability, Status.StatusTypes.ArmorMalus);
                Status s02 = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.HealthDebuff);
                break;
            case Ability.ElementType.PSY:
                Status s3 = new Status(receiver, ability.destruModif, ability, Status.StatusTypes.ArmorMalus);
                Status s03 = new Status(receiver, ability.bonusmalus, ability, Status.StatusTypes.ArmorMalus);
                break;
        }
    }
    public void PutDot(Characters receiver, Ability ability)
    {
        float dmg = Mathf.Round(Random.Range(damageRange.x, damageRange.y));
        dmg *= (ability.dotMult / 100);
        Status s = new Status(receiver, this, dmg, ability, Status.StatusTypes.Dot, dmg);
        Debug.Log("Caster " + gameObject.name + "Receiver : " + s.statusTarget.gameObject.name + "Dot damage " + s.dmg + " Dot element " + s.statusElement.ToString());
    }
    public void PutMark(Characters receiver, Ability ability)
    {
        float dmg = Mathf.Round(Random.Range(damageRange.x, damageRange.y));
        dmg *= (ability.markMult / 100);
        Status s = new Status(receiver, this, dmg, ability, Status.StatusTypes.Mark, dmg);
        Debug.Log("Caster " + gameObject.name + "Receiver : " + s.statusTarget.gameObject.name + "Mark damage " + s.dmg + " Mark element " + s.statusElement.ToString());
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
                    Status s = null;
                    foreach (Status s1 in statusList)
                    {
                        if (s1.statusType == Status.StatusTypes.Defence)
                            s1.RevertStatus();
                        else if (s1.statusType == Status.StatusTypes.Stun)
                        {
                            s = s1;
                            s1.turnsActive = 1;
                        }
                    }
                    if (s == null)
                        s = new Status(this, 0, 1, Status.StatusTypes.Stun);
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
                    Status s = new Status(this, 0.3f, 2, Status.StatusTypes.ArmorMalus);
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.MUD)
                {
                    Debug.Log("Nothing happened..");
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.PSY)
                {
                    Status s = new Status(this, 10, 2, Status.StatusTypes.ArmorBonus);
                    currentElement = CurrentElement.BASE;
                }
                break;
            case CurrentElement.MUD:
                if (ndElement == CurrentElement.ASH)
                {
                    Status s = new Status(this, 0.5f, 2, Status.StatusTypes.HealBonus);
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.ICE)
                {
                    Debug.Log("Nothing happened..");
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.PSY)
                {
                    Status s = new Status(this, 5.0f, 2, Status.StatusTypes.Bleeding);
                    Debug.Log("Put bleeding");
                    s.statusElement = Status.StatusElement.BASE;
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
                    Status s = new Status(this, 0.5f, 2, Status.StatusTypes.PrecisionMalus);
                    currentElement = CurrentElement.BASE;
                }
                else if (ndElement == CurrentElement.MUD)
                {
                    Status s = new Status(this, 5.0f, 2, Status.StatusTypes.DodgeBonus);
                    currentElement = CurrentElement.BASE;
                }
                break;
        }
        UpdateStateIcon();
    }
    public virtual void TakeHealing(float value, float duration)
    {
    }
    public virtual IEnumerator TakeHealingCor(float value, float duration)
    {
        yield return null;
    }

    public void TakeDamageDots(Status.StatusElement stElem, float dmg)
    {
        TakeDamage(dmg, durationDecreaseHealth);
        Debug.Log("Receiver : " + gameObject.name + "Dot damage " + dmg + " Dot element " + stElem.ToString());
        ElementReactions((CurrentElement)stElem);
    }
    public void TakeDamageMark(Status.StatusElement stElem, float dmg)
    {
        TakeDamage(dmg, durationDecreaseHealth);
        Debug.Log("Receiver : " + gameObject.name + "Mark damage " + dmg + " Mark element " + stElem.ToString());
        ElementReactions((CurrentElement)System.Enum.Parse(typeof(CurrentElement), stElem.ToString()));
    }

    public void ShowFloatingHealth(string value, bool red)
    {
        GameObject go = Instantiate(floatingHealth, transform.position, Quaternion.identity);
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
        StartCoroutine(TakeDamageCor(value, duration));
    }
    public virtual IEnumerator TakeDamageCor(float value, float duration)
    {
        yield return null;
    }

    IEnumerator ChangePosCoroutine(float duration)
    {
        Vector3 startPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        Vector3 endPos = new Vector3(posInitial.x + offsetPos * teamPosition, posInitial.y);
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
        if (isTargetable)
            healthBarOutline.SetActive(true);
        else
            healthBarOutline.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + (Vector3)debuffsInitialPos, new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube((Vector3)posInitial, new Vector3(0.1f, 0.1f, 0.1f));

    }
}
