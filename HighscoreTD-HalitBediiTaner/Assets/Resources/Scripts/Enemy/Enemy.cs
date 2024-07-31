using Resources.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Resources.Scripts.Enemy
{
    public class Enemy : MonoBehaviour
    {
        #region Content

        [SerializeField]
        private EnemyConfigSO config;
        public EnemyConfigSO Config { get { return config; } }

        #endregion

        #region Fields

        private NavMeshAgent agent;
        private int damageToCastle = 10;
        public Vector3 Position { get { return transform.position; } }

        #endregion
        

        #region Unity: Awake | Start | OnTriggerEnter

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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Castle"))
            {
                GameManager.Instance.DamageCastle(damageToCastle);
                Die();
            }
        }
    
        #endregion

        
        #region Take: Damage

        public void TakeDamage(float damageAmount)
        {
            config.currentHealth -= damageAmount;
            if (config.currentHealth <= 0)
            {
                Die();
            }
        }

        #endregion

        #region Die

        private void Die()
        {
            Destroy(gameObject);
            GameManager.Instance.OnEnemyKilled(this);
        }

        #endregion
    }
}