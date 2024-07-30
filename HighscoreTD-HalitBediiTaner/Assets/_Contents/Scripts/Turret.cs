using UnityEngine;

public class Turret : Tower
{
    protected override void Activate()
    {
        // Turret specific activation logic
    }

    public override void Attack(Enemy enemy)
    {
        if (IsEnemyInRange(enemy))
        {
            enemy.TakeDamage(config.damage);
        }
    }
    
    private bool IsEnemyInRange(Enemy enemy)
    {
        return Vector3.Distance(Position, enemy.Position) <= config.range;
    }
}