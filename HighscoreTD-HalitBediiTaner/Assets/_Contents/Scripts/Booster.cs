using UnityEngine;

public class Booster : MonoBehaviour
{
    public float maxBoosterAmount = 100f;
    public float currentBoosterAmount;
    public float boosterDuration = 5f; // Booster etkisinin süresi
    private bool isBoosterActive = false;

    void Update()
    {
        if (isBoosterActive)
        {
            boosterDuration -= Time.deltaTime;
            if (boosterDuration <= 0)
            {
                DeactivateBooster();
            }
        }
    }

    public void AddBoosterAmount(float amount)
    {
        if (!isBoosterActive)
        {
            currentBoosterAmount += amount;
            if (currentBoosterAmount >= maxBoosterAmount)
            {
                ActivateBooster();
            }
        }
    }

    private void ActivateBooster()
    {
        isBoosterActive = true;
        currentBoosterAmount = 0;

        // Tüm kulelerin yenilenme süresini geçici olarak azalt
        foreach (var tower in GameManager.Instance.Towers)
        {
            tower.config.cooldown /= 2;
        }
    }

    private void DeactivateBooster()
    {
        isBoosterActive = false;
        boosterDuration = 5f;

        // Tüm kulelerin yenilenme süresini eski haline getir
        foreach (var tower in GameManager.Instance.Towers)
        {
            tower.config.cooldown *= 2;
        }
    }
}