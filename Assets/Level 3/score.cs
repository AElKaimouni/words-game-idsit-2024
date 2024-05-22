using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    private int scoreValue = 0;
    private const int totalScoreNeeded = 3;

    public static score Instance; // Singleton instance

    void Awake()
    {
        Instance = this; // Assign singleton instance
    }

    void Start()
    {
        UpdateScoreText();
    }

    public void WordPlacedCorrectly(string correspondingWord)
    {
        scoreValue++;
        if (scoreValue >= totalScoreNeeded)
        {
            DisplayWinText();
        }
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + scoreValue + " / " + "3";
    }

    void DisplayWinText()
    {

        SceneManager.LoadScene("Level 4");
    }
}
