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

    public void UpdatePlayerState(CharacterState state)
    {
        this.state = state;
        
        CharacterAnimation? animation = characterAnimationMap.FirstOrDefault(pair => pair.state == state);

        if (animation.HasValue)
        {
            animator.Play(animation.Value.animationName);
        }
        
    }
}
