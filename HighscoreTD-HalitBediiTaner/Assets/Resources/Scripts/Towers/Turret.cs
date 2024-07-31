using UnityEngine;

public class Turret : Tower
{
    protected override void Activate()
    {
        gameObject.transform.position = Position;
        Debug.Log("Turret activated and placed at position: " + Position);
    }

    public override void Attack(Enemy enemy)
    {
        if (IsEnemyInRange(enemy))
        {
            FireAtEnemy(enemy);
        }
    }

    private bool IsEnemyInRange(Enemy enemy)
    {
        return Vector3.Distance(Position, enemy.Position) <= config.maxRange;
    }

    private void FireAtEnemy(Enemy enemy)
    {
        // Logic to fire at the closest enemy
    }
}