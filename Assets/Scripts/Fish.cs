using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New fish", menuName = "Fish")]
public class Fish : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public int health;
    public float physicalDefense;
    public float magicalDefense;
    public float evasion;
    public Element element = Element.None;
    [SerializeField]
    public List<Attack> attacks;

    public int CalculateDamage(Fish target, Attack attack)
    {
        float damageMultiplier = ElementDictionary.GetElementMultiplier(attack.element, target.element);
        float physicalDamageValue = (attack.physicalDamage - target.physicalDefense) * damageMultiplier;
        float magicalDamageValue = (attack.magicalDamage - target.magicalDefense) * damageMultiplier;

        return ((int)Mathf.Max(0, Mathf.Ceil(physicalDamageValue + magicalDamageValue)));
    }
}
