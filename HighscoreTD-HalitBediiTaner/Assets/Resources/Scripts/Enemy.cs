// Enemy.cs

using Resources.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyConfigSO config;
    private NavMeshAgent agent;
    public int damageToCastle = 10;

    public Vector3 Position { get { return transform.position; } }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.speed = config.speed;
        config.currentHealth = config.maxHealth;

        agent.SetDestination(GameManager.Instance.CastleTransform.position);
    }

    void Update()
    {
    }

    public void TakeDamage(float damageAmount)
    {
        config.currentHealth -= damageAmount;
        if (config.currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        GameManager.Instance.OnEnemyKilled(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Castle"))
        {
            GameManager.Instance.DamageCastle(damageToCastle);
            Die();
        }
    }
}