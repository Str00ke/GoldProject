using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Enemy : Characters
{
    void Start()
    {
        debuffsInitialPos = new Vector2(-70, -158);
        buffsInitialPos = new Vector2(70, -158);
        stateIcons = UIManager.uiManager.stateIcons;
        stateIcons = UIManager.uiManager.stateIcons;
        CombatManager.combatManager.enemies.Add(this);
        charType = CharType.ENEMY; 
        anim = this.GetComponent<Animator>();
        thisColorBody = this.GetComponent<SpriteRenderer>();
        thisColorHead = this.GetComponent<SpriteRenderer>();
        durationMove = 1.0f;
        healthBar = GameObject.Find(gameObject.name + "/CanvasSlider/healthBar").GetComponent<Slider>();
        canvasChar = GameObject.Find(gameObject.name + "/CanvasSlider");
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        dodge = dodgeValue;
        armor = armorValue;
        initiative = Random.Range(1, 14);
        healthBar.value = health;
        durationDecreaseHealth = 0.3f;
        ChangePos();

        //ISTARGETABLE FOR ABILITIES
        isTargetable = false;
        healthBarOutline = GameObject.Find(gameObject.name + "/CanvasSlider/HealthBarOutline");
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
    /*public override void TakeDamage(float value, float duration)
    {
        ShowFloatingHealth(Mathf.Round(value), true);
        float elapsed = 0;
        float ratio = 0.0f;
        float startValue = health;
        float endValue = startValue - value;
        endValue = Mathf.Round(endValue);
        health = endValue;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            healthBar.value = Mathf.Lerp(healthBar.value, endValue, ratio);
            if (healthBar.value <= 0)
            {
                health = 0;
                break;
            }
            elapsed += Time.deltaTime;
        }
        healthBar.value = endValue;
        if (health <= 0)
        {
            health = 0;
            CombatManager.combatManager.RemoveEnemy(teamPosition);
        }
    }*/
    
    public override IEnumerator TakeDamageCor(float value, float duration)
    {
        ShowFloatingHealth(Mathf.Round(value), true);
        float startValue = health;
        float endValue = startValue - value;
        endValue = Mathf.Round(endValue);
        float elapsed = 0.0f;
        float ratio = 0.0f;
        health = endValue;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            healthBar.value = Mathf.Lerp(startValue, endValue, ratio);
            if (healthBar.value <= 0)
            {
                health = 0;
                break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        healthBar.value = endValue;
        yield return new WaitForSeconds(durationDecreaseHealth);
        if (health <= 0)
        {
            health = 0;
            CombatManager.combatManager.RemoveEnemy(teamPosition);
        }
    }

    
    public override void TakeHealing(float value, float duration)
    {
        ShowFloatingHealth(Mathf.Round(value), false);
        StartCoroutine(TakeHealingCor(value, duration));
    }
    public override IEnumerator TakeHealingCor(float value, float duration)
    {
        var startValue = healthBar.value;
        value *= healReceivedModif;
        var endValue = startValue + value;
        if (endValue > maxHealth)
            endValue = maxHealth;
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            healthBar.value = Mathf.Lerp(startValue, endValue, ratio);
            health = healthBar.value;
            if (healthBar.value >= maxHealth)
            {
                health = maxHealth;
                break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        healthBar.value = endValue;
        yield return new WaitForSeconds(durationDecreaseHealth);
    }
}
