using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI levelsText;
    private int levelNum;
    private int highScore;
    public GameObject tutorialWindow;

    void Start()
    {
        levelNum = PlayerPrefs.GetInt("farthestlevel", 1);
        highScore = PlayerPrefs.GetInt("highscore", 0);

        string formattedScore = highScore.ToString("D4");
        highscoreText.text = $"HiScore: {formattedScore}";
        levelsText.text = $"Level {levelNum}";
    }

    public void PlayGame()
    {
        GameManager.GameDifficulty = 1;
        GameManager.LevelCount = 1;
        ScoreManager.HighScore = PlayerPrefs.GetInt("highscore", 0);

        MapManager.ColIndex = 0; // reset map columns
        MapManager.NumOfCols = 0;
        MapManager.PrevIcons.Clear();
        GameManager.SectorCount = 0;

        StartCoroutine(LoadNextScene());
    }

    public void LoadGame()
    {
        GameManager.GameDifficulty = PlayerPrefs.GetInt("difficulty", 1);
        GameManager.LevelCount = PlayerPrefs.GetInt("levels", 0);
        ScoreManager.TotalScore = PlayerPrefs.GetInt("totalscore", 0);

        // TODO: make sure getting all player prefs (compare w/ GameManager)

        StartCoroutine(LoadNextScene());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1f); // waiting so button sfx can play
        SceneManager.LoadScene("Map");
    }

    public void ToggleTutorial()
    {
        tutorialWindow.SetActive(!tutorialWindow.activeSelf);
    }
}
