using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Enemy : Characters
{
    void Start()
    {
        CombatManager.combatManager.enemies.Add(this);
        anim = this.GetComponent<Animator>();
        thisColor = this.GetComponent<SpriteRenderer>();
        durationMove = 1.0f;
        healthBar = this.GetComponentInChildren<Slider>();
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        durationDecreaseHealth = 1.5f;
        posInitial = GameObject.Find("Pos00Enemy").transform;
        ChangePos();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    public override void ChangeColor()
    {
        if (isSelected && !isAttacking)
        {
            thisColor.color = selectedColor;
        }
        else if (!isSelected && !isAttacking)
        {
            thisColor.color = baseColor;
        }
        else if (isAttacking)
        {
            thisColor.color = hasPlayedColor;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (isSelected)
        {
            if (CombatManager.combatManager.enemySelected == this)
            {
                isSelected = false;
                CombatManager.combatManager.enemySelected = null;
            }
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

        ChangeColor();
        //CombatManager.combatManager.ChangeTexts();
    }
    public override IEnumerator TakeDamageCor(float value, float duration)
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
            CombatManager.combatManager.RemoveEnemy(teamPosition);
        }
        //CHECK AFTER ALLY ATTACK IF ALLIES HAVE ALL PLAYED
        if (CombatManager.combatManager.chars.Count > 0 && CombatManager.combatManager.nbCharsPlayed >= CombatManager.combatManager.chars.Count && !CombatManager.combatManager.enemAttacking)
        {
            CombatManager.combatManager.EnemiesAttack();
        }
    }

}
