using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Managers
{
    public class TowerPlacementManager : MonoBehaviour
    {
        #region Singleton

        public static TowerPlacementManager Instance { get; private set; }

        #endregion

        #region Fields

        private TowerType selectedTowerType;
        public TowerType SelectedTowerType => selectedTowerType;
    
        private bool isPlacingTower;
    
        private List<Vector3> placedTowerPositions = new List<Vector3>();

        #endregion

        #region Unity: Awake

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

        #endregion

        
        #region Tower: StartPlacing

        public void StartPlacingTower(TowerType towerType)
        {
            selectedTowerType = towerType;
            isPlacingTower = true;

            ShowPlacementAreas(true);
        }

        #endregion

        #region Tower: Place

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

            ShowPlacementAreas(false);
        }

        #endregion

        #region Is: TowerPlacedAtPosition

        private bool IsTowerPlacedAtPosition(Vector3 position)
        {
            foreach (var placedPosition in placedTowerPositions)
            {
                if (Vector3.Distance(placedPosition, position) < 0.1f)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Show: Placement: Areas

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

        #endregion
    }
}