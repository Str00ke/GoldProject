
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Ability))]
[CanEditMultipleObjects]
public class AbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var ability = target as Ability;
        serializedObject.ApplyModifiedProperties();
        GUILayout.Label("\n \n \n");

        if (GUILayout.Button("Save Changes (Do Ctrl+S after click)"))
        {
            EditorUtility.SetDirty(target);
        }
        ability.name = EditorGUILayout.TextField("Name", ability.name);
        ability.multiplicator = EditorGUILayout.FloatField("Multiplicator(heal/damage)", ability.multiplicator);
        ability.icon = (Sprite)EditorGUILayout.ObjectField("Icone  ", ability.icon, typeof(Sprite), allowSceneObjects: true);
        ability.objectType = (Ability.ObjectType)EditorGUILayout.EnumPopup("Object Type ", ability.objectType);
        ability.elementType = (Ability.ElementType)EditorGUILayout.EnumPopup("Element ", ability.elementType);
        if (ability.objectType == Ability.ObjectType.CRISTAL)
        {
            ability.crType = (Ability.CristalAbilityType)EditorGUILayout.EnumPopup("Cristal ability ", ability.crType);
            switch (ability.crType) 
            {
                case Ability.CristalAbilityType.HEAL:
                    ability.crHealType = (Ability.CristalHealType)EditorGUILayout.EnumPopup("Cristal heal ability ", ability.crHealType);
                    if(ability.crHealType == Ability.CristalHealType.DRINK)
                    {
                        switch (ability.elementType)
                        {
                            case Ability.ElementType.ICE:
                                ability.bonusmalus = EditorGUILayout.FloatField("Bonus crit chance %", ability.bonusmalus);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                            case Ability.ElementType.ASH:
                                ability.bonusmalus = EditorGUILayout.FloatField("Damage bonus", ability.bonusmalus);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                            case Ability.ElementType.MUD:
                                ability.bonusmalus = EditorGUILayout.FloatField("Dodge bonus %", ability.bonusmalus);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                            case Ability.ElementType.PSY:
                                ability.bonusmalus = EditorGUILayout.FloatField("Bonus crit damage %", ability.bonusmalus);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;

                        }
                    }else if(ability.crHealType == Ability.CristalHealType.BOOST)
                    {
                        switch (ability.elementType)
                        {
                            case Ability.ElementType.ICE:
                                ability.bonusmalus = EditorGUILayout.FloatField("Bonus crit damage %", ability.bonusmalus);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                            case Ability.ElementType.ASH:
                                ability.bonusmalus = EditorGUILayout.FloatField("Damage bonus", ability.bonusmalus);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                            case Ability.ElementType.MUD:
                                ability.bonusmalus = EditorGUILayout.FloatField("Armor bonus ", ability.bonusmalus);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                            case Ability.ElementType.PSY:
                                ability.bonusmalus = EditorGUILayout.FloatField("Dodge bonus %", ability.bonusmalus);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                        }
                    }else if(ability.crHealType == Ability.CristalHealType.BATH)
                    {
                        switch (ability.elementType)
                        {
                            case Ability.ElementType.ICE:
                                EditorGUILayout.LabelField("Targets: ", "targeted ally");
                                break;
                            case Ability.ElementType.ASH:
                                EditorGUILayout.LabelField("Targets: ", "targeted ally and one random");
                                break;
                            case Ability.ElementType.MUD:
                                EditorGUILayout.LabelField("Targets: ", "All allies");
                                break;
                            case Ability.ElementType.PSY:
                                EditorGUILayout.LabelField("Targets: ", " All Allies & Enemies");
                                break;
                        }
                    }
                    break; 
                case Ability.CristalAbilityType.ATTACK:
                    ability.crAttackType = (Ability.CristalAttackType)EditorGUILayout.EnumPopup("Cristal attack ability ", ability.crAttackType);
                    if(ability.crAttackType == Ability.CristalAttackType.DOT)
                    {
                        ability.dotMult = EditorGUILayout.FloatField("Dot damage mod", ability.dotMult);
                        ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                    }else if (ability.crAttackType == Ability.CristalAttackType.MARK)
                    {
                        ability.markMult = EditorGUILayout.FloatField("Mark damage mod", ability.markMult);
                        ability.turnDuration = EditorGUILayout.IntField("Turns before explosion", ability.turnDuration);
                    }
                    break;
                case Ability.CristalAbilityType.OTHERS:
                    ability.crSpecialType = (Ability.CristalSpecialType)EditorGUILayout.EnumPopup("Cristal special ability ", ability.crSpecialType);
                    if(ability.crSpecialType == Ability.CristalSpecialType.DESTRUCTION)
                    {

                        switch (ability.elementType)
                        {
                            case Ability.ElementType.ICE:
                                ability.bonusmalus = EditorGUILayout.FloatField("Malus dodge flat", ability.bonusmalus);
                                ability.destruModif = EditorGUILayout.FloatField("Armor destruction", ability.destruModif);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                            case Ability.ElementType.ASH:
                                ability.bonusmalus = EditorGUILayout.FloatField("Damage Malus", ability.bonusmalus);
                                ability.destruModif = EditorGUILayout.FloatField("Armor destruction", ability.destruModif);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                            case Ability.ElementType.MUD:
                                ability.bonusmalus = EditorGUILayout.FloatField("Health malus ", ability.bonusmalus);
                                ability.destruModif = EditorGUILayout.FloatField("Armor destruction", ability.destruModif);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                            case Ability.ElementType.PSY:
                                ability.bonusmalus = EditorGUILayout.FloatField("Armor malus", ability.bonusmalus);
                                ability.destruModif = EditorGUILayout.FloatField("Armor destruction", ability.destruModif);
                                ability.turnDuration = EditorGUILayout.IntField("Duration (turn)", ability.turnDuration);
                                break;
                        }
                    }
                    break;
            }
        }
        else if(ability.objectType == Ability.ObjectType.WEAPON)
        {
            ability.targetType = (Ability.TargetType)EditorGUILayout.EnumPopup("Target ", ability.targetType);
            ability.weaponAbilityType = (Ability.WeaponAbilityType)EditorGUILayout.EnumPopup("Weapon type ", ability.weaponAbilityType);
        }

    }
}

#endif