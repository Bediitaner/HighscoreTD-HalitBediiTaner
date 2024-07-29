using System.Numerics;

[System.Serializable]
public class TowerData
{
    public string towerType;
    public Vector3 position;
    public int level;

    public TowerData(string towerType, Vector3 position, int level)
    {
        this.towerType = towerType;
        this.position = position;
        this.level = level;
    }
}