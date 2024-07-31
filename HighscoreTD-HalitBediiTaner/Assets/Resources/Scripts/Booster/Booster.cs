using UnityEngine;

namespace Resources.Scripts.Booster
{
    public class Booster : MonoBehaviour
    {
        #region Fields

        public float maxBoosterAmount = 100f;
        public float currentBoosterAmount;
        public float boosterDuration = 5f;
        private bool isBoosterActive = false;

        #endregion

        
        #region Unity: Update

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

        #endregion

        
        #region Add: Booster: Amount

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

        #endregion

        #region Booster: Activate

        private void ActivateBooster()
        {
            isBoosterActive = true;
            currentBoosterAmount = 0;

            // // Temporarily reduce the regeneration time of all towers
            // // foreach (var tower in GameManager.Instance.Towers)
            // {
            //     tower.config.cooldown /= 2;
            // }
        }

        #endregion

        #region Booster: Deactivate

        private void DeactivateBooster()
        {
            isBoosterActive = false;
            boosterDuration = 5f;

            // Restore the regeneration time of all towers
            // foreach (var tower in GameManager.Instance.Towers)
            // {
            //     tower.config.cooldown *= 2;
            // }
        }

        #endregion
    }
}