using _Contents.Scripts.Managers;
using UnityEngine;

public class WaitingToStartState : State
{
    public WaitingToStartState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        base.Enter();
        // Allow user to place towers
    }

    public override void Update()
    {
        base.Update();
        // Check if user has placed a tower
        if (gameManager.HasPlacedTower)
        {
            gameManager.StateMachine.ChangeState(new StartedState(gameManager));
        }
    }
}