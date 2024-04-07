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

[Serializable]
public struct CharacterActionDefinition
{
    public CharacterAction action;
    public GameObject prefab;
    public Transform instantiateFrom;
}


public class CharacterBase : MonoBehaviour
{
    public CharacterState state = CharacterState.Idle;
 
    [SerializeField]
    private CharacterAnimation[] characterAnimationMap;
    [SerializeField]
    private CharacterActionDefinition[] characterActionDefinitionMap;
    [SerializeField]
    private int health = 100;

    [SerializeField]
    private CharacterAction[] actionPattern;
    private int actionPatternIndex;

    private Animator animator;

    [SerializeField]
    private GameObject floatingTextPrefab;

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

        if (floatingTextPrefab != null)
        {
            Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        }

        if (health <= 0)
        {
            return true;
        }

        return false;
    }

    public void TriggerActionPrefab(CharacterAction action)
    {
        CharacterActionDefinition? characterActionDefinition = characterActionDefinitionMap.FirstOrDefault(item => item.action == action);
        
        if (characterActionDefinition.HasValue && characterActionDefinition.Value.prefab != null)
        {
            GameObject instance = GameObject.Instantiate(characterActionDefinition.Value.prefab);
            instance.transform.position = characterActionDefinition.Value.instantiateFrom.position;
        }
    }
}
