using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 1)]
public class CharacterScriptableObject : ScriptableObject
{
    public string charName;
    public int health;
    public int armor;
    public int attack;
    public float initiative;
    public float dodge;
    public float criticalChance;
    public float crititalDamage;
    [Space]
    public Character.ECharacterType characterType;
    public Sprite characterHead;
}
