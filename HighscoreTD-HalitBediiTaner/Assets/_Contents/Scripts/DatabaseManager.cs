using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using TMPro;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

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
    }

    void Start()
    {
        _userId = SystemInfo.deviceUniqueIdentifier;
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateGameData(TMP_InputField scoreInput, TMP_InputField goldInput, TMP_InputField enemyDifficultyInput, TMP_InputField spawnIntervalInput)
    {
        GameData gameData = new GameData
        {
            Score = int.Parse(scoreInput.text),
            Gold = int.Parse(goldInput.text),
            EnemyDifficulty = int.Parse(enemyDifficultyInput.text),
            SpawnInterval = float.Parse(spawnIntervalInput.text)
        };

        string json = JsonUtility.ToJson(gameData);
        _reference.Child("users").Child(_userId).SetRawJsonValueAsync(json);
    }
    
    public IEnumerator GetScore(Action<int> callback)
    {
        var task = _reference.Child("users").Child(_userId).Child("Score").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogWarning(task.Exception);
        }
        else
        {
            DataSnapshot snapshot = task.Result;
            callback(int.Parse(snapshot.Value.ToString()));
        }
    }
    
    public IEnumerator GetGold(Action<int> callback)
    {
        var task = _reference.Child("users").Child(_userId).Child("Gold").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogWarning(task.Exception);
        }
        else
        {
            DataSnapshot snapshot = task.Result;
            callback(int.Parse(snapshot.Value.ToString()));
        }
    }
    
    public IEnumerator GetEnemyDifficulty(Action<int> callback)
    {
        var task = _reference.Child("users").Child(_userId).Child("EnemyDifficulty").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogWarning(task.Exception);
        }
        else
        {
            DataSnapshot snapshot = task.Result;
            callback(int.Parse(snapshot.Value.ToString()));
        }
    }
    
    public IEnumerator GetSpawnInterval(Action<float> callback)
    {
        var task = _reference.Child("users").Child(_userId).Child("SpawnInterval").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogWarning(task.Exception);
        }
        else
        {
            DataSnapshot snapshot = task.Result;
            callback(float.Parse(snapshot.Value.ToString()));
        }
    }
}