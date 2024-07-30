using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarHUD : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI scoreText;

    private int gold;
    private int score;

    void Start()
    {
        UpdateGoldText();
        UpdateScoreText();
    }

    void Update()
    {
        // This can be used to update the HUD in real-time if needed.
    }

    public void SetGold(int amount)
    {
        gold = amount;
        UpdateGoldText();
    }

    public void SetScore(int amount)
    {
        score = amount;
        UpdateScoreText();
    }

    private void UpdateGoldText()
    {
        goldText.text = "Gold: " + gold;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}