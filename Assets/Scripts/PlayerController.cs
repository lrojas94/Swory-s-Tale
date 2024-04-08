using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    private CharacterBase character;

     
    // Start is called before the first frame update
    void Awake()
    {
        if (character == null)
        {
            character = GetComponent<CharacterBase>();
        }
        animator = GetComponent<Animator>();    
        character.UpdatePlayerState(CharacterState.Walking);
        character.isPlayer = true;
    }
}
