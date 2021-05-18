using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ability : ScriptableObject
{
    public new string name;
    public int multiplicator;
    public Sprite icon;
    public bool isMelee;
    public bool onAlly;
    public enum DamageType 
    {
        BASE,
        MUD,
        ASH,
        ICE,
        EROSION
    }
    public DamageType damageType;
    
}
