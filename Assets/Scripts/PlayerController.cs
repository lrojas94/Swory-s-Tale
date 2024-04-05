using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PlayerState
{
    Walking = 0,
    Idle = 1,
    Attacking = 2,
    Defending = 3,
    LaunchingSpell = 4,
    Winding = 5,
    Dead = 6
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1f;
    Animator animator;

    Dictionary<PlayerState, string> playerStateAnimationMap = new Dictionary<PlayerState, string>() {
        { PlayerState.Walking, "Player_Walk" },
        { PlayerState.Idle, "Player_Idle" },
    };


    public PlayerState state;

     
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePlayerState(PlayerState playerState)
    {
        this.state = playerState;
        switch (state)
        {
            case PlayerState.Idle:
                animator.Play(playerStateAnimationMap.GetValueOrDefault(state));
                break;

            default:
                break;
        }
    }
}
