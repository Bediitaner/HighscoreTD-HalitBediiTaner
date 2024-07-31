using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public StateMachine StateMachine { get; private set; }
        private DatabaseReference databaseReference;
        private string _userId;


        private Dictionary<TowerType, int> towerPrices;
        private Dictionary<TowerType, TowerConfigSO> towerConfigs;
        private Dictionary<TowerType, GameObject> towerPrefabs;
        private bool hasPlacedTower;
        private bool isDataLoaded;
        private bool isBuyingTower;
        private List<Tower> towers = new List<Tower>();
        private List<Enemy> enemies = new List<Enemy>();
        public int gold;
        public int score;
        public float enemySpawnInterval;
        public int currentEnemyDifficulty;
        private int _castleHp = 100;
        public Transform enemySpawnPoint;
        public Transform[] waypoints;
        public GameObject[] enemyPrefabs;
        public Booster booster;

        [SerializeField] private Transform castleTransform;
        public Transform CastleTransform => castleTransform;

        [SerializeField] public TowerPurchaseHUD towerPurchaseHUD;

        [SerializeField] private TopBarHUD _topBarHUD;

        [SerializeField] private TowerConfigSO[] towerConfigArray;

        private void Awake()
        {
            InitializeSingleton();
            InitializeStateMachine();
            InitializeGameSettings();
        }

        private void Start()
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            _userId = SystemInfo.deviceUniqueIdentifier;

            LoadExistingData();
            StateMachine.ChangeState(new StartingState(this));
        }

        private void Update()
        {
            StateMachine.Update();
            Debug.Log(StateMachine.GetCurrentState());
        }

        private void OnApplicationQuit()
        {
            SaveCurrentData();
        }

        #region Initialization

        private void InitializeSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void InitializeStateMachine()
        {
            StateMachine = new StateMachine();
        }

        private void InitializeGameSettings()
        {
            InitializeTowerPrices();
            InitializeTowerConfigs();
            InitializeTowerPrefabs();
        }

        private void InitializeTowerPrices()
        {
            towerPrices = new Dictionary<TowerType, int>
            {
                { TowerType.Turret, 100 },
                { TowerType.Mine, 50 },
                { TowerType.Mortar, 150 }
            };
        }

        private void InitializeTowerConfigs()
        {
            towerConfigs = new Dictionary<TowerType, TowerConfigSO>();
            foreach (var config in towerConfigArray)
            {
                towerConfigs[config.towerType] = config;
            }
        }

        private void InitializeTowerPrefabs()
        {
            towerPrefabs = new Dictionary<TowerType, GameObject>
            {
                { TowerType.Turret, UnityEngine.Resources.Load<GameObject>("Prefabs/Tower_Turret") },
                { TowerType.Mine, UnityEngine.Resources.Load<GameObject>("Prefabs/Tower_Mine") },
                { TowerType.Mortar, UnityEngine.Resources.Load<GameObject>("Prefabs/Tower_Mortar") }
            };

            // Check if any prefab failed to load
            foreach (var prefab in towerPrefabs)
            {
                if (prefab.Value == null)
                {
                    Debug.LogError($"Failed to load prefab for {prefab.Key}");
                }
            }
        }

        #endregion

        #region Castle Management

        public void DamageCastle(int damage)
        {
            _castleHp -= damage;
            if (_castleHp <= 0)
            {
                EndGame();
            }
        }

        #endregion

        #region Tower Management

        public int GetTowerPrice(TowerType towerType)
        {
            return towerPrices[towerType];
        }

        public void PlaceTower(TowerType towerType, Vector3 position)
        {
            Tower tower = InstantiateTower(towerType);
            tower.Place(position);
            towers.Add(tower);

            // Increase the price for the next tower of the same type
            towerPrices[towerType] += 10;

            // Set hasPlacedTower to true
            hasPlacedTower = true;

            // Update the gold text in the UI
            UpdateGoldText();
        }

        private Tower InstantiateTower(TowerType towerType)
        {
            if (!towerPrefabs.ContainsKey(towerType))
            {
                Debug.LogError($"Tower type {towerType} not found in prefabs dictionary");
                return null;
            }

            GameObject towerObject = towerPrefabs[towerType];
            if (towerObject == null)
            {
                Debug.LogError($"Prefab for tower type {towerType} is null");
                return null;
            }

            GameObject instantiatedTowerObject = Instantiate(towerObject);
            if (instantiatedTowerObject == null)
            {
                Debug.LogError($"Failed to instantiate tower of type {towerType}");
                return null;
            }

            Tower tower = instantiatedTowerObject.GetComponent<Tower>();
            if (tower == null)
            {
                Debug.LogError($"Tower component not found on instantiated object of type {towerType}");
                return null;
            }

            TowerConfigSO config = GetTowerConfig(towerType);
            tower.Initialize(config);
            return tower;
        }

        private TowerConfigSO GetTowerConfig(TowerType towerType)
        {
            return towerConfigs[towerType];
        }

        public bool HasPlacedTower => hasPlacedTower;
        public bool IsDataLoaded => isDataLoaded;

        public bool IsBuyingTower
        {
            get => isBuyingTower;
            set => isBuyingTower = value;
        }

        #endregion

        #region Enemy Management

        public void OnEnemyKilled(Enemy enemy)
        {
            enemies.Remove(enemy);
            score += 10;
            gold += 5;
            UpdateTopBarHUD();
            booster.AddBoosterAmount(10);
            UpdateGoldText();
        }

        private void UpdateTopBarHUD()
        {
            _topBarHUD.SetGold(gold);
            _topBarHUD.SetScore(score);
        }

        public void StartSpawningEnemies()
        {
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                yield return new WaitForSeconds(enemySpawnInterval);
                Enemy enemy = InstantiateEnemy();
                enemies.Add(enemy);
                enemySpawnInterval = Mathf.Max(1.0f, enemySpawnInterval - 0.1f);
                currentEnemyDifficulty++;
            }
        }

        private Enemy InstantiateEnemy()
        {
            GameObject enemyObject = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], enemySpawnPoint.position, Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            return enemy;
        }

        #endregion

        #region Game State Management

        private void ClearCurrentGameState()
        {
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
        }

        private void LoadGameData(SessionData data)
        {
            gold = data.currentGoldAmount;
            score = data.currentScoreAmount;
            enemySpawnInterval = data.currentSpawnInterval;
            currentEnemyDifficulty = data.currentEnemyDifficulty;
            _castleHp = data.currentCastleHP;


            foreach (var towerData in data.towers)
            {
                Tower tower = InstantiateTower(towerData.towerType);
                tower.Place(towerData.position);
                tower.level = towerData.level;
                towers.Add(tower);
            }
        }

        public void EndGame()
        {
            Debug.Log("Game Over");
            StateMachine.ChangeState(new GameEndState(this));
        }

        #endregion

        #region UI Management

        public void UpdateGoldText()
        {
            towerPurchaseHUD.UpdatePriceTexts();
        }

        #endregion

        #region Data Management

        private SessionData CreateDefaultSessionData()
        {
            return new SessionData
            {
                towers = new List<TowerData>(),
                currentGoldAmount = 250,
                currentEnemyDifficulty = 1,
                currentScoreAmount = 0,
                currentSpawnInterval = 5.0f,
                currentCastleHP = 100
            };
        }


        private void CreateAndSaveDefaultData(string userId)
        {
            SessionData defaultData = CreateDefaultSessionData();
            string json = JsonUtility.ToJson(defaultData);
            databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);

            gold = defaultData.currentGoldAmount;
            currentEnemyDifficulty = defaultData.currentEnemyDifficulty;
            score = defaultData.currentScoreAmount;
            enemySpawnInterval = defaultData.currentSpawnInterval;
            _castleHp = defaultData.currentCastleHP;
        }

        private void ShowLoadGamePopup()
        {
            PopupManager.Instance.ShowPopup(PopupType.LoadSessionPopup);
        }

        public void LoadExistingData()
        {
            databaseReference.Child("users").Child(_userId).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        SessionData data = JsonUtility.FromJson<SessionData>(snapshot.GetRawJsonValue());
                        LoadGameData(data);
                    }
                    else
                    {
                        LoadNewData();
                    }
                }
            });
        }

        public void LoadNewData()
        {
            CreateAndSaveDefaultData(_userId);
        }

        public void SaveCurrentData()
        {
            string userId = _userId;
            SessionData currentData = GetCurrentSessionData();
            string json = JsonUtility.ToJson(currentData);
            databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
        }

        private SessionData GetCurrentSessionData()
        {
            SessionData currentData = new SessionData
            {
                towers = new List<TowerData>(),
                currentGoldAmount = gold,
                currentEnemyDifficulty = currentEnemyDifficulty,
                currentScoreAmount = score,
                currentSpawnInterval = enemySpawnInterval,
                currentCastleHP = _castleHp
            };

            foreach (var tower in towers)
            {
                TowerData towerData = new TowerData
                {
                    towerType = tower.config.towerType,
                    position = tower.transform.position,
                    level = tower.level
                };
                currentData.towers.Add(towerData);
            }

            return currentData;
        }

        #endregion
    }
}