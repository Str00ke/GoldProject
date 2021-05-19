using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 1)]
public class CharacterScriptableObject : ScriptableObject
{
    public int health;
    public int armor;
    public int attack;
    public float dodge;
    public float criticalChance;
    public float crititalDamage;
}
