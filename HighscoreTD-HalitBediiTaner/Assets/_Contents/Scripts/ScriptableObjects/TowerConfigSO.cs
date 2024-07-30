using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Config", menuName = "Tower Config", order = 51)]
public class TowerConfig : ScriptableObject
{
    public TowerType towerType;
    public float range;
    public float damage;
    public float cooldown;
}