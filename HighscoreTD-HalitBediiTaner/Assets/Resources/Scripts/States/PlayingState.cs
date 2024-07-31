using Resources.Scripts.Managers;
using UnityEngine;

public class PlayingState : State
{
    public PlayingState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        base.Enter();
        // Normal game speed
        Time.timeScale = 1f;
    }

    public override void Update()
    {
        base.Update();
        // Game logic for playing state
    }
}