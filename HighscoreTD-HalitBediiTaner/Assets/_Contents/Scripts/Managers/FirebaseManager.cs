using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    private string _userId;
    private DatabaseReference _reference;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        _userId = SystemInfo.deviceUniqueIdentifier;
        _reference = FirebaseDatabase.DefaultInstance.RootReference;

        LoadData().ContinueWith(task =>
        {
            if (task.Result == null)
            {
                SessionData defaultData = new SessionData
                {
                    towers = new List<TowerData>(),
                    currentGoldAmount = 100,
                    currentEnemyDifficulty = 1,
                    currentScoreAmount = 0,
                    currentSpawnInterval = 5.0f
                };
                SaveData(defaultData);
            }
        });
    }

    public void SaveData(SessionData data)
    {
        string json = JsonUtility.ToJson(data);
        _reference.Child("users").Child(_userId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data saved successfully.");
            }
            else
            {
                Debug.LogError("Failed to save data: " + task.Exception);
            }
        });
    }

    public Task<SessionData> LoadData()
    {
        var task = _reference.Child("users").Child(_userId).GetValueAsync();
        return task.ContinueWith(t =>
        {
            if (t.IsCompleted && t.Result.Exists)
            {
                string json = t.Result.GetRawJsonValue();
                SessionData data = JsonUtility.FromJson<SessionData>(json);
                return data;
            }
            else
            {
                Debug.LogWarning("No data found for user: " + _userId);
                return null;
            }
        });
    }
}
