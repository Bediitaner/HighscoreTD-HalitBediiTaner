using Resources.Scripts.Managers;
using UnityEngine;

public class WaitingToStartState : State
{
    public WaitingToStartState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (gameManager.HasPlacedTower)
        {
            gameManager.StateMachine.ChangeState(new StartedState(gameManager));
        }
    }
}