using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character : MonoBehaviour, IPointerDownHandler
{
    [Header("Labels")]
    public string charName;
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
    public float durationDecreaseHealth;
    [Header("Position")]
    public int teamPosition;
    public float durationMove;
    public Transform posInitial;    
    public float offsetPos = 1.0f;
    public float speedMove = 1.0f;

    private void Awake()
    {
        CombatManager.combatManager.chars.Add(this);
        durationDecreaseHealth = 1.5f;
        durationMove = 1.0f;
        health = maxHealth;
        healthBar = this.GetComponentInChildren<Slider>();
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        thisColor = this.GetComponent<SpriteRenderer>();
        posInitial = GameObject.Find("Pos00").transform;
        ChangePos();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    public void CreateChar(string name, float maxHP, Vector2 dmgRange, int dodg, float critCh, float critDmg, int armr, int position)
    {
        charName = name;
        maxHealth = maxHP;
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        damageRange = dmgRange;
        dodge = dodg;
        critChance = critCh;
        critDamage = critDmg;
        armor = armr;
        teamPosition = position;
    }
    public void CreateChar(string name) 
    {
        charName = name;
        maxHealth = Random.Range(15,30);
        damageRange = new Vector2(Random.Range(5, 8), Random.Range(10, 12));
        dodge = Random.Range(5, 25);
        critChance = Random.Range(0.1f, 0.25f);
        critDamage = Random.Range(0.75f, 1);
        armor = Random.Range(10, 25);
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }

    public void ChangePos()
    {
        StartCoroutine(ChangePosCoroutine(durationMove));
    }

    public void AttackEnemy() 
    {

    }

    public void ChangeColor()
    {
        if (isSelected)
        {
            thisColor.color = selectedColor;
        }
        else if (!isSelected && !hasPlayed)
        {
            thisColor.color = baseColor;
        }
        else if (hasPlayed)
        {
            thisColor.color = highlightColor;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!hasPlayed)
        {
            if (isSelected)
            {
                if (CombatManager.combatManager.charSelected == this)
                {
                    isSelected = false;
                    CombatManager.combatManager.charSelected = null;
                }
                Debug.Log("Lol");
            }
            else if (!isSelected)
            {
                if (CombatManager.combatManager.charSelected != null)
                {
                    CombatManager.combatManager.charSelected.isSelected = false;
                    CombatManager.combatManager.charSelected = null;
                }
                isSelected = true;
                CombatManager.combatManager.charSelected = this;
            }
            CombatManager.combatManager.ChangeTexts();
        }
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
            StopCoroutine(ReduceSlider(value, duration));
        }
    }
    IEnumerator ChangePosCoroutine(float duration)
    {
        Vector3 startPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        Vector3 endPos = new Vector3(posInitial.position.x - offsetPos * teamPosition, posInitial.position.y, posInitial.position.z);
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
