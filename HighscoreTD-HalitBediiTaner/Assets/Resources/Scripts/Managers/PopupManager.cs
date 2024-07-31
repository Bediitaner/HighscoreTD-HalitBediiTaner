using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Managers
{
    public class PopupEventArgs : EventArgs
    {
        public PopupType PopupType { get; }

        public PopupEventArgs(PopupType popupType)
        {
            PopupType = popupType;
        }
    }

    public class PopupManager : MonoBehaviour
    {
        #region Singleton

        public static PopupManager Instance { get; private set; }

        #endregion

        #region Struct: Popup

        [Serializable]
        public struct Popup
        {
            public PopupType type;
            public GameObject popupObject;
        }

        #endregion

        #region Contents

        public List<Popup> popups;
        private Dictionary<PopupType, GameObject> popupDictionary;

        #endregion

        #region Events

        public event EventHandler<PopupEventArgs> PopupShown;
        public event EventHandler<PopupEventArgs> PopupHidden;

        #endregion

        #region Unity: Awake

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitPopups();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        
        #region Init: Popups

        void InitPopups()
        {
            popupDictionary = new Dictionary<PopupType, GameObject>();
            foreach (var popup in popups)
            {
                popupDictionary.Add(popup.type, popup.popupObject);
                popup.popupObject.SetActive(false);
            }
        }

        #endregion

        #region Show: Popup

        public void ShowPopup(PopupType type)
        {
            if (!popupDictionary.ContainsKey(type))
            {
                Debug.LogError($"Popup with type {type} not found!");
                return;
            }

            GameObject popupObject = popupDictionary[type];
            if (popupObject == null)
            {
                Debug.LogError($"Popup object for type {type} is null!");
                return;
            }

            Debug.Log($"Showing popup of type {type}");
            popupObject.SetActive(true);
            Debug.Log($"Popup {type} is now active: {popupObject.activeSelf}");
            Debug.Log($"Popup {type} is in hierarchy: {popupObject.transform.parent != null}");
            PopupShown?.Invoke(this, new PopupEventArgs(type));
        }

        #endregion

        #region Hide: Popup

        public void HidePopup(PopupType type)
        {
            if (!popupDictionary.ContainsKey(type))
            {
                Debug.LogError($"Popup with type {type} not found!");
                return;
            }

            popupDictionary[type].SetActive(false);
            PopupHidden?.Invoke(this, new PopupEventArgs(type));
        }

        #endregion

        #region Hide: All: Popups

        public void HideAllPopups()
        {
            foreach (var popup in popupDictionary.Values)
            {
                popup.SetActive(false);
            }
        }

        #endregion
    }
}