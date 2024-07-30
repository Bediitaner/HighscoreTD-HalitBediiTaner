using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Config", menuName = "Enemy Config", order = 51)]
public class EnemyConfig : ScriptableObject
{
    public float health;
    public float speed;
    public int damage;
}