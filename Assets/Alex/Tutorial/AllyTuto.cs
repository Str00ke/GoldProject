using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AllyTuto : CharactersTuto
{
    public Sprite weaponSpriteBase;
    public Sprite weaponSpriteAnim;
    public float holdAllyCombo;
    private void Start()
    {
        holdAllyCombo = holdCharacValue * 2;
        stateIcons = UITuto.uiTuto.stateIcons;
        TutoCombat.tutoCombat.allies.Add(this);
        charType = CharType.ALLY;
        anim = this.GetComponent<Animator>();
        durationMove = 1.0f;
        cursorNotPlayedYet = GameObject.Find(gameObject.name + "/CanvasChar/cursorNotPlayedYet");
        cursorSelected = GameObject.Find(gameObject.name + "/CanvasChar/cursorSelected");
        cursorPlaying = GameObject.Find(gameObject.name + "/CanvasChar/cursorPlaying");
        healthBar = GameObject.Find(gameObject.name + "/CanvasChar/healthBar").GetComponent<Slider>();
        canvasChar = GameObject.Find(gameObject.name + "/CanvasChar");
        cursorSelected.SetActive(false);
        cursorPlaying.SetActive(false);
        durationDecreaseHealth = 1.0f;
        //ISTARGETABLE FOR ABILITIES
        isTargetable = false;
        healthBarOutline = GameObject.Find(gameObject.name + "/CanvasChar/HealthBarOutline");
        healthBarOutline.SetActive(false);
        TutoCombat.tutoCombat.allies.Add(this);
        UpdateMeleeState();
        UpdateStateIcon();
        ChangePos();
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
                if (TutoCombat.tutoCombat.charSelected != null)
                {
                    TutoCombat.tutoCombat.charSelected.isSelected = false;
                    TutoCombat.tutoCombat.charSelected = null;
                }
                isSelected = true;
                TutoCombat.tutoCombat.charSelected = this;
                UITuto.uiTuto.statsUI.SetActive(true);
            }
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
            TutoCombat.tutoCombat.RemoveAlly(this);
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
        GetComponentInChildren<DamagedBarScript>().gameObject.SetActive(false);
        
        Character c = CharacterManager.characterManager.AskForCharacter(teamPosition);
        c.RemoveItem((NItem.EPartType)3);
        c.RemoveItem((NItem.EPartType)Random.Range(0, 3));
        for (int i = 0; i < 3; i++)
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
