using Resources.Scripts.Managers;
using UnityEngine;

public class TowerPlacementState : State
{
    public TowerPlacementState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0.33f;
    }

    public override void Update()
    {
        base.Update();
        if (gameManager.HasPlacedTower)
        {
            gameManager.StateMachine.ChangeState(new PlayingState(gameManager));
        }
    }
}