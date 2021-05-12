using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IPointerDownHandler
{
    [Header("Labels")]
    public string enemyName;
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public Vector2 damageRange;
    public int dodge;
    public float critChance;
    public float critDamage;
    public int armor;
    public Slider healthBar;


    [Header("CombatVariables")]
    public bool hasPlayed;
    public bool isSelected;
    public SpriteRenderer thisColor;
    public Color selectedColor;
    public Color highlightColor;
    public Color baseColor;
    public float durationDecreaseHealth; //animation time in seconds

    [Header("Position")]
    public int teamPosition;
    public Transform posInitial;
    public float offsetPos = 1.0f;
    public float speedMove = 1.0f;
    public float durationMove = 1.0f;


    void Start()
    {
        CombatManager.combatManager.enemies.Add(this);
        durationDecreaseHealth = 1.5f;
        durationMove = 1.0f;
        health = maxHealth;
        healthBar = this.GetComponentInChildren<Slider>();
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        thisColor = this.GetComponent<SpriteRenderer>();
        posInitial = GameObject.Find("Pos00Enemy").transform;
        ChangePos();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    public void ChangePos()
    {
        StartCoroutine(ChangePosCoroutine(durationMove));
    }
    public void ChangeColor() 
    {
        if (isSelected)
        {
            thisColor.color = selectedColor;
        }else if(!isSelected)
        {
            thisColor.color = baseColor;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSelected)
        {
            if (CombatManager.combatManager.enemySelected == this)
            {
                isSelected = false;
                CombatManager.combatManager.enemySelected = null;
            }
            Debug.Log("Lol");
        }
        else if (!isSelected)
        {
            if (CombatManager.combatManager.enemySelected != null)
            {
                CombatManager.combatManager.enemySelected.isSelected = false;
                CombatManager.combatManager.enemySelected = null;
            }
            isSelected = true;
            CombatManager.combatManager.enemySelected = this;
        }
        CombatManager.combatManager.ChangeTexts();
    }
    public void OnPointerUp(PointerEventData eventData) 
    {
    }

    public void ChangeStats(string name, float maxHP, Vector2 dmgRange, int dodg, float critCh, float critDmg, int armr, int position)
    {
        enemyName = name;
        maxHealth = maxHP;
        damageRange = dmgRange;
        dodge = dodg;
        critChance = critCh;
        critDamage = critDmg;
        armor = armr;
        teamPosition = position;
    }
    public void TakeDamage(float dmg) 
    {
        Debug.Log(dmg);
        dmg = dmg - armor / 100;
        health -= dmg;
        Debug.Log("Health" + health + "   dmg" + dmg);
        StartCoroutine(ReduceSlider(dmg, durationDecreaseHealth));
    }

    IEnumerator ReduceSlider(float value, float duration)
    {
        var startValue = healthBar.value;
        Debug.Log(healthBar.value);
        var endValue = startValue - value;
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            healthBar.value = Mathf.Lerp(startValue, endValue, ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
        healthBar.value = endValue;
        if (health <= 0 && elapsed >= duration)
        {
            CombatManager.combatManager.RemoveEnemy(teamPosition);
        }
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
