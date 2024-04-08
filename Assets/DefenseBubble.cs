using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefenseBubble : CharacterAttack
{
    [SerializeField]
    private float attackTimeoutTime = 1.5f; // 3 seconds
    private Animator animator;
    private bool instantiationComplete = false;

    private void Awake()
    {
        ActionType = CharacterAction.Defend;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(instantiationComplete)
        {
            attackTimeoutTime -= Time.deltaTime;
            if (attackTimeoutTime < 0 )
            {
                animator.SetTrigger("Fade");
                instantiationComplete = false;
            }
        }
    }

    public void InstantiationComplete()
    {
        instantiationComplete = true;
    }

    public void OnFadeOutComplete()
    {
        ActionCompleted();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PhysicalAttack"))
        {
            // Confirm it's defense value:
            CharacterAttack collidingAttack = collision.gameObject.GetComponent<CharacterAttack>();

            if (collidingAttack.attackValue > attackValue * 1.5f)
            {
                Destroy(gameObject);
                ActionCompleted();
            } else
            {
                // We will reflect, we can fade this out.
                animator.SetTrigger("Fade");
            }

        }
    }
}
