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
    public Sprite weaponSpriteBase;
    public Sprite weaponSpriteAnim;
    public float holdAllyCombo;
    [Header("Character Items & Scriptable Object")]
    public Character.ECharacterType charaType;
    public NItem.EWeaponType weaponType;
    public Sprite[] deadBodies;

    private void Start()
    {
        holdAllyCombo = holdCharacValue*2;
        stateIcons = UIManager.uiManager.stateIcons;
        CombatManager.combatManager.allies.Add(this);
        charType = CharType.ALLY;
        anim = this.GetComponent<Animator>();
        durationMove = 1.0f;
        healthBar = GameObject.Find(gameObject.name + "/CanvasChar/healthBar").GetComponent<Slider>();
        canvasChar = GameObject.Find(gameObject.name + "/CanvasChar");
        cursorNotPlayedYet = GameObject.Find(gameObject.name + "/CanvasChar/Cursors/cursorNotPlayedYet");
        cursorSelected = GameObject.Find(gameObject.name + "/CanvasChar/Cursors/cursorSelected");
        cursorPlaying = GameObject.Find(gameObject.name + "/CanvasChar/Cursors/cursorPlaying");
        statusLayoutGroup = GameObject.Find(gameObject.name + "/CanvasChar/StatusLayoutGroup");
        cursorSelected.SetActive(false);
        cursorPlaying.SetActive(false);
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
    public void CreateChar(Character cs, int teamPos)
    {
        charName = cs.charName;
        //Sprites
        head.sprite = cs.charHead;
        body.sprite = cs.itemSprites[0];
        helmet.sprite = cs.itemSprites[1];
        bodyArmor.sprite = cs.itemSprites[2];
        weaponSpriteBase = cs.itemSprites[3];
        charaType = cs.characterType;
        weaponType = cs.GetItem(NItem.EPartType.Weapon).itemWeaponType;

        switch (cs.GetItem(NItem.EPartType.Weapon).itemWeaponType)
        {
            case NItem.EWeaponType.Sword:
                abilities[0] = AbilitiesManager.abilitiesManager.weaponAbilities[0];
                abilities[1] = AbilitiesManager.abilitiesManager.weaponAbilities[2];
                break;
            case NItem.EWeaponType.Bow:
                abilities[0] = AbilitiesManager.abilitiesManager.weaponAbilities[1];
                abilities[1] = AbilitiesManager.abilitiesManager.weaponAbilities[3];
                break;
            case NItem.EWeaponType.Staff:
                abilities[0] = AbilitiesManager.abilitiesManager.weaponAbilities[1];
                abilities[1] = AbilitiesManager.abilitiesManager.weaponAbilities[4];
                break;
        }
        if(weaponSpriteBase != null && cs.GetItem(NItem.EPartType.Weapon).itemWeaponType == NItem.EWeaponType.Bow)
        {
            weaponSpriteAnim = cs.itemSprites[4];
        }
        else
        {
            weaponSpriteAnim = weaponSpriteBase;
        }
        weapon.sprite = weaponSpriteBase;
        teamPosition = teamPos;
        maxHealth = cs.maxHealth;
        damageRange = new Vector2(cs.attack - cs.attack * 0.1f, cs.attack + cs.attack * 0.1f);
        dodge = cs.dodge;
        //initiative = cs.initiative;
        initiative = Random.Range(1, 14);
        critChance = cs.criticalChance;
        critDamage = cs.crititalDamage;
        armor = cs.armor;
        health = cs.health;
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

    public override void LaunchAttack(Characters receiver, Ability ability)
    {
        float dmg = Mathf.Round(Random.Range(damageRange.x, damageRange.y));
        switch (weaponType)
        {
            case NItem.EWeaponType.Bow:
                AudioManager.audioManager.Play("AllyAttackBow");
                break;
            case NItem.EWeaponType.Staff:
                AudioManager.audioManager.Play("AllyAttackBow");
                break;
            case NItem.EWeaponType.Sword:
                AudioManager.audioManager.Play("AllyAttackSword");
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
            if (Random.Range(0.0f, 1.0f) < this.critChance)
            {
                FindObjectOfType<CameraScript>().CamShake(0.4f, 0.3f);
                dmg += dmg * this.critDamage;
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

    public override IEnumerator TakeDamageCor(float value, float duration)
    {
        GetComponentInChildren<DamagedBarScript>().gameObject.GetComponentInChildren<Image>().color = Color.grey;
        AudioManager.audioManager.Play("AllyTakeDamage");
        CombatManager.combatManager.hasTookDamage = true;
        ShowFloatingHealth(Mathf.Round(value).ToString(), true);
        float startValue = healthBar.value;
        float endValue = startValue - value;
        endValue = Mathf.Round(endValue);
        healthBar.value = endValue;
        health = endValue;
        if (health <= 0)
        {
        }
        yield return new WaitForSeconds(duration);
        GetComponentInChildren<DamagedBarScript>().UpdateDamagedBar(endValue, duration, false);
        yield return new WaitForSeconds(duration);
        if (health <= 0)
        {
            health = 0;
            Death();
        }
        yield return null;
    }
    
    public override void TakeHealing(float value, float duration)
    {
        CombatManager.combatManager.hasHeal = true;
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
    }
    public void Death()
    {
        CombatManager.combatManager.RemoveAlly(this);
        isMelee = false;
        isDead = true;
        isTargetable = false;
        canBeSelected = false;
        health = 0;
        GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        //GetComponentInChildren<DamagedBarScript>().gameObject.SetActive(false);
       // gameObject.SetActive(false);
        head.sprite = null;
        weapon.sprite = null;
        bodyArmor.sprite = null;
        helmet.sprite = null;
        if (charaType == Character.ECharacterType.Char0)
            body.sprite = deadBodies[0];
        else if (charaType == Character.ECharacterType.Char1)
            body.sprite = deadBodies[1];
        else if (charaType == Character.ECharacterType.Char2)
            body.sprite = deadBodies[2];

        Character c = CharacterManager.characterManager.AskForCharacter(teamPosition);
        c.RemoveItem(NItem.EPartType.Gem);
        c.RemoveItem((NItem.EPartType)Random.Range(0,3));
        for(int i = 0; i < 3; i++)
        {
            NItem.ItemScriptableObject item = c.GetItem((NItem.EPartType)i);
            if (item)
            {
                LevelData.AddSoToList(item);
                c.RemoveItem((NItem.EPartType)i);
            }
        }
        CharacterManager.characterManager.RemoveCharacter(c);
    }

    private void CanAttackEvent()
    {
        AbilitiesManager.abilitiesManager.canAttack = true;
        weapon.sprite = weaponSpriteBase;
    }

    private void IsAttackFinished()
    {
        AbilitiesManager.abilitiesManager.isAttackFinished = true;
    }

    private void BendingBow()
    {
        weapon.sprite = weaponSpriteAnim;
    }
}
