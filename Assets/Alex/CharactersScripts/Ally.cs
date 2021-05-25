using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ally : Characters
{
    public enum CristalElement
    {
        ASH,
        ICE,
        MUD,
        PSY
    }
    public CristalElement cristalElement;
    public SpriteRenderer head;
    public SpriteRenderer body;
    private void Start()
    {
        CombatManager.combatManager.allies.Add(this);
        posInitial = GameObject.Find("Pos00").transform;
        charType = CharType.ALLY;
        anim = this.GetComponent<Animator>();
        thisColorBody = body;
        thisColorHead = head;
        durationMove = 1.0f;
        healthBar = GameObject.Find(gameObject.name + "/CanvasChar/healthBar").GetComponent<Slider>();
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        dodge = dodgeValue;
        armor = armorValue;
        durationDecreaseHealth = 0.3f;
        CreateChar("Char" + teamPosition);
        ChangePos();
        //ISTARGETABLE FOR ABILITIES
        isTargetable = false;
        healthBarOutline = GameObject.Find(gameObject.name + "/CanvasChar/HealthBarOutline");
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
        damageRange = new Vector2(Random.Range(5, 8), Random.Range(9, 13));
        dodge = Random.Range(5, 25);
        initiative = Random.Range(1, 14);
        critChance = Random.Range(0.1f, 0.25f);
        critDamage = Random.Range(0.75f, 1);
        armor = Random.Range(10, 25);
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        offsetPos *= -1;


        switch (itemElement)
        {
            case ItemElement.ASH:
                while (abilitiesCristal[0] == abilitiesCristal[1])
                {
                    abilitiesCristal[0] = AbilitiesManager.abilitiesManager.cristalsAsh[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsAsh.Length)];
                    abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsAsh[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsAsh.Length)];
                }
                break;
            case ItemElement.ICE:
                while (abilitiesCristal[0] == abilitiesCristal[1])
                {
                    abilitiesCristal[0] = AbilitiesManager.abilitiesManager.cristalsIce[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsIce.Length)];
                    abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsIce[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsIce.Length)];
                }
                break;
            case ItemElement.MUD:
                while (abilitiesCristal[0] == abilitiesCristal[1])
                {
                    abilitiesCristal[0] = AbilitiesManager.abilitiesManager.cristalsMud[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsMud.Length)];
                    abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsMud[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsMud.Length)];
                }
                break;
            case ItemElement.PSY:
                while (abilitiesCristal[0] == abilitiesCristal[1])
                {
                    abilitiesCristal[0] = AbilitiesManager.abilitiesManager.cristalsPsy[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsPsy.Length)];
                    abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsPsy[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsPsy.Length)];
                }
                break;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Selected");
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


    /*public override void TakeDamage(float value, float duration)
    {
        ShowFloatingHealth(Mathf.Round(value), true);
        float startValue = health;
        float endValue = startValue - value;
        endValue = Mathf.Round(endValue);
        health = endValue;
        if (health <= 0)
        {
            CombatManager.combatManager.RemoveAlly(this);
        }
        while (healthBar.value < health)
        {
            healthBar.value -= Time.deltaTime;
            if (healthBar.value <= 0)
            {
                health = 0;
                break;
            }
        }
        healthBar.value = endValue;
        if (health <= 0)
        {
            Death();
        }
    }*/
    
    public override IEnumerator TakeDamageCor(float value, float duration)
    {
        ShowFloatingHealth(Mathf.Round(value), true);
        float startValue = healthBar.value;
        float endValue = startValue - value;
        endValue = Mathf.Round(endValue);
        float elapsed = 0.0f;
        float ratio = 0.0f;
        health = endValue;
        if (health <= 0)
        {
            CombatManager.combatManager.RemoveAlly(this);
        }
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
        if (health <= 0)
        {
            isDead = true;
            isTargetable = false;
            health = 0;
            healthBar.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(durationDecreaseHealth);
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
        health = endValue;
        yield return new WaitForSeconds(durationDecreaseHealth);
    }
    public void Death()
    {
        isMelee = false;
        isDead = true;
        isTargetable = false;
        health = 0;
        healthBar.gameObject.SetActive(false);
        body.sprite = head.sprite;
        head.sprite = null;
    }
}
