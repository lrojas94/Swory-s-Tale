using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public enum GameStatus
{
    Scrolling = 0,
    SelectingPlayerAction = 1,
    ProcessingAnimation = 2,
    PlayerDead = 3
}

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public event Action<GameStatus> OnGameStatusChange;

    public GameObject PlayerMovementSelector;
    public GameStatus status;

    private CharacterBase activePlayer;
    private CharacterBase activeEnemy;

    [SerializeField]
    private CinemachineVirtualCamera playerCamera;
    [SerializeField]
    private CinemachineVirtualCamera battleCamera;

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

    public void EnemyEncounter(CharacterBase player, CharacterBase enemy, Transform encounterTransform)
    {
        status = GameStatus.SelectingPlayerAction;
        player.UpdatePlayerState(CharacterState.Idle);
        enemy.UpdatePlayerState(CharacterState.Idle);
        OnGameStatusChange?.Invoke(status);
        PlayerMovementSelector.SetActive(true);

        activePlayer = player;
        activeEnemy = enemy;

        playerCamera.Priority = 1;
        battleCamera.Priority = 2;
        battleCamera.Follow = encounterTransform;
    }

    

    public void SelectPlayerMove(int playerAction)
    {

        Debug.Log($"Player selected {playerAction}");
        status = GameStatus.ProcessingAnimation;
        activePlayer.PerformAction((CharacterAction) playerAction);
        activeEnemy.PerformNextAction();
        PlayerMovementSelector.SetActive(false);
    }
}
