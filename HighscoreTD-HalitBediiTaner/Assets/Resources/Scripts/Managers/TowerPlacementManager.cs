using Resources.Scripts.Managers;
using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    public static TowerPlacementManager Instance { get; private set; }

    private TowerType selectedTowerType;
    public TowerType SelectedTowerType => selectedTowerType;
    private bool isPlacingTower;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartPlacingTower(TowerType towerType)
    {
        selectedTowerType = towerType;
        isPlacingTower = true;
        // Show placement areas
        ShowPlacementAreas(true);
    }

    public void PlaceTower(Vector3 position)
    {
        if (!isPlacingTower) return;

        GameManager.Instance.PlaceTower(selectedTowerType, position);
        isPlacingTower = false;
        // Hide placement areas
        ShowPlacementAreas(false);
    }

    private void ShowPlacementAreas(bool show)
    {
        if (selectedTowerType == TowerType.Mine)
        {
            foreach (var area in FindObjectsOfType<PathArea>())
            {
                area.SetHighlight(show);
            }
        }
        else
        {
            foreach (var area in FindObjectsOfType<PlacementArea>())
            {
                area.SetHighlight(show);
            }
        }
    }
}