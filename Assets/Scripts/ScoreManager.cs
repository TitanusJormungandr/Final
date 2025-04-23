using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Adds score + updates the UI
    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
