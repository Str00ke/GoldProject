using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Characters : MonoBehaviour, IPointerDownHandler
{
    [Header("Labels")]
    public string charName;
    public enum CharType 
    {
        ENEMY,
        ALLY
    }
    public CharType charType;
    public enum WeaponType 
    {
        MELEE,
        RANGE
    }
    WeaponType weaponType;
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public Vector2 damageRange;
    public int initiative;
    public int dodge;
    public float critChance;
    public float critDamage;
    public int armor;
    public Slider healthBar;


    [Header("CombatVariables")]
    public bool isDead;
    public bool hasPlayed;
    public bool CanAttack;
    public bool isSelected;
    public SpriteRenderer thisColor;
    public Color selectedColor;
    public Color hasPlayedColor;
    public Color AttackColor;
    public Color baseColor;
    public float durationDecreaseHealth; //animation time in seconds

    [Header("Position")]
    public int teamPosition;
    public Transform posInitial;
    public float offsetPos = 1.0f;
    public float speedMove = 1.0f;
    public float durationMove = 1.0f;

    [Header("Animation")]
    public Animator anim;      
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangePos()
    {
        StartCoroutine(ChangePosCoroutine(durationMove));
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
        }else if (hasPlayed && !isSelected)
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
    //-------------------------------------------------ASSIGN ABILITIES------------------------------------------
    public void AssignAbilities() 
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
    public void TakeDamageFrom(Characters attacker)
    {
        float dmg = Mathf.Round(Random.Range(attacker.damageRange.x, attacker.damageRange.y));
        dmg = dmg - armor / 100;
        health -= dmg;
        Debug.Log(this.charName + "Health" + health + "   dmg" + dmg);
        StartCoroutine(TakeDamageCor(dmg, durationDecreaseHealth));
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
}
