using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterState
{
    Walking = 0,
    Idle = 1,
    Attacking = 2,
    Defending = 3,
    LaunchingSpell = 4,
    Winding = 5,
    Dead = 6
}

public enum CharacterAction
{
    Attack = 0,
    Defend = 1,
    Spell = 2,
}

[Serializable]
public struct CharacterAnimation
{
    public CharacterState state;
    public string animationName;
}


public class CharacterBase : MonoBehaviour
{

    [SerializeField]
    private CharacterAnimation[] characterAnimationMap;
    public CharacterState state = CharacterState.Idle;
    private int health = 100;

    [SerializeField]
    private CharacterAction[] actionPattern;
    private int actionPatternIndex;

    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        UpdatePlayerState(state);
    }

    public void PerformNextAction()
    {
        PerformAction(actionPattern[actionPatternIndex]);
        actionPatternIndex++;

        if (actionPatternIndex == actionPattern.Length)
        {
            actionPatternIndex = 0;
        }
    }

    public void PerformAction(CharacterAction action)
    {
        CharacterState newState = action == CharacterAction.Attack ? CharacterState.Attacking
            : action == CharacterAction.Defend ? CharacterState.Defending
            : action == CharacterAction.Spell ? CharacterState.LaunchingSpell
            : CharacterState.Idle;

        UpdatePlayerState(newState);
    }


    public void UpdatePlayerState(CharacterState state)
    {
        this.state = state;
        
        CharacterAnimation? animation = characterAnimationMap.FirstOrDefault(pair => pair.state == state);

        if (animation.HasValue)
        {
            animator.Play(animation.Value.animationName);
        }
    }

    public bool DamageCharacter(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            return true;
        }

        return false;
    }

}
