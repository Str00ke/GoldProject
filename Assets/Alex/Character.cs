using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character : Characters
{

    private void Awake()
    {
        CombatManager.combatManager.chars.Add(this);
        charType = CharType.ALLY;
        anim = this.GetComponent<Animator>();
        thisColor = this.GetComponent<SpriteRenderer>();
        durationMove = 1.0f;
        healthBar = this.GetComponentInChildren<Slider>();
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        durationDecreaseHealth = 1.5f;
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
        initiative = Random.Range(1, 14);
        critChance = Random.Range(0.1f, 0.25f);
        critDamage = Random.Range(0.75f, 1);
        armor = Random.Range(10, 25);
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        offsetPos *= -1;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!hasPlayed && !isDead)
        {
            if (isSelected)
            {
                if (CombatManager.combatManager.charSelected == this)
                {
                    isSelected = false;
                    CombatManager.combatManager.charSelected = null;
                }
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
        }
    }
    public override IEnumerator TakeDamageCor(float value, float duration)
    {
        var startValue = healthBar.value;
        var endValue = startValue - value;
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            healthBar.value = Mathf.Lerp(startValue, endValue, ratio);
            if (healthBar.value <= 0)
            {
                break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        healthBar.value = endValue;
        if (health <= 0)
        {
            isDead = true;
            hasPlayed = true;
            CombatManager.combatManager.chars.Remove(this);
            if(CombatManager.combatManager.chars.Count <= 0) 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            healthBar.gameObject.SetActive(false);
            //Play death animation
        }
    }
}
