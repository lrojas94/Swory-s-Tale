using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;

public class SlashAttack : CharacterAttack
{
    // Start is called before the first frame update
    [SerializeField]
    float speed = 1f;

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
        updatedAttackValue = attackValue;
        OnPlayerAttackUpdated();
    }

    private void FixedUpdate()
    {
        Debug.Log(rb.velocity);
        if (IsPlayerAttack == false)
        {
            rb.velocity = new Vector2(-speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(speed, 0);
        }
    }

    private void LateUpdate()
    {
        if (attackValue != updatedAttackValue)
        {
            attackValue = updatedAttackValue;
        }
    }

    protected override void OnPlayerAttackUpdated()
    {
        Transform sprite = transform.GetChild(0);
        Debug.Log($"Called, {IsPlayerAttack} / {sprite.localScale.x}");

        if((sprite.localScale.x < 0 && IsPlayerAttack)
            || (sprite.localScale.x > 0 && !IsPlayerAttack))
        {
            sprite.localScale = -sprite.localScale;
        }
         
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        CinemachineCameraShaker.Instance.Shake(1, .1f);
        Debug.Log($"Collided with: {collision.gameObject.name}, Layer: {LayerMask.LayerToName(gameObject.layer)}");
        if (collision.gameObject.CompareTag("DefensiveAttack"))
        {
            // Confirm it's defense value:
            CharacterAttack collidingAttack = collision.gameObject.GetComponent<CharacterAttack>();

            float collidingAttackValueWithMultiplier = collidingAttack.attackValue * 1.5f;

            if (collidingAttackValueWithMultiplier > attackValue)
            {
                int newLayer = IsPlayerAttack ? LayerMask.NameToLayer("EnemyAttack") : LayerMask.NameToLayer("PlayerAttack");
                // Reflect:
                gameObject.layer = newLayer;
                foreach (Transform t in gameObject.transform)
                {
                    t.gameObject.layer = newLayer;
                }

                IsPlayerAttack = !IsPlayerAttack;
            } else
            {
                updatedAttackValue = attackValue - collidingAttackValueWithMultiplier;
            }


        }

        if (collision.gameObject.CompareTag("PhysicalAttack"))
        {
            CharacterAttack collidingAttack = collision.gameObject.GetComponent<CharacterAttack>();
            updatedAttackValue = attackValue - collidingAttack.attackValue;
            if (updatedAttackValue <= 0)
            {
                Destroy(gameObject);
                Instantiate(AssetManager.Instance.ExplosionPrefab, collision.GetContact(0).point, Quaternion.identity);
                ActionCompleted();
            } 

        }

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            CharacterBase character = collision.gameObject.GetComponent<CharacterBase>();
            character.TakeDamage((int)Mathf.Ceil(attackValue));
            Destroy(gameObject);
            Instantiate(AssetManager.Instance.ExplosionPrefab, collision.GetContact(0).point, Quaternion.identity);
            ActionCompleted();
        }
    }
}
