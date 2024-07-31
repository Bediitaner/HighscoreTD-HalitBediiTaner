using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Config", menuName = "ScriptableObjects/New Enemy", order = 51)]
public class EnemyConfigSO : ScriptableObject
{
    public float currentHealth;
    public float maxHealth;
    public float speed;
    public int damage;
}