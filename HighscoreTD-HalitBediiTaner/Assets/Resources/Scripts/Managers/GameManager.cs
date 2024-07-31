using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager Instance { get; private set; }

        #endregion

        #region StateMachine

        public StateMachine StateMachine { get; private set; }

        #endregion

        #region Contents

        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private TowerConfigSO[] _towerConfigArray;

        [Space] [SerializeField] private Transform _enemySpawnPoint;
        [SerializeField] private Transform _castleTransform;
        public Transform CastleTransform => _castleTransform;

        [Space] [SerializeField] private TowerPurchaseHUD _towerPurchaseHUD;
        [SerializeField] private TopBarHUD _topBarHUD;

        // public Booster booster;

        #endregion

        #region Fields

        private Dictionary<TowerType, int> towerPrices;
        private Dictionary<TowerType, TowerConfigSO> towerConfigs;
        private Dictionary<TowerType, GameObject> towerPrefabs;

        private List<Tower> towers = new List<Tower>();
        private List<Enemy.Enemy> enemies = new List<Enemy.Enemy>();

        private bool _hasPlacedTower;
        private bool _isDataLoaded;
        private bool _isBuyingTower;

        [Space] [Header("Debug")] public int Gold;
        public int Score;
        public float EnemySpawnInterval;
        public int CurrentEnemyDifficulty;
        public int CastleHp = 100;

        //Firebase
        private DatabaseReference databaseReference;
        private string _userId;

        #endregion

        #region Properties

        public bool HasPlacedTower => _hasPlacedTower;
        public bool IsDataLoaded => _isDataLoaded;

        public bool IsBuyingTower
        {
            get => _isBuyingTower;
            set => _isBuyingTower = value;
        }

        #endregion


        #region Unity: Awake | Start | Update | OnApplicationQuit

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            _userId = SystemInfo.deviceUniqueIdentifier;

            LoadExistingData();
            StateMachine.ChangeState(new StartingState(this));
            
            UpdateTopBarHUDs();
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void OnApplicationQuit()
        {
            SaveCurrentData();
        }

        #endregion


        #region Init

        private void Init()
        {
            InitializeSingleton();
            InitializeStateMachine();
            InitializeTowerPrices();
            InitializeTowerConfigs();
            InitializeTowerPrefabs();
        }

        #region Init: Singleton

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

        #endregion

        #region Init: StateMachine

        private void InitializeStateMachine()
        {
            StateMachine = new StateMachine();
        }

        #endregion

        #region Init: Prices | Configs | Prefabs

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
            foreach (var config in _towerConfigArray)
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

        #endregion


        #region Management: Castle

        #region Damage: Castle

        public void DamageCastle(int damage)
        {
            CastleHp -= damage;
            if (CastleHp <= 0)
            {
                EndGame();
            }
        }

        #endregion

        #endregion

        #region Management: Tower

        #region Get: TowerPrice

        public int GetTowerPrice(TowerType towerType)
        {
            return towerPrices[towerType];
        }

        #endregion

        #region Get: TowerConfig: SO

        private TowerConfigSO GetTowerConfig(TowerType towerType)
        {
            return towerConfigs[towerType];
        }

        #endregion

        #region Tower: Instantiate

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

        #endregion

        #region Tower: Place

        public void PlaceTower(TowerType towerType, Vector3 position)
        {
            Tower tower = InstantiateTower(towerType);
            tower.Place(position);
            towers.Add(tower);

            towerPrices[towerType] += 10;
            _hasPlacedTower = true;
            UpdateTowerPurchaseGoldText();
        }

        #endregion

        #endregion

        #region Management: Enemy

        #region Start: SpawningEnemies

        public void StartSpawningEnemies()
        {
            StartCoroutine(SpawnEnemies());
        }

        #endregion

        #region Spawn: Enemies

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                yield return new WaitForSeconds(EnemySpawnInterval);
                Enemy.Enemy enemy = InstantiateEnemy();
                enemies.Add(enemy);
                EnemySpawnInterval = Mathf.Max(1.0f, EnemySpawnInterval - 0.1f);
                CurrentEnemyDifficulty++;
            }
        }

        #endregion

        #region Instantiate: Enemy

        private Enemy.Enemy InstantiateEnemy()
        {
            GameObject enemyObject = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)], _enemySpawnPoint.position, Quaternion.identity);
            Enemy.Enemy enemy = enemyObject.GetComponent<Enemy.Enemy>();
            return enemy;
        }

        #endregion

        #region Enemy: OnKilled

        public void OnEnemyKilled(Enemy.Enemy enemy)
        {
            enemies.Remove(enemy);
            Score += 10;
            Gold += 5;
            UpdateTopBarHUDs();
            // booster.AddBoosterAmount(10);
            UpdateTowerPurchaseGoldText();
        }

        #endregion

        #endregion

        #region Management: Game State

        #region Clear: CurrentGameState

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

        #endregion

        #region Game: End

        public void EndGame()
        {
            Debug.Log("Game Over");
            StateMachine.ChangeState(new GameEndState(this));
        }

        #endregion

        #endregion

        #region Management: UI

        public void UpdateTowerPurchaseGoldText()
        {
            _towerPurchaseHUD.UpdatePriceTexts();
        }

        private void UpdateTopBarHUDs()
        {
            _topBarHUD.SetGold(Gold);
            _topBarHUD.SetScore(Score);
        }

        private void ShowLoadGamePopup()
        {
            PopupManager.Instance.ShowPopup(PopupType.LoadSessionPopup);
        }

        #endregion

        #region Management: Data

        #region Create: DefaultSessionData

        private void CreateDefaultSessionData(string userId)
        {
            SessionData defaultData = new SessionData()
            {
                towers = new List<TowerData>(),
                currentGoldAmount = 4000,
                currentEnemyDifficulty = 1,
                currentScoreAmount = 0,
                currentSpawnInterval = 4.0f,
                currentCastleHP = 100
            };
            string json = JsonUtility.ToJson(defaultData);
            databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);

            Gold = defaultData.currentGoldAmount;
            CurrentEnemyDifficulty = defaultData.currentEnemyDifficulty;
            Score = defaultData.currentScoreAmount;
            EnemySpawnInterval = defaultData.currentSpawnInterval;
            CastleHp = defaultData.currentCastleHP;
        }

        #endregion

        #region Load: GameData

        private void LoadGameData(SessionData data)
        {
            Gold = data.currentGoldAmount;
            Score = data.currentScoreAmount;
            EnemySpawnInterval = data.currentSpawnInterval;
            CurrentEnemyDifficulty = data.currentEnemyDifficulty;
            CastleHp = data.currentCastleHP;


            foreach (var towerData in data.towers)
            {
                Tower tower = InstantiateTower(towerData.towerType);
                tower.Place(towerData.position);
                tower.level = towerData.level;
                towers.Add(tower);
            }
        }

        #endregion

        #region Load: ExistingData

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

        #endregion

        #region Load: NewDefaultData

        public void LoadNewData()
        {
            CreateDefaultSessionData(_userId);
        }

        #endregion

        #region Save: CurrentData

        private void SaveCurrentData()
        {
            string userId = _userId;
            SessionData currentData = GetCurrentSessionData();
            string json = JsonUtility.ToJson(currentData);
            databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
        }

        #endregion

        #region Get: Current: SessionData

        private SessionData GetCurrentSessionData()
        {
            SessionData currentData = new SessionData
            {
                towers = new List<TowerData>(),
                currentGoldAmount = Gold,
                currentEnemyDifficulty = CurrentEnemyDifficulty,
                currentScoreAmount = Score,
                currentSpawnInterval = EnemySpawnInterval,
                currentCastleHP = CastleHp
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

        #endregion
    }
}