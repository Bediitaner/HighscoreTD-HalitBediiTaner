using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Transform _mainTower;
    public Transform MainTower => _mainTower;

    private List<Tower> towers;
    public List<Tower> Towers => towers;
    private List<Enemy> enemies;
    public int gold;
    public int score;
    public float enemySpawnInterval;
    public int currentEnemyDifficulty;
    public Transform enemySpawnPoint;
    public Transform[] waypoints; // Waypoints for enemies to follow
    public GameObject[] enemyPrefabs;
    public Booster booster; // Booster objesi

    //Views
    [SerializeField]
    private TopBarHUD _topBarHUD;
    
    // Scriptable Object configurations
    private TowerConfig[] towerConfigs;

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

    private void Start()
    {
        InitializeGame();
    }

    private async void InitializeGame()
    {
        towers = new List<Tower>();
        enemies = new List<Enemy>();

        SessionData data = await FirebaseManager.Instance.LoadData();

        if (data != null)
        {
            gold = data.currentGoldAmount;
            score = data.currentScoreAmount;
            enemySpawnInterval = data.currentSpawnInterval;
            currentEnemyDifficulty = data.currentEnemyDifficulty;

            foreach (var towerData in data.towers)
            {
                Tower tower = InstantiateTower(towerData.towerType);
                tower.Place(towerData.position);
                tower.level = towerData.level;
                towers.Add(tower);
            }
        }
        else
        {
            Debug.Log("No saved data found. Starting a new game.");
        }

        StartCoroutine(SpawnEnemies());
    }

    public void StartGame()
    {
        // Logic to start the game
    }

    public void EndGame()
    {
        // Logic to end the game
        StopAllCoroutines();
    }

    public void UpdateGame()
    {
        // Logic to update the game state
    }

    private void OnApplicationQuit()
    {
        SaveSession();
    }

    public void SaveSession()
    {
        SessionData data = new SessionData
        {
            towers = new List<TowerData>(),
            currentGoldAmount = gold,
            currentEnemyDifficulty = currentEnemyDifficulty,
            currentScoreAmount = score,
            currentSpawnInterval = enemySpawnInterval
        };

        foreach (var tower in towers)
        {
            TowerData towerData = new TowerData
            {
                towerType = tower.config.towerType,
                position = tower.Position,
                level = tower.level
            };
            data.towers.Add(towerData);
        }

        FirebaseManager.Instance.SaveData(data);
    }

    public async void LoadSession()
    {
        SessionData data = await FirebaseManager.Instance.LoadData();

        if (data != null)
        {
            // Clear current game state
            foreach (var tower in towers)
            {
                Destroy(tower.gameObject);
            }

            towers.Clear();

            foreach (var enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }

            enemies.Clear();

            // Load saved game state
            foreach (var towerData in data.towers)
            {
                // Instantiate and place towers based on saved data
                Tower tower = InstantiateTower(towerData.towerType);
                tower.Place(towerData.position);
                tower.level = towerData.level;
                towers.Add(tower);
            }

            gold = data.currentGoldAmount;
            currentEnemyDifficulty = data.currentEnemyDifficulty;
            score = data.currentScoreAmount;
            enemySpawnInterval = data.currentSpawnInterval;
        }
    }

    private Tower InstantiateTower(TowerType towerType)
    {
        // Logic to instantiate a tower based on its type
        TowerConfig config = GetTowerConfig(towerType);
        GameObject towerObject = new GameObject(towerType.ToString());
        Tower tower = towerObject.AddComponent<Tower>(); // Add the appropriate tower script (Turret, Mine, Mortar) dynamically
        tower.Initialize(config);
        return tower;
    }

    private TowerConfig GetTowerConfig(TowerType towerType)
    {
        foreach (var config in towerConfigs)
        {
            if (config.towerType == towerType)
            {
                return config;
            }
        }

        return null;
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemySpawnInterval);

            // Logic to spawn an enemy
            Enemy enemy = InstantiateEnemy();
            enemies.Add(enemy);

            // Adjust spawn interval and difficulty based on game progression
            enemySpawnInterval = Mathf.Max(1.0f, enemySpawnInterval - 0.1f);
            currentEnemyDifficulty++;
        }
    }

    private Enemy InstantiateEnemy()
    {
        // Logic to instantiate an enemy

        GameObject enemyObject = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], enemySpawnPoint.position, Quaternion.identity);

        Enemy enemy = enemyObject.GetComponent<Enemy>();
        return enemy;
    }

    public void OnEnemyKilled(Enemy enemy)
    {
        enemies.Remove(enemy);
        score += 10; // Example score increment
        gold += 5; // Example gold increment

        // Update the TopBarHUD with the new values
        UpdateTopBarHUD();

        // Booster barını doldur
        booster.AddBoosterAmount(10); // Örneğin, her öldürmede 10 birim ekle
    }
    private void UpdateTopBarHUD()
    {
        _topBarHUD.SetGold(gold);
        _topBarHUD.SetScore(score);
    }
}