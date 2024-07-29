using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int Score;
    public int Gold;
    public int EnemyDifficulty;
    public float SpawnInterval;
    // public List<TowerData> Towers;

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>
        {
            { "Score", Score },
            { "Gold", Gold },
            { "EnemyDifficulty", EnemyDifficulty },
            { "SpawnInterval", SpawnInterval },
            // { "Towers", Towers.ConvertAll(tower => tower.ToDictionary()) }
        };
        return dict;
    }
}