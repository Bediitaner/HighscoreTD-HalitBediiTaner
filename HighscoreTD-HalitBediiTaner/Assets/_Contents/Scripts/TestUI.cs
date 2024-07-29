using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        DatabaseManager.Instance.CreateGameData(_scoreInput, _goldInput, _enemyDifficultyInput, _spawnIntervalInput);
    }

    #endregion

    #region Event: Load

    private void LoadGameData()
    {
        Debug.Log("LoadGameData");
        StartCoroutine(LoadGameDataCoroutine());
    }

    private IEnumerator LoadGameDataCoroutine()
    {
        yield return DatabaseManager.Instance.GetScore(score =>
        {
            _scoreInput.text = score.ToString();
        });

        yield return DatabaseManager.Instance.GetGold(gold =>
        {
            _goldInput.text = gold.ToString();
        });

        yield return DatabaseManager.Instance.GetEnemyDifficulty(enemyDifficulty =>
        {
            _enemyDifficultyInput.text = enemyDifficulty.ToString();
        });

        yield return DatabaseManager.Instance.GetSpawnInterval(spawnInterval =>
        {
            _spawnIntervalInput.text = spawnInterval.ToString();
        });
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