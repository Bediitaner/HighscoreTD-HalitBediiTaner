using Resources.Scripts.Managers;
using UnityEngine;
using System.Collections.Generic;

public class TowerPlacementManager : MonoBehaviour
{
    public static TowerPlacementManager Instance { get; private set; }

    private TowerType selectedTowerType;
    public TowerType SelectedTowerType => selectedTowerType;
    private bool isPlacingTower;
    private List<Vector3> placedTowerPositions = new List<Vector3>();

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

        if (IsTowerPlacedAtPosition(position))
        {
            Debug.Log("A tower is already placed at this position.");
            return;
        }

        GameManager.Instance.PlaceTower(selectedTowerType, position);
        placedTowerPositions.Add(position);
        isPlacingTower = false;
        // Hide placement areas
        ShowPlacementAreas(false);
    }

    private bool IsTowerPlacedAtPosition(Vector3 position)
    {
        foreach (var placedPosition in placedTowerPositions)
        {
            if (Vector3.Distance(placedPosition, position) < 0.1f) // Adjust the threshold as needed
            {
                return true;
            }
        }
        return false;
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