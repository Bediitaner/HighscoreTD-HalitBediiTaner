using Resources.Scripts.Managers;
using UnityEngine;

public class StartingState : State
{
    public StartingState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        base.Enter();
        GameManager.Instance.StartSpawningEnemies();
    }

    public override void Update()
    {
        base.Update();
        if (gameManager.IsDataLoaded)
        {
            gameManager.StateMachine.ChangeState(new WaitingToStartState(gameManager));
        }
    }
}