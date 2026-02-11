using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreOverTime : MonoBehaviour
{
    public float scorePerSecond = 1f; // Amount of score to add per second
    private float currentScore = 0f; // Current score value
    private float elapsedTime = 0f; // Time elapsed since the last score update
    //
    [Header("Display score in the UI")]
    public TextMeshProUGUI scoreText;
    public HighScoreScriptableObject highScore;
    public TextMeshProUGUI highScoreText;
    

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the score if needed
        currentScore = 0f;
        elapsedTime = 0f; 
        if(highScore.highScore > 0)
        {
            Debug.Log("Current High Score: " + highScore.highScore);
            highScoreText.text = highScore.highScore.ToString("000000");
        }
        else
        {
            Debug.Log("No high score set yet.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentScore += scorePerSecond * Time.deltaTime; // Increment score based on time
        scoreText.text = currentScore.ToString("000000");
        if (Input.GetKeyDown(KeyCode.R))
        {
            highScore.highScore = 0;
            highScoreText.text = highScore.highScore.ToString("000000");
        }
    }
    
    private void OnDestroy()
    {
        // Save the high score when the object is destroyed
        if (currentScore > highScore.highScore)
        {
            highScore.highScore = Mathf.RoundToInt(currentScore);
            Debug.Log("New High Score: " + highScore.highScore);
        }
    }
}
