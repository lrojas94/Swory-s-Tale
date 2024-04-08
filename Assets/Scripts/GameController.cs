using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum GameStatus
{
    Scrolling = 0,
    SelectingPlayerAction = 1,
    ProcessingActions = 2,
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

    // 0 => Player, 1 => Enemy
    [SerializeField]
    private bool[] actionsCompleted = new bool[2] { false, false };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (status == GameStatus.ProcessingActions)
        {
            bool allActionsComplete = actionsCompleted[0] && actionsCompleted[1];

            if (allActionsComplete)
            {
                // All players completed their actions. We'll update the state.
                if (activePlayer.isDead)
                {
                    status = GameStatus.PlayerDead;
                    OnGameStatusChange?.Invoke(status);
                }
                else if (activeEnemy.isDead)
                {
                    EnterWorldMode();
                }
                else
                {
                    EnterPlayerActionSelectionMode();
                }

            }
        }
    }

    public void OnActionComplete(bool isPlayer)
    {
        if (isPlayer)
        {
            actionsCompleted[0] = true;
        } else
        {
            actionsCompleted[1] = true;
        }
    }

    public void EnemyEncounter(CharacterBase player, CharacterBase enemy, Transform encounterTransform)
    {
        activePlayer = player;
        activeEnemy = enemy;
        playerCamera.Priority = 1;
        battleCamera.Priority = 2;
        battleCamera.Follow = encounterTransform;
        EnterPlayerActionSelectionMode();
    }

    public void EnterPlayerActionSelectionMode()
    {
        status = GameStatus.SelectingPlayerAction;
        OnGameStatusChange?.Invoke(status);
        PlayerMovementSelector.SetActive(true);
        activePlayer.UpdatePlayerState(CharacterState.Idle);
        activeEnemy.UpdatePlayerState(CharacterState.Idle);
    }

    public void EnterWorldMode()
    {
        status = GameStatus.Scrolling;
        OnGameStatusChange?.Invoke(status);
        playerCamera.Priority = 2;
        battleCamera.Priority = 1;
        activePlayer.UpdatePlayerState(CharacterState.Walking);
    }

    public void SelectPlayerMove(int playerAction)
    {

        Debug.Log($"Player selected {playerAction}");
        status = GameStatus.ProcessingActions;
        activePlayer.PerformAction((CharacterAction) playerAction);
        activeEnemy.PerformNextAction();
        PlayerMovementSelector.SetActive(false);
        actionsCompleted = new bool[2] { false, false };
    }
}
