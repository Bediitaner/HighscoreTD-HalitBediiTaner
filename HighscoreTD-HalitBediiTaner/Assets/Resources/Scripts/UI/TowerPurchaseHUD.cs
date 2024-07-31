using System.Collections;
using Resources.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPurchaseHUD : MonoBehaviour
{
    public Button turretButton;
    public Button mineButton;
    public Button mortarButton;
    public TextMeshProUGUI turretPrice;
    public TextMeshProUGUI minePrice;
    public TextMeshProUGUI mortarPrice;

    private void Start()
    {
        turretButton.onClick.AddListener(() => OnTowerButtonClicked(TowerType.Turret));
        mineButton.onClick.AddListener(() => OnTowerButtonClicked(TowerType.Mine));
        mortarButton.onClick.AddListener(() => OnTowerButtonClicked(TowerType.Mortar));

        UpdatePriceTexts();
    }

    private void OnTowerButtonClicked(TowerType towerType)
    {
        if (GameManager.Instance.gold >= GameManager.Instance.GetTowerPrice(towerType))
        {
            GameManager.Instance.gold -= GameManager.Instance.GetTowerPrice(towerType);
            TowerPlacementManager.Instance.StartPlacingTower(towerType);
            UpdatePriceTexts();
        }
        else
        {
            Debug.Log("Not enough gold to buy this tower.");
        }
    }

    public void UpdatePriceTexts()
    {
        turretPrice.text = GameManager.Instance.GetTowerPrice(TowerType.Turret).ToString();
        minePrice.text = GameManager.Instance.GetTowerPrice(TowerType.Mine).ToString();
        mortarPrice.text = GameManager.Instance.GetTowerPrice(TowerType.Mortar).ToString();
    }
}