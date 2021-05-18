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
        charType = CharType.ENEMY; 
        anim = this.GetComponent<Animator>();
        thisColor = this.GetComponent<SpriteRenderer>();
        durationMove = 1.0f;
        healthBar = this.GetComponentInChildren<Slider>();
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        initiative = Random.Range(1, 14);
        healthBar.value = health;
        durationDecreaseHealth = 1.5f;
        posInitial = GameObject.Find("Pos00Enemy").transform;
        ChangePos();

        //ISTARGETABLE FOR ABILITIES
        isTargetable = false;
        healthBarOutline = GameObject.Find("HealthBarOutline");
        healthBarOutline.SetActive(false);
        UpdateMeleeState();
    }

    // Update is called once per frame
    void Update()
    {
        IsTargetable();
        ChangeColor();
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
    }
    public override IEnumerator TakeDamageCor(float value, float duration)
    {
        var startValue = healthBar.value;
        var endValue = startValue - value;
        float elapsed = 0.0f;
        float ratio = 0.0f;

        if (health <= 0)
        {
            //CombatManager.combatManager.fightersList.Remove(this);
            isDead = true;
        }
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
            CombatManager.combatManager.RemoveEnemy(teamPosition);
        }
    }

}
