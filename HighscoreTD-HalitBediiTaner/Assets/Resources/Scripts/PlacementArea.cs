using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    private Renderer areaRenderer;
    public Material defaultMaterial;
    public Material highlightMaterial;

    private void Awake()
    {
        areaRenderer = GetComponent<Renderer>();
        SetHighlight(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleInput(touch.position);
            }
        }
    }

    private void HandleInput(Vector3 inputPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
        {
            if (TowerPlacementManager.Instance == null)
            {
                Debug.Log("TowerPlacementManager.Instance is null.");
                return;
            }
            if (TowerType.Mine == TowerPlacementManager.Instance.SelectedTowerType)
            {
                Debug.Log("This area is not for mines.");
                return;
            }

            TowerPlacementManager.Instance.PlaceTower(transform.position);
        }
    }

    public void SetHighlight(bool highlight)
    {
        areaRenderer.material = highlight ? highlightMaterial : defaultMaterial;
    }
}