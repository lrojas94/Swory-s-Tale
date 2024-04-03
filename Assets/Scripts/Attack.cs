using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum AttackType
{
    Physical = 0,
    Magical = 1
}

[CreateAssetMenu(fileName="Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    public new string name;
    public ParticleSystem hitEfx;
    public float physicalDamage;
    public float magicalDamage;
    public AttackType type = AttackType.Physical;
    public Element element = Element.None;
}
