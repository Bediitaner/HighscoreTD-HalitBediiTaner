using UnityEngine;
using System.Collections.Generic;

public class Mine : Tower
{
    protected override void Activate()
    {
        // Mine specific activation logic
        // This could include setting up visual indicators or enabling collision detection
        Debug.Log("Mine activated and placed at position: " + Position);
    }

    public override void Attack(Enemy enemy)
    {
        if (IsEnemyOnMine(enemy))
        {
            Explode();
        }
    }

    private bool IsEnemyOnMine(Enemy enemy)
    {
        return Vector3.Distance(Position, enemy.Position) <= 0.5f; // Example range for mine activation
    }

    private void Explode()
    {
        List<Enemy> enemiesInRange = GetEnemiesInRange();
        foreach (Enemy enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(Position, enemy.Position);
            float damage = CalculateDamage(distance);
            enemy.TakeDamage(damage);
        }
        // Logic for explosion effects
        Debug.Log("Mine exploded and will be destroyed.");
        Destroy(gameObject);
    }

    private List<Enemy> GetEnemiesInRange()
    {
        // Implement logic to get all enemies within maxRange
        // This is a placeholder and should be replaced with actual game logic
        return new List<Enemy>();
    }

    private float CalculateDamage(float distance)
    {
        // Example damage calculation based on distance
        float maxDamage = config.damage;
        float minDamage = config.damage * 0.5f; // Minimum damage is 50% of max damage
        float damageRange = maxDamage - minDamage;
        float damage = maxDamage - (distance / config.maxRange) * damageRange;
        return Mathf.Max(damage, minDamage);
    }
}