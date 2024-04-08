using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class CharacterAttack : MonoBehaviour
{
    public CharacterAction ActionType;  
    [SerializeField]
    private bool isPlayerAttack = true;
    [SerializeField]
    public bool SourceIsPlayer = true;
    [SerializeField]
    public float attackValue;
    [SerializeField]
    protected float updatedAttackValue;
    public bool IsPlayerAttack
    {
        get
        {
            return isPlayerAttack;
        }
        set
        {
            var changed = isPlayerAttack != value;
            isPlayerAttack = value;
            if (changed)
            {
                OnPlayerAttackUpdated();
            }
        }
    }
    
    public void UpdateTag(string  tag)
    {
        gameObject.tag = tag;
    }

    public void ActionCompleted()
    {
        GameController.Instance.OnActionComplete(SourceIsPlayer);
    }
    protected virtual void OnPlayerAttackUpdated()
    {
        // Do nothing.
    }
}
