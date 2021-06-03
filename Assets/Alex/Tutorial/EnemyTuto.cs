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
        TutoCombat.tutoCombat.enemies.Add(this);
        charType = CharType.ENEMY;
        anim = this.GetComponent<Animator>();
        durationDecreaseHealth = 1.0f;

        //ISTARGETABLE FOR ABILITIES
        isTargetable = false;
        UpdateStateIcon();
        healthBar = GameObject.Find(gameObject.name + "/CanvasSlider/healthBar").GetComponent<Slider>();
        canvasChar = GameObject.Find(gameObject.name + "/CanvasSlider");
        healthBarOutline = GameObject.Find(gameObject.name + "/CanvasSlider/HealthBarOutline");
        cursorNotPlayedYet = GameObject.Find(gameObject.name + "/CanvasSlider/cursorNotPlayedYet");
        cursorSelected = GameObject.Find(gameObject.name + "/CanvasSlider/cursorSelected");
        cursorPlaying = GameObject.Find(gameObject.name + "/CanvasSlider/cursorPlaying");
        cursorSelected.SetActive(false);
        cursorPlaying.SetActive(false);
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        TutoCombat.tutoCombat.enemies.Add(this);
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
        float startValue = health;
        float endValue = startValue - value;
        endValue = Mathf.Round(endValue);
        healthBar.value = endValue;
        health = endValue;
        yield return new WaitForSeconds(duration);
        GetComponentInChildren<DamagedBarScript>().UpdateDamagedBar(endValue, duration, false);
        yield return new WaitForSeconds(duration);
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
