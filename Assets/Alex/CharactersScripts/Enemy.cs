using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Enemy : Characters
{
    public EEnemyType enemyType;
    public EElement enemyElement;
    bool removed;
    void Awake()
    {
        stateIcons = UIManager.uiManager.stateIcons;
        stateIcons = UIManager.uiManager.stateIcons;
        CombatManager.combatManager.enemies.Add(this);
        charType = CharType.ENEMY; 
        anim = this.GetComponent<Animator>();
        thisColorBody = this.GetComponent<SpriteRenderer>();
        thisColorHead = this.GetComponent<SpriteRenderer>();
        durationMove = 1.0f;
        durationDecreaseHealth = 1.0f;

        //ISTARGETABLE FOR ABILITIES
        isTargetable = false;
        UpdateMeleeState();
        UpdateStateIcon();
    }


    // Update is called once per frame
    void Update()
    {
        IsTargetable();
        ChangeColor();
        InteractiveHoldToSelect();
        if (onPointerHold)
        {
            holdCharac += Time.deltaTime;
        }
        else
        {
            holdCharac = 0;
        }
        if (holdCharac > holdCharacValue)
        {
            if (!isSelected)
            {
                if (CombatManager.combatManager.enemySelected != null)
                {
                    CombatManager.combatManager.enemySelected.isSelected = false;
                    CombatManager.combatManager.enemySelected = null;
                }
                isSelected = true;
                CombatManager.combatManager.enemySelected = this;
            }
            UIManager.uiManager.enemyStatsUI.SetActive(true);
        }

        if(!removed && isDead)
        {
            CombatManager.combatManager.RemoveEnemy(this);
            removed = true;
        }
    }

    public void CreateEnemy(Ennemy e, int teamPos, Level level, MapRoom mapRoom)
    {
        teamPosition = teamPos;
        charName = e.enemyName;
        gameObject.name = charName + teamPos;
        healthBar = GameObject.Find(gameObject.name + "/CanvasSlider/healthBar").GetComponent<Slider>();
        canvasChar = GameObject.Find(gameObject.name + "/CanvasSlider");
        healthBarOutline = GameObject.Find(gameObject.name + "/CanvasSlider/HealthBarOutline");
        healthBarOutline.SetActive(false);
        maxHealth = e.maxHealth;
        enemyType = e.enemyType;
        damageRange = e.damageRange;
        dodge = e.dodge * 100;
        critChance = e.critChance;
        critDamage = e.critDamage;
        initiative = e.initiative;
        armor = e.armor;
        enemyElement = e.element;
        abilities = e.enemyAbilities;
        abilitiesCristal = e.enemySpecialsAbilities;
        EnnemyManager._enemyManager.MultiplicateByValues(this, level, mapRoom);
        itemElement = (ItemElement)System.Enum.Parse(typeof(ItemElement), e.element.ToString());
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        onPointerHold = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        UIManager.uiManager.ResetEnemyDisplayUI();
        onPointerHold = false;
    }

    public override void TakeDamage(float value, float duration)
    {
        StartCoroutine(TakeDamageCor(value, duration));
    }
    public override IEnumerator TakeDamageCor(float value, float duration)
    {
        ShowFloatingHealth(Mathf.Round(value).ToString(), true);
        float startValue = health;
        float endValue = startValue - value;
        endValue = Mathf.Round(endValue);
        healthBar.value = endValue;
        health = endValue;
        yield return new WaitForSeconds(duration);
        GetComponentInChildren<DamagedBarScript>().UpdateDamagedBar(endValue, duration, false);
        yield return new WaitForSeconds(duration);
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }

    
    public override void TakeHealing(float value, float duration)
    {
        ShowFloatingHealth(Mathf.Round(value).ToString(), false);
        StartCoroutine(TakeHealingCor(value, duration));
    }
    public override IEnumerator TakeHealingCor(float value, float duration)
    {
        var startValue = healthBar.value;
        var endValue = startValue + value;
        GetComponentInChildren<DamagedBarScript>().UpdateDamagedBar(endValue, duration, true);
        value *= healReceivedModif;
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
