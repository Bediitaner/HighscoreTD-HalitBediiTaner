using _Contents.Scripts.Managers;
using UnityEngine;

public class GameEndState : State
{
    public GameEndState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        base.Enter();
        // Logic for game end
        gameManager.EndGame();
    }
}