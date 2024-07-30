using _Contents.Scripts.Managers;
using UnityEngine;

public class StartingState : State
{
    public StartingState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        base.Enter();
        // Load data from Firebase
        gameManager.LoadSession();
    }

    public override void Update()
    {
        base.Update();
        // Check if data is loaded and show popup
        if (gameManager.IsDataLoaded)
        {
            // Show load game popup
            // On user decision, transition to WaitingToStart
            gameManager.StateMachine.ChangeState(new WaitingToStartState(gameManager));
        }
    }
}