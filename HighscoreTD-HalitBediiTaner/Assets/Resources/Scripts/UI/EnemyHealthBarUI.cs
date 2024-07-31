using Resources.Scripts.Enemy;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Image foregroundImage; // Ön plan image'i referans alınacak.
    private Enemy enemy; // Düşmanın canını takip edecek.
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        enemy = GetComponentInParent<Enemy>(); // Enemy scriptine erişim.
    }

    void Update()
    {
        if (enemy != null)
        {
            // Can barını düşmanın mevcut canına göre güncelle.
            foregroundImage.fillAmount = enemy.Config.currentHealth / enemy.Config.maxHealth;
        }
        
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
        }
    }
    
    
}