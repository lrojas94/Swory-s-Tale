using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
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
    public bool isPlayer = false;
    public CharacterState state = CharacterState.Idle;
    
    [SerializeField]
    private CharacterAnimation[] characterAnimationMap;
    [SerializeField]
    private CharacterActionDefinition[] characterActionDefinitionMap;
    [SerializeField]
    private int health = 100;
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private HealthBar healthBar;


    public bool isDead
    {
        get
        {
            return this.health <= 0;
        }
    }


    [SerializeField]
    private CharacterAction[] actionPattern;
    private int actionPatternIndex;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField]
    private GameObject floatingTextPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (healthBar == null)
        {
            healthBar = GetComponent<HealthBar>();
        }

        health = maxHealth;
    }

    void Start()
    {
        UpdatePlayerState(state);
    }

    public void PerformNextAction()
    {
        Debug.Log($"Performing {actionPattern[actionPatternIndex]}");
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

        if (animation.HasValue && animator != null)
        {
            animator.Play(animation.Value.animationName);
        }

        if (state == CharacterState.Dead)
        {
            // Go ahead and kill the player:
            if (rb == null)
            {
                rb = this.AddComponent<Rigidbody2D>();
            }

            rb.mass = 1;
            rb.gravityScale = 1;
            rb.AddForce(new Vector2(isPlayer ? -1 : 1, 3), ForceMode2D.Impulse);
            rb.constraints = RigidbodyConstraints2D.None;
            Collider2D[] colliders = new Collider2D[rb.attachedColliderCount];
            rb.GetAttachedColliders(colliders);
            foreach(Collider2D collider in colliders) { collider.enabled = false; }
            StartCoroutine(RotateAndFade());
        }
    }

    public bool TakeDamage(int damage)
    {
        
        health -= damage;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(maxHealth, health);
        }
        DamagePopup.ShowDamage(damage, transform.position);

        if (floatingTextPrefab != null)
        {
            Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        }

        if (health <= 0)
        {
            UpdatePlayerState(CharacterState.Dead);
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
            CharacterAttack characterAttack = instance.GetComponent<CharacterAttack>();
            characterAttack.IsPlayerAttack = isPlayer;
            characterAttack.SourceIsPlayer = isPlayer;
            int layer = isPlayer ? LayerMask.NameToLayer("PlayerAttack") : LayerMask.NameToLayer("EnemyAttack");
            instance.layer = layer;
            foreach (Transform child in instance.transform)
            {
                child.gameObject.layer = layer;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            UpdatePlayerState(CharacterState.Dead);
        } 
    }

    IEnumerator RotateAndFade()
    {
        Color c = sr.color;
        Debug.Log(sr.color);
        float fadeSpeed = 0.5f;
        float rotateSpeed = 300f;
        Debug.Log(c.a);

        while(c.a > 0)
        {
            c.a -= fadeSpeed * Time.deltaTime;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation  += new Vector3(0, 0, -rotateSpeed * Time.deltaTime);
            Debug.Log(rotation);
            transform.rotation = Quaternion.Euler(rotation);
            sr.color = c;
            yield return null;
        }

        Destroy(gameObject);
    }
}
