using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public void SetGold(int gold)
    {
        Debug.Log($"Setting Gold: {gold}");
        goldText.text = gold.ToString();
    }

    public void SetScore(int score)
    {
        Debug.Log($"Setting Score: {score}");
        scoreText.text = score.ToString();
    }
}