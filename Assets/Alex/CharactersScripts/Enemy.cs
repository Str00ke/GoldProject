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
        CombatManager.combatManager.enemies.Add(this);
        charType = CharType.ENEMY; 
        anim = this.GetComponent<Animator>();
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
                if (CombatManager.combatManager.charSelected != null)
                {
                    CombatManager.combatManager.charSelected.isSelected = false;
                    CombatManager.combatManager.charSelected.gameObject.GetComponentInChildren<CursorEffectsScript>().DeactivateCursor(CombatManager.combatManager.charSelected.cursorSelected);
                    CombatManager.combatManager.charSelected = null;
                }
                isSelected = true; 
                GetComponentInChildren<CursorEffectsScript>().ActivateCursor(cursorSelected);
                CombatManager.combatManager.charSelected = this;
                UIManager.uiManager.buttonStatus.SetActive(true);
                UIManager.uiManager.statsUI.SetActive(true);
            }
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
        cursorNotPlayedYet = GameObject.Find(gameObject.name + "/CanvasSlider/Cursors/cursorNotPlayedYet");
        cursorSelected = GameObject.Find(gameObject.name + "/CanvasSlider/Cursors/cursorSelected");
        cursorPlaying = GameObject.Find(gameObject.name + "/CanvasSlider/Cursors/cursorPlaying");
        statusLayoutGroup = GameObject.Find(gameObject.name + "/CanvasSlider/StatusLayoutGroup");
        cursorSelected.SetActive(false);
        cursorPlaying.SetActive(false);
        healthBarOutline.SetActive(false);
        maxHealth = e.maxHealth;
        enemyType = e.enemyType;
        SetEnemyImg();
        damageRange = e.damageRange;
        dodge = e.dodge;
        critChance = e.critChance;
        critDamage = e.critDamage;
        initiative = e.initiative;
        armor = e.armor;
        enemySprites = e.enemySprites;
        enemyElement = e.element;
        abilities = e.enemyAbilities;
        abilitiesCristal = e.enemySpecialsAbilities;
        EnnemyManager._enemyManager.MultiplicateByValues(this, level, mapRoom);
        itemElement = (ItemElement)System.Enum.Parse(typeof(ItemElement), e.element.ToString());
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        Debug.Log(this.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
        cursorSelected.transform.position = new Vector3(cursorSelected.transform.position.x, GetComponent<SpriteRenderer>().sprite.bounds.size.y / 5, cursorSelected.transform.position.z);
        cursorPlaying.transform.position = new Vector3(cursorPlaying.transform.position.x, GetComponent<SpriteRenderer>().sprite.bounds.size.y / 5, cursorPlaying.transform.position.z);
    }

    void SetEnemyImg()
    {
        switch (enemyType)
        {
            case (EEnemyType.SNAKE):
                gameObject.GetComponent<SpriteRenderer>().sprite = EnemiesImgPath.GetSprite(EnemiesImgPath.Snake_Spr);
                baseScale = new Vector3(0.3f,0.3f,1);
                break;

            case (EEnemyType.GIRAFFE):
                gameObject.GetComponent<SpriteRenderer>().sprite = EnemiesImgPath.GetSprite(EnemiesImgPath.Giraffe_Spr);
                baseScale = new Vector3(0.4f, 0.4f, 1);
                break;

            case (EEnemyType.DEATH):
                gameObject.GetComponent<SpriteRenderer>().sprite = EnemiesImgPath.GetSprite(EnemiesImgPath.Death_Spr);
                baseScale = new Vector3(0.4f, 0.4f, 1);
                break;
        }
    }

    public override void LaunchAttack(Characters receiver, Ability ability)
        {
        float dmg = Mathf.Round(Random.Range(damageRange.x, damageRange.y));
        switch (enemyType)
        {
            case (EEnemyType.SNAKE):
                AudioManager.audioManager.Play("SnakeAttack");
                break;

            case (EEnemyType.GIRAFFE):
                AudioManager.audioManager.Play("GiraffeAttack");
                break;

            case (EEnemyType.DEATH):
                AudioManager.audioManager.Play("DeathAttack");
                break;
        }
        dmg *= (ability.multiplicator / 100);
        if (Random.Range(0, 100) < receiver.dodge)
        {
            receiver.ShowFloatingHealth("Dodge", true);
            AudioManager.audioManager.Play("Dodge");
        }
        else
        {
            //-CRITIC DAMAGE-
            if (Random.Range(0.0f, 1.0f) < this.critChance/100)
            {
                FindObjectOfType<CameraScript>().CamShake(0.4f, 0.3f);
                dmg += dmg * this.critDamage / 100;
            }
            else
            {
                FindObjectOfType<CameraScript>().CamShake(0.2f, 0.05f);
            }
            if (ability.crAttackType == Ability.CristalAttackType.DESTRUCTION)
            {
                LaunchDestruction(receiver, ability);
            }
            else if (ability.crAttackType == Ability.CristalAttackType.DOT)
            {
                PutDot(receiver, ability);
            }
            //-ARMOR MODIF ON DAMAGE-
            dmg -= receiver.armor;
            dmg = dmg < 0 ? 0 : dmg;

            //-ELEMENTAL REACTIONS-

            receiver.TakeDamage(dmg, durationDecreaseHealth);

            receiver.ElementReactions((CurrentElement)System.Enum.Parse(typeof(CurrentElement), ability.elementType.ToString()));

            if (receiver.GetType() == typeof(Enemy))
            {
                AchievementsManager.DamageDeal(dmg);
            }
        }
    }


    public override void TakeDamage(float value, float duration)
    {
        if(enemyType == EEnemyType.SNAKE)
        {
            AudioManager.audioManager.Play("SnakeTakeDamage");
        }else if(enemyType == EEnemyType.DEATH)
        {
            AudioManager.audioManager.Play("DeathTakeDamage");
        }
        else if (enemyType == EEnemyType.GIRAFFE)
        {
            AudioManager.audioManager.Play("GiraffeTakeDamage");
        }
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
        if (health <= 0)
        {
            health = 0;
            if (enemyType == EEnemyType.SNAKE)
            {
                AudioManager.audioManager.Play("SnakeDeath");
            }
            else if (enemyType == EEnemyType.DEATH)
            {
                AudioManager.audioManager.Play("DeathDeath");
            }
            else if (enemyType == EEnemyType.GIRAFFE)
            {
                AudioManager.audioManager.Play("GiraffeDeath");
            }
            isDead = true;
        }
        yield return new WaitForSeconds(duration);
        GetComponentInChildren<DamagedBarScript>().UpdateDamagedBar(endValue, duration, false);
        yield return new WaitForSeconds(duration);
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
        if (!removed && isDead)
        {
            CombatManager.combatManager.RemoveEnemy(this);
            removed = true;
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
