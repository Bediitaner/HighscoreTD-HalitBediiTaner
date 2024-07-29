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

    private async void InitializeDatabase()
    {
        _userId = SystemInfo.deviceUniqueIdentifier;
        _reference = FirebaseDatabase.DefaultInstance.RootReference;

        var task = _reference.Child("users").Child(_userId).GetValueAsync();
        await Task.WhenAll(task);

        if (task.Result == null || !task.Result.Exists)
        {
            SessionData defaultData = new SessionData
            {
                towers = new List<TowerData>(),
                currentGoldAmount = 0,
                currentEnemyDifficulty = 1,
                currentScoreAmount = 0,
                currentSpawnInterval = 1.0f
            };
            SaveSession(defaultData);
        }
    }

    public void SaveSession(SessionData data)
    {
        if (_reference == null || string.IsNullOrEmpty(_userId))
        {
            Debug.LogError("Firebase reference or user ID is not initialized.");
            return;
        }

        string json = JsonUtility.ToJson(data);
        _reference.Child("users").Child(_userId).SetRawJsonValueAsync(json);
    }

    public async Task<SessionData> LoadSession()
    {
        if (_reference == null || string.IsNullOrEmpty(_userId))
        {
            Debug.LogError("Firebase reference or user ID is not initialized.");
            return null;
        }

        var task = _reference.Child("users").Child(_userId).GetValueAsync();
        await Task.WhenAll(task);

        if (task.Exception != null)
        {
            Debug.LogWarning(task.Exception);
            return null;
        }
        else
        {
            DataSnapshot snapshot = task.Result;
            return JsonUtility.FromJson<SessionData>(snapshot.GetRawJsonValue());
        }
    }
}