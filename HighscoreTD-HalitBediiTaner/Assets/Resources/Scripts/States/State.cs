using Resources.Scripts.Managers;
using UnityEngine;

public abstract class State
{
    protected GameManager gameManager;

    public State(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}