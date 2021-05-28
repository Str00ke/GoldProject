using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ally : Characters
{
    public SpriteRenderer head;
    public SpriteRenderer body;
    public SpriteRenderer bodyArmor;
    public SpriteRenderer helmet;
    public SpriteRenderer weapon;
    public float holdAllyCombo;
    private void Start()
    {
        holdAllyCombo = holdCharacValue*2;
        stateIcons = UIManager.uiManager.stateIcons;
        CombatManager.combatManager.allies.Add(this);
        charType = CharType.ALLY;
        anim = this.GetComponent<Animator>();
        thisColorBody = body;
        thisColorHead = head;
        durationMove = 1.0f;
        healthBar = GameObject.Find(gameObject.name + "/CanvasChar/healthBar").GetComponent<Slider>();
        canvasChar = GameObject.Find(gameObject.name + "/CanvasChar");
        durationDecreaseHealth = 1.0f;
        //ISTARGETABLE FOR ABILITIES
        isTargetable = false;
        healthBarOutline = GameObject.Find(gameObject.name + "/CanvasChar/HealthBarOutline");
        healthBarOutline.SetActive(false);
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
        if (holdCharac > holdCharacValue && holdCharac < holdAllyCombo)
        {
            if (!isSelected)
            {
                if (CombatManager.combatManager.allySelected != null)
                {
                    CombatManager.combatManager.allySelected.isSelected = false;
                    CombatManager.combatManager.allySelected = null;
                }
                isSelected = true;
                CombatManager.combatManager.allySelected = this;
            }
            UIManager.uiManager.allyStatsUI.SetActive(true);
        }
        if(!hasPlayed && holdCharac > holdAllyCombo && CombatManager.combatManager.allyPlaying != this)
        {
            CombatManager.combatManager.allyCombo = this;
        }
    }

    public void CreateChar(string name) 
    {
        charName = name;
        maxHealth = Random.Range(50,80);
        damageRange = new Vector2(Random.Range(10, 12), Random.Range(15, 18));
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
        onPointerHold = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("UP");
        UIManager.uiManager.ResetAllyDisplayUI();
        onPointerHold = false;
    }

    public void CreateChar(Character cs, int teamPos)
    {
        // charName = cs.charName;
        charName = "Char0" + teamPos;
        //Sprites
        head.sprite = cs.charHead;
        body.sprite = cs.itemSprites[0];
        helmet.sprite = cs.itemSprites[1];
        bodyArmor.sprite = cs.itemSprites[2];
        weapon.sprite = cs.itemSprites[3];
        teamPosition = teamPos;
        maxHealth = cs.maxHealth;
        damageRange = new Vector2(cs.attack - cs.attack * 0.1f, cs.attack + cs.attack * 0.1f);
        dodge = cs.dodge;
        //initiative = cs.initiative;
        initiative = Random.Range(1, 14);
        critChance = cs.criticalChance;
        critDamage = cs.crititalDamage;
        armor = cs.armor;
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        offsetPos *= -1;

        itemElement = (ItemElement)cs.GetItem(NItem.EPartType.Gem).itemType;

        switch (itemElement)
        {
            case ItemElement.ASH:
                abilitiesCristal[0] = AbilitiesManager.abilitiesManager.cristalsAsh[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsAsh.Length)];
                abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsAsh[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsAsh.Length)];
                while (abilitiesCristal[0] == abilitiesCristal[1])
                {
                    abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsAsh[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsAsh.Length)];
                }
                break;
            case ItemElement.ICE:
                abilitiesCristal[0] = AbilitiesManager.abilitiesManager.cristalsIce[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsIce.Length)];
                abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsIce[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsIce.Length)];
                while (abilitiesCristal[0] == abilitiesCristal[1])
                {
                    abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsIce[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsIce.Length)];
                }
                break;
            case ItemElement.MUD:
                abilitiesCristal[0] = AbilitiesManager.abilitiesManager.cristalsMud[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsMud.Length)];
                abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsMud[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsMud.Length)];
                while (abilitiesCristal[0] == abilitiesCristal[1])
                {
                    abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsMud[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsMud.Length)];
                }
                break;
            case ItemElement.PSY:
                abilitiesCristal[0] = AbilitiesManager.abilitiesManager.cristalsPsy[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsPsy.Length)];
                abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsPsy[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsPsy.Length)];
                while (abilitiesCristal[0] == abilitiesCristal[1])
                {
                    abilitiesCristal[1] = AbilitiesManager.abilitiesManager.cristalsPsy[Random.Range(0, AbilitiesManager.abilitiesManager.cristalsPsy.Length)];
                }
                break;
        }
    }

    public override IEnumerator TakeDamageCor(float value, float duration)
    {
        ShowFloatingHealth(Mathf.Round(value).ToString(), true);
        float startValue = healthBar.value;
        float endValue = startValue - value;
        endValue = Mathf.Round(endValue);
        healthBar.value = endValue;
        health = endValue;
        if (health <= 0)
        {
            CombatManager.combatManager.RemoveAlly(this);
        }
        yield return new WaitForSeconds(duration);
        GetComponentInChildren<DamagedBarScript>().UpdateDamagedBar(endValue, duration, false);
        yield return new WaitForSeconds(duration);
        if (health <= 0)
        {
            isDead = true;
            isTargetable = false;
            health = 0;
            healthBar.gameObject.SetActive(false);
        }
        yield return null;
        /*float startValue = healthBar.value;
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
        yield return new WaitForSeconds(duration * 2);*/
    }
    
    public override void TakeHealing(float value, float duration)
    {
        ShowFloatingHealth(Mathf.Round(value).ToString(), false);
        StartCoroutine(TakeHealingCor(value, duration));
    }   
    public override IEnumerator TakeHealingCor(float value, float duration)
    {
        var startValue = healthBar.value;
        value *= healReceivedModif;
        var endValue = startValue + value;
        GetComponentInChildren<DamagedBarScript>().UpdateDamagedBar(endValue, duration, true);
        yield return new WaitForSeconds(duration);
        endValue = Mathf.Round(endValue);
        if (endValue >= maxHealth)
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
        Character c = CharacterManager.characterManager.AskForCharacter(teamPosition);
        c.RemoveItem((NItem.EPartType)3);
        c.RemoveItem((NItem.EPartType)Random.Range(0,3));
        for(int i = 0; i < 3; i++)
        {
            NItem.ItemScriptableObject item = c.GetItem((NItem.EPartType)i);
            if (item)
            {
                Inventory.inventory.AddItem(item);
                c.RemoveItem((NItem.EPartType)i);
            }
        }
    }
}
