using Resources.Scripts.Managers;
using UnityEngine;

public class StartedState : State
{
    private State currentState;

    public StartedState(GameManager gameManager) : base(gameManager)
    {
        currentState = new PlayingState(gameManager);
    }

    public override void Enter()
    {
        base.Enter();
        currentState.Enter();
    }

    public override void Update()
    {
        base.Update();
        currentState.Update();

        if (gameManager.IsBuyingTower)
        {
            currentState.Exit();
            currentState = new TowerPlacementState(gameManager);
            currentState.Enter();
        }
    }

    public override void Exit()
    {
        base.Exit();
        currentState.Exit();
    }
}