using UnityEngine;
using System.Collections.Generic;
using Resources.Scripts.Enemy;

public class Mine : Tower
{
    protected override void Activate()
    {
        gameObject.transform.position = Position;
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
        return Vector3.Distance(Position, enemy.Position) <= 0.5f;
    }

    private void Explode()
    {
        List<Enemy> enemiesInRange = GetEnemiesInRange();
        foreach (Enemy enemy in enemiesInRange)
        {
            var distance = Vector3.Distance(Position, enemy.Position);
            var damage = CalculateDamage(distance);
            enemy.TakeDamage(damage);
        }
        Debug.Log("Mine exploded and will be destroyed.");
        Destroy(gameObject);
    }

    private List<Enemy> GetEnemiesInRange()
    {
        //TODO: @Halit
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