using UnityEngine;

[CreateAssetMenu(fileName = "TowerConfig", menuName = "ScriptableObjects/New Tower", order = 1)]
public class TowerConfigSO : ScriptableObject
{
    public TowerType towerType;
    public int cost;
    public float maxRange;
    public float deadRange;
    public float damage;
    public float fireRate;
    public float cooldown;
}