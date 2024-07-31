using UnityEngine;
using System.Collections.Generic;
using Resources.Scripts.Enemy;

public class Mortar : Tower
{
    protected override void Activate()
    {
        gameObject.transform.position = Position;
        Debug.Log("Mortar activated and placed at position: " + Position);
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
        //TODO: @Halit - Logic
        Debug.Log("Mortar shell fired at position: " + targetPosition);
        Explode(targetPosition);
    }

    private void Explode(Vector3 explosionCenter)
    {
        List<Enemy> enemiesInRange = GetEnemiesInRange(explosionCenter);
        foreach (Enemy enemy in enemiesInRange)
        {
            var distance = Vector3.Distance(explosionCenter, enemy.Position);
            var damage = CalculateDamage(distance);
            enemy.TakeDamage(damage);
        }
        //TODO: @Halit - Explosion Effect 
        Debug.Log("Mortar shell exploded at position: " + explosionCenter);
    }

    private List<Enemy> GetEnemiesInRange(Vector3 explosionCenter)
    {
        //TODO: @Halit - Logic
        return new List<Enemy>();
    }

    private float CalculateDamage(float distance)
    {
        var maxDamage = config.damage;
        var minDamage = config.damage * 0.5f;
        var damageRange = maxDamage - minDamage;
        var damage = maxDamage - (distance / config.maxRange) * damageRange;
        return Mathf.Max(damage, minDamage);
    }
}