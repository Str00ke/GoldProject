using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ally : Characters
{
    private void Awake()
    {
        CombatManager.combatManager.allies.Add(this);
        charType = CharType.ALLY;
        anim = this.GetComponent<Animator>();
        thisColor = this.GetComponent<SpriteRenderer>();
        durationMove = 1.0f;
        healthBar = this.GetComponentInChildren<Slider>();
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        dodge = dodgeValue;
        armor = armorValue;
        durationDecreaseHealth = 1.5f;
        posInitial = GameObject.Find("Pos00").transform;
        ChangePos();


    }

    private void Start()
    {
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
            if (isSelected)
            {
                if (CombatManager.combatManager.allySelected == this)
                {
                    isSelected = false;
                    CombatManager.combatManager.allySelected = null;
                }
            }
            else if (!isSelected)
            {
                if (CombatManager.combatManager.allySelected != null)
                {
                    CombatManager.combatManager.allySelected.isSelected = false;
                    CombatManager.combatManager.allySelected = null;
                }
                isSelected = true;
                CombatManager.combatManager.allySelected = this;
            }
    }
    public override IEnumerator TakeDamageCor(float value, float duration)
    {
        float startValue = healthBar.value;
        float endValue = startValue - value;
        endValue = Mathf.Round(endValue);
        float elapsed = 0.0f;
        float ratio = 0.0f;
        /*if (health <= 0)
        {
            isDead = true;
            isTargetable = false;
        }*/
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            healthBar.value = Mathf.Lerp(startValue, endValue, ratio);
            health = Mathf.Round(healthBar.value);
            if (healthBar.value <= 0)
            {
                health = 0;
                break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        healthBar.value = endValue;
        if (health <= 0)
        {
            isDead = true;
            isTargetable = false;
            health = 0;
            CombatManager.combatManager.RemoveAlly(this);
            healthBar.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(durationDecreaseHealth);
    }

    public override void TakeHealing(float value, float duration)
    {
        StartCoroutine(TakeHealingCor(value, duration));
    }
    public override IEnumerator TakeHealingCor(float value, float duration)
    {
        var startValue = healthBar.value;
        value *= healReceivedModif;
        var endValue = startValue + value;
        endValue = Mathf.Round(endValue);
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
