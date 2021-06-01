using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemyTuto : CharactersTuto, IPointerDownHandler, IPointerUpHandler
{
    
    public EEnemyType enemyType;
    public EElement enemyElement;
    void Start()
    {
        stateIcons = UITuto.uiTuto.stateIcons;
        stateIcons = UITuto.uiTuto.stateIcons;
        TutoCombat.tutoCombat.enemies.Add(this);
        charType = CharType.ENEMY;
        anim = this.GetComponent<Animator>();
        thisColorBody = this.GetComponent<SpriteRenderer>();
        thisColorHead = this.GetComponent<SpriteRenderer>();
        durationDecreaseHealth = 1.0f;

        //ISTARGETABLE FOR ABILITIES
        isTargetable = false;
        UpdateStateIcon();
        healthBar = GameObject.Find(gameObject.name + "/CanvasSlider/healthBar").GetComponent<Slider>();
        canvasChar = GameObject.Find(gameObject.name + "/CanvasSlider");
        healthBarOutline = GameObject.Find(gameObject.name + "/CanvasSlider/HealthBarOutline");
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
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
                if (TutoCombat.tutoCombat.enemySelected != null)
                {
                    TutoCombat.tutoCombat.enemySelected.isSelected = false;
                    TutoCombat.tutoCombat.enemySelected = null;
                }
                isSelected = true;
                TutoCombat.tutoCombat.enemySelected = this;
            }
            UITuto.uiTuto.enemyStatsUI.SetActive(true);
        }

    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        onPointerHold = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        UITuto.uiTuto.ResetEnemyDisplayUI();
        onPointerHold = false;
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
