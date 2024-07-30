using UnityEngine;
using System.Collections.Generic;

public class Mortar : Tower
{
    protected override void Activate()
    {
        // Mortar-specific activation logic
    }

    public override void Attack(Enemy enemy)
    {
        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
        if (IsWithinEffectiveRange(distanceToEnemy))
        {
            FireMortarShell(enemy.Position);
        }
        else
        {
            Debug.Log("Enemy is out of mortar's effective range.");
        }
    }

    private bool IsWithinEffectiveRange(float distance)
    {
        return distance > config.deadRange && distance <= config.maxRange;
    }

    private void FireMortarShell(Vector3 targetPosition)
    {
        // Logic to fire a mortar shell
        Debug.Log("Mortar shell fired at position: " + targetPosition);
        Explode(targetPosition);
    }

    private void Explode(Vector3 explosionCenter)
    {
        List<Enemy> enemiesInRange = GetEnemiesInRange(explosionCenter);
        foreach (Enemy enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(explosionCenter, enemy.Position);
            float damage = CalculateDamage(distance);
            enemy.TakeDamage(damage);
        }
        // Logic for explosion effects
        Debug.Log("Mortar shell exploded at position: " + explosionCenter);
    }

    private List<Enemy> GetEnemiesInRange(Vector3 explosionCenter)
    {
        // Implement logic to get all enemies within the explosion radius
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