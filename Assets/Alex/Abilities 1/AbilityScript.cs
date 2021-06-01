using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SpriteRenderer thisColor;
    public Ability ability;
    public Color selectedColor;
    public Color idleColor;
    public bool isSelected;
    // Start is called before the first frame update
    void Start()
    {
        thisColor = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()   
    {
        ChangeColor();
    }


    public void ChangeColor()
    {
        if (isSelected)
        {
            thisColor.color = selectedColor;
        }
        else
        {
            thisColor.color = idleColor;
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {

        if (isSelected)
        {
            if (AbilitiesManager.abilitiesManager.abilitySelected == this)
            {
                isSelected = false;
                AbilitiesManager.abilitiesManager.abilitySelected = null;
                AbilitiesManager.abilitiesManager.ClearTargets();
            }
        }
        else if (!isSelected)
        {
            if (AbilitiesManager.abilitiesManager.abilitySelected != null)
            {
                AbilitiesManager.abilitiesManager.abilitySelected.isSelected = false;
                AbilitiesManager.abilitiesManager.abilitySelected = null;
            }

            isSelected = true;
            AbilitiesManager.abilitiesManager.abilitySelected = this;
            AbilitiesManager.abilitiesManager.SetTargets(AbilitiesManager.abilitiesManager.abilitySelected.ability);
        }
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
