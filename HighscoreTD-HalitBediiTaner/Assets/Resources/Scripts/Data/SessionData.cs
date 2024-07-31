using System.Collections.Generic;

[System.Serializable]
public class SessionData
{
    public List<TowerData> towers;
    public int currentGoldAmount;
    public int currentEnemyDifficulty;
    public int currentScoreAmount;
    public float currentSpawnInterval;
}