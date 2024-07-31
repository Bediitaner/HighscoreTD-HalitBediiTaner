using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    // Singleton instance
    public static PopupManager Instance { get; private set; }

    [System.Serializable]
    public struct Popup
    {
        public PopupType type;
        public GameObject popupObject;
    }

    public List<Popup> popups;
    private Dictionary<PopupType, GameObject> popupDictionary;

    public event EventHandler<PopupEventArgs> PopupShown;
    public event EventHandler<PopupEventArgs> PopupHidden;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePopups();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializePopups()
    {
        popupDictionary = new Dictionary<PopupType, GameObject>();
        foreach (var popup in popups)
        {
            popupDictionary.Add(popup.type, popup.popupObject);
            popup.popupObject.SetActive(false);
        }
    }

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

    public void HideAllPopups()
    {
        foreach (var popup in popupDictionary.Values)
        {
            popup.SetActive(false);
        }
    }
}

public class PopupEventArgs : EventArgs
{
    public PopupType PopupType { get; }

    public PopupEventArgs(PopupType popupType)
    {
        PopupType = popupType;
    }
}
