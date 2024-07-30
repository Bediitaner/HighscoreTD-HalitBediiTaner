using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Config", menuName = "Enemy Config", order = 51)]
public class EnemyConfig : ScriptableObject
{
    public float currentHealth;
    public float maxHealth;
    public float speed;
    public int damage;
}