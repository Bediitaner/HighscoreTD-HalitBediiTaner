using Resources.Scripts.Managers;
using UnityEngine;

public class PlayingState : State
{
    public PlayingState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 1f;
    }

    public override void Update()
    {
        base.Update();
    }
}