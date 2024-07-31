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

        DataManager.Instance.LoadData().ContinueWith(task =>
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
                DataManager.Instance.SaveData(defaultData);
            }
        });
    }
}