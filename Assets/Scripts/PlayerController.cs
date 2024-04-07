using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1f;
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
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Oh no");
        if (collision.CompareTag("Enemy"))
        {
            GameController.Instance.EnemyEncounter(character, collision.gameObject.GetComponent<CharacterBase>());
        }
    }
}
