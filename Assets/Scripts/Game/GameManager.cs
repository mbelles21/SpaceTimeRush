using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int GameDifficulty = 1;
    public static int LevelDifficulty = 0;
    public static int LevelCount = 0;
    public static int FarthestLevel = 0;
    public static int SectorCount = 0;
    // TODO: add furthest sector
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Difficulty: " + GameDifficulty);
        Debug.Log("Level: " + LevelCount);
        
        if(SectorCount % 5 == 0 && SectorCount != 0) {
            GameDifficulty++;
        }
    }

    public void ContinueGame()
    {
        // GameDifficulty++; // TODO: change so increments less often (like every floor or something)
        LevelCount++;
        if(LevelCount > FarthestLevel) {
            FarthestLevel = LevelCount;
        }

        // max level limit
        if(LevelCount > int.MaxValue - 1) {
            Debug.Log("you beat the game!");
            SceneManager.LoadScene("MainMenu");
        }
        else {
            // Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene("Map");
        }
    }

    public void SaveAndQuitGame()
    {
        PlayerPrefs.SetInt("difficulty", GameDifficulty);
        PlayerPrefs.SetInt("levels", LevelCount);
        PlayerPrefs.SetInt("farthestlevel", FarthestLevel);
        PlayerPrefs.SetInt("highscore", ScoreManager.HighScore);
        PlayerPrefs.SetInt("totalscore", ScoreManager.TotalScore);
        PlayerPrefs.SetInt("sector", SectorCount);

        PlayerPrefs.SetInt("previconscount", MapManager.PrevIcons.Count);
        for(int i = 0; i < MapManager.PrevIcons.Count; i++) {
            PlayerPrefs.SetInt("previcon" + i, MapManager.PrevIcons[i]);
        }

        PlayerPrefs.Save();

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("highscore", ScoreManager.HighScore);
        PlayerPrefs.SetInt("farthestlevel", FarthestLevel);
        // PlayerPrefs.SetInt("sector", SectorCount);
        PlayerPrefs.Save();

        SceneManager.LoadScene("MainMenu");
    }
}
