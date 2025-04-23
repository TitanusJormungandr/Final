using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class WinTrigger : MonoBehaviour
{
    public GameObject winScreen; 
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        //Hides the win screen
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //When the player enters the win zone
        if (other.CompareTag("Player"))
        {
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        //Check if the win screen is not null and display it
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }

        //Get the score from ScoreManager
        int finalScore = ScoreManager.instance.score;

        //Update the score on the win screen UI
        if (scoreText != null)
        {
            scoreText.text = "Final Score: " + finalScore.ToString();
        }
    }
}
