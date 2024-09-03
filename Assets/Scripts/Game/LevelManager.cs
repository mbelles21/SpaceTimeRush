using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // type 0 = normal, 1 = boss, 2 = meteor shower, 3 = healing level
    public int levelType = 0;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;

    public float timer = 120f;
    public bool timerRunning = true;

    public GameObject resultsUI;

    public float countdownTime = 3f;
    public TextMeshProUGUI countdownText;
    public AudioClip countdownSFX;
    private AudioSource src;

    private ScoreManager scoreManager;
    private SpeedController speedController;
    private MusicManager musicManager;

    public GameObject gameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        levelText.text = "Level " + GameManager.LevelCount;
        // Debug.Log("LevelCount = " + GameManager.LevelCount);

        src = GetComponent<AudioSource>();
        scoreManager = GetComponent<ScoreManager>();
        speedController = GameObject.FindGameObjectWithTag("Player").GetComponent<SpeedController>();
        musicManager = GetComponent<MusicManager>();

        if(levelType == 1) {
            timer = 0;
        }

        timerRunning = false; // timer won't start until starting countdown complete
        StartCoroutine(Countdown(countdownTime)); // temp implmentation
    }

    // Update is called once per frame
    void Update()
    {
        if(levelType == 1) {
            if(timerRunning) {
                timer += Time.deltaTime;
                UpdateTimerText();
            }
            else {
                timerRunning = false; // idk if this is necessary
                UpdateTimerText();
            }
        }
        else {
            if(timerRunning) {
                if(timer > 0) {
                    timer -= Time.deltaTime;
                    UpdateTimerText();
                }
                else {
                    timer = 0;
                    timerRunning = false;
                    UpdateTimerText();
                
                    Debug.Log("level complete");
                    resultsUI.SetActive(true);
                    PauseMenu.GamePaused = true; // to prevent shooting while clicking buttons (haven't tested for movement yet -- doesn't halt movement, will need to do separately)
                    musicManager.StopMusic();

                    CalculateScore();
                }
            }
        }
    }

    void UpdateTimerText()
    {
        float mins = Mathf.FloorToInt(timer/60);
        float secs = Mathf.FloorToInt(timer%60);
        timerText.text = string.Format("{0:0}:{1:00}", mins, secs);
    }

    // for when boss is defeated
    public void BossDefeated(int points) 
    {
        timerRunning = false;
        resultsUI.SetActive(true);
        PauseMenu.GamePaused = true;
        musicManager.StopMusic();
        scoreManager.AddBossScore(points);
        CalculateScore();

        MapManager.ColIndex = 0; // reset map columns
        MapManager.NumOfCols = 0;
        MapManager.PrevIcons.Clear();
        GameManager.SectorCount++;
    }

    void CalculateScore()
    {
        float timeSped = speedController.GetSpeedTime();
        float timeSlowed = speedController.GetSlowTime();

        scoreManager.GetSpeedScore(timeSped);
        scoreManager.GetSlowScore(timeSlowed);

        if(levelType == 1) {
            scoreManager.GetBossScore(timer);
        }

        scoreManager.CalculateScore();
    }

    IEnumerator Countdown(float seconds)
    {
        while(seconds > 0) {
            src.clip = countdownSFX;
            src.volume = 1f;
            src.Play();

            if(seconds-1 == 0) {
                countdownText.text = "GO!";
            }
            else {
                countdownText.text = Mathf.CeilToInt(seconds-1).ToString();
            }
            yield return new WaitForSeconds(1f);
            seconds -= 1f;
        }
        countdownText.text = "";
        timerRunning = true;
        // Debug.Log("level started");
        musicManager.PlayMusic();
    }

    public bool GetTimerRunning()
    {
        return timerRunning;
    }

    public void GameOver()
    {
        timerRunning = false;
        gameOverUI.SetActive(true);
        PauseMenu.GamePaused = true;
        musicManager.StopMusic();
        // TODO: play game over music
    }
}
