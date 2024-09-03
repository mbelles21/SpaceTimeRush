using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    [Header("Results")]
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI speedScoreText;
    public TextMeshProUGUI slowScoreText;

    public static int HighScore;
    public static int TotalScore;
    private int currentScore;
    private int levelScore = 0; 
    private int speedScore = 0;
    private int slowScore = 0;

    [Header("Multipliers")]
    public int speedMultiplier = 1000;
    public int slowMultiplier = 1000;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HighScore: " + HighScore);
        currentScore = TotalScore;
        UpdateScore();

        Enemy.OnEnemyDied += EnemyOnOnEnemyDied;
    }

    void EnemyOnOnEnemyDied(int points, Transform t) 
    {
        Debug.Log("got " + points + " points!");
        currentScore += points;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentScore > levelScore) {
            levelScore = currentScore;
            UpdateScore();
        }
    }

    void UpdateScore()
    {
        string formattedScore = levelScore.ToString("D4");
        scoreText.text = $"Score: {formattedScore}";
    }

    public void GetSpeedScore(float time)
    {
        speedScore = speedMultiplier * Mathf.CeilToInt(time);
    }

    public void GetSlowScore(float time)
    {
        slowScore = slowMultiplier * Mathf.CeilToInt(time);
    }

    public void AddBossScore(int points)
    {
        // TODO: find a way to do this w/o adding to levelScore?
        levelScore += points;
    }

    public void GetBossScore(float time)
    {
        // adding to level score for now (might change later idk)
        // might also want to change how score is calculated in general
        int timePoints = Mathf.CeilToInt(time * 100);
        float timeScore = 10000 - timePoints; // TODO: change how this is calculated
        levelScore += Mathf.CeilToInt(timeScore);
    }

    public void CalculateScore()
    {
        UpdateResultScores();

        TotalScore = levelScore + speedScore - slowScore;
        string formattedScore = TotalScore.ToString("D4");
        totalScoreText.text = $"Score: {formattedScore}";

        if(TotalScore > HighScore) {
            HighScore = TotalScore;
        }
    }

    void UpdateResultScores()
    {
        speedScoreText.text = "Speed: " + speedScore;
        slowScoreText.text = "Slow: " + slowScore;
    }

    public void PickupScore(int points)
    {
        currentScore += points;
    }
}
