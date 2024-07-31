using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Contents.Scripts
{
    public class TestUI : MonoBehaviour
    {
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private TMP_InputField _scoreInput;
        [SerializeField] private TMP_InputField _goldInput;
        [SerializeField] private TMP_InputField _enemyDifficultyInput;
        [SerializeField] private TMP_InputField _spawnIntervalInput;


        void Start()
        {
            AddEvents();
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }


        #region Event: Save

        private void SaveGameData()
        {
            Debug.Log("SaveGameData");
            // DatabaseManager.Instance.SaveGameData(_scoreInput, _goldInput, _enemyDifficultyInput, _spawnIntervalInput);
        }

        #endregion

        #region Event: Load

        private void LoadGameData()
        {
            Debug.Log("LoadGameData");
            // StartCoroutine(DatabaseManager.Instance.LoadGameDataCoroutine(_scoreInput, _goldInput, _enemyDifficultyInput, _spawnIntervalInput));
        }

        #endregion

        #region Events: Add | Remove

        private void AddEvents()
        {
            _saveButton.onClick.AddListener(SaveGameData);
            _loadButton.onClick.AddListener(LoadGameData);
        }

        private void RemoveEvents()
        {
            _saveButton.onClick.RemoveListener(SaveGameData);
            _loadButton.onClick.RemoveListener(LoadGameData);
        }

        #endregion
    }
}