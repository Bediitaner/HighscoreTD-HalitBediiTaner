using Resources.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Contents.Scripts
{
    public class LoadSessionPopup : MonoBehaviour
    {
        [SerializeField] private Button _newButton;
        [SerializeField] private Button _loadButton;

        void Start()
        {
            AddEvents();
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }

        #region Hide

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        #endregion
        

        #region Event: New: Game

        private void NewGameData()
        {
            GameManager.Instance.LoadNewData();
            Hide();
        }

        #endregion

        #region Event: Load: Game

        private void LoadGameData()
        {
            GameManager.Instance.LoadExistingData();
            Hide();
        }

        #endregion

        #region Events: Add | Remove

        private void AddEvents()
        {
            _newButton.onClick.AddListener(NewGameData);
            _loadButton.onClick.AddListener(LoadGameData);
        }

        private void RemoveEvents()
        {
            _newButton.onClick.RemoveListener(NewGameData);
            _loadButton.onClick.RemoveListener(LoadGameData);
        }

        #endregion
    }
}