using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitiesScriptTuto : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SpriteRenderer thisColor;
    public Ability ability;
    public Color selectedColor;
    public Color idleColor;
    public bool isSelected;
    public bool interactible;
    // Start is called before the first frame update
    void Start()
    {
        thisColor = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
        if (!ability)
        {
            interactible = false;
        }
        else
        {
            interactible = true;
        }
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
        if (interactible)
        {
            if (isSelected)
            {
                if (AbilitiesTuto.abilitiesTuto.abilitySelected == this)
                {
                    isSelected = false;
                    AbilitiesTuto.abilitiesTuto.abilitySelected = null;
                    AbilitiesTuto.abilitiesTuto.ClearTargets();
                }
            }
            else if (!isSelected)
            {
                if (AbilitiesTuto.abilitiesTuto.abilitySelected != null)
                {
                    AbilitiesTuto.abilitiesTuto.abilitySelected.isSelected = false;
                    AbilitiesTuto.abilitiesTuto.abilitySelected = null;
                }

                isSelected = true;
                AbilitiesTuto.abilitiesTuto.abilitySelected = this;
                AbilitiesTuto.abilitiesTuto.SetTargets(AbilitiesTuto.abilitiesTuto.abilitySelected.ability);
            }
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
