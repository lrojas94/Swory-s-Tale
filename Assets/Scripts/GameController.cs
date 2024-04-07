using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStatus
{
    Scrolling = 0,
    Fighting = 1
}

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public GameStatus status;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (status == GameStatus.Scrolling)
        {
            ScrollWorld();
        } 
    }

    void ScrollWorld()
    {
        
    }

    public void EnemyEncounter(CharacterBase player, CharacterBase enemy)
    {
        status = GameStatus.Fighting;
        player.UpdatePlayerState(CharacterState.Idle);
        enemy.UpdatePlayerState(CharacterState.Idle);

    }
}
