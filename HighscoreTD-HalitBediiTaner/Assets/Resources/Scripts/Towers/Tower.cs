using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public TowerConfigSO config;
    public int level;
    protected float currentCooldown;

    public Vector3 Position { get; private set; }

    public void Initialize(TowerConfigSO towerConfigSO)
    {
        config = towerConfigSO;
        transform.name = config.towerType.ToString();
        ResetCooldown();
    }

    public void Place(Vector3 newPosition)
    {
        Position = newPosition;
        Activate();
    }

    protected abstract void Activate();
    public abstract void Attack(Enemy enemy);

    protected void ResetCooldown()
    {
        currentCooldown = config.cooldown;
    }
}