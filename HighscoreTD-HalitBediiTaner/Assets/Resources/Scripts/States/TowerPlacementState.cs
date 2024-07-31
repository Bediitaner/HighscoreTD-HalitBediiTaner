using Resources.Scripts.Managers;
using UnityEngine;

public class TowerPlacementState : State
{
    public TowerPlacementState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        base.Enter();
        // Slow down game speed
        Time.timeScale = 0.33f;
    }

    public override void Update()
    {
        base.Update();
        // Check if tower is placed
        if (gameManager.HasPlacedTower)
        {
            gameManager.StateMachine.ChangeState(new PlayingState(gameManager));
        }
    }
}