using System.Collections;
using _Contents.Scripts.Managers;
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
            GameManager.Instance.IsBuyingTower = true;
            StartCoroutine(HandleTowerPlacement(towerType));
        }
        else
        {
            Debug.Log("Not enough gold to buy this tower.");
        }
    }

    private IEnumerator HandleTowerPlacement(TowerType towerType)
    {
        while (GameManager.Instance.IsBuyingTower)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.z = 0; // Ensure the position is on the same plane as the game objects
                GameManager.Instance.PlaceTower(towerType, position);
                GameManager.Instance.IsBuyingTower = false;
            }
            yield return null;
        }
    }

    public void UpdatePriceTexts()
    {
        turretPrice.text = GameManager.Instance.GetTowerPrice(TowerType.Turret).ToString();
        minePrice.text = GameManager.Instance.GetTowerPrice(TowerType.Mine).ToString();
        mortarPrice.text = GameManager.Instance.GetTowerPrice(TowerType.Mortar).ToString();
    }
}