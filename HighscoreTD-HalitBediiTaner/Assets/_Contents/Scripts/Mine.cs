using UnityEngine;

public class Mine : Tower
{
    protected override void Activate()
    {
        // Mine specific activation logic
    }

    public override void Attack(Enemy enemy)
    {
        if (IsEnemyOnMine(enemy))
        {
            enemy.TakeDamage(config.damage);
            Explode();
        }
    }
    
    private bool IsEnemyOnMine(Enemy enemy)
    {
        return Vector3.Distance(Position, enemy.Position) <= 0.5f; // Example range for mine activation
    }
    
    private void Explode()
    {
        // Logic for explosion
        Destroy(gameObject);
    }
}