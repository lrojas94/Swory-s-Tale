using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BattleTrigger : MonoBehaviour
{
    public CharacterBase enemy;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameController.Instance.EnemyEncounter(collision.gameObject.GetComponent<CharacterBase>(), enemy, transform);
            
        }
    }
}
