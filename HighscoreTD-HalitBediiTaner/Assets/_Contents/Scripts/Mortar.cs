using UnityEngine;

public class Mortar : Tower
{
    protected override void Activate()
    {
        // Mortar specific activation logic
    }

    public override void Attack(Enemy enemy)
    {
        if (IsEnemyInRange(enemy) && !IsEnemyTooClose(enemy))
        {
            FireMortarShell(enemy);
        }
    }
    
    private bool IsEnemyInRange(Enemy enemy)
    {
        return Vector3.Distance(Position, enemy.Position) <= config.range;
    }
    
    private bool IsEnemyTooClose(Enemy enemy)
    {
        return Vector3.Distance(Position, enemy.Position) <= 2.0f; // Example dead range
    }
    
    private void FireMortarShell(Enemy enemy)
    {
        // Logic to fire a mortar shell
    }
}