using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        if (FirebaseManager.Instance != null)
        {
            SessionData sessionData = await FirebaseManager.Instance.LoadSession();
            if (sessionData != null)
            {
                PromptRestoreSession(sessionData);
            }
            else
            {
                StartNewSession();
            }
        }
        else
        {
            Debug.LogError("FirebaseManager instance is null!");
        }
    }

    void PromptRestoreSession(SessionData sessionData)
    {
        PopupManager.Instance.ShowPopup(PopupType.LoadSessionPopup);
    }

    void RestoreSession(SessionData sessionData)
    {
        foreach (var tower in sessionData.towers)
        {
            // Use the tower placement function to place towers on the map
        }

        // Restore other session data
        // Set gold amount, enemy difficulty, score, and spawn interval
        Debug.Log("Session successfully restored.");
    }

    void StartNewSession()
    {
        Debug.Log("New session started.");
    }

    void OnApplicationQuit()
    {
        SaveCurrentSession();
    }

    void SaveCurrentSession()
    {
        SessionData currentData = new SessionData
        {
            towers = new List<TowerData>(),
            currentGoldAmount = 200,
            currentEnemyDifficulty = 3,
            currentScoreAmount = 1000,
            currentSpawnInterval = 2.5f
        };
        FirebaseManager.Instance.SaveSession(currentData);
        Debug.Log("Current session saved.");
    }
}