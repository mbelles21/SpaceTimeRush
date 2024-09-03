using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused;
    public GameObject ui;
    public AudioClip pauseSFX;
    public AudioSource src;

    void Start()
    {
        // src = GetComponent<AudioSource>();
        GamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        src.clip = pauseSFX;
        ui.SetActive(!ui.activeSelf);
        if(ui.activeSelf) {
            Time.timeScale = 0f;
            GamePaused = true;
            src.Play();
        }
        else {
            int speed = SpeedController.CurrentSpeed;
            if(speed == 0) {
                Time.timeScale = 1f;
            }
            else if(speed == 1) {
                Time.timeScale = 0.5f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
            else if(speed == 2) {
                Time.timeScale = 1.5f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
            GamePaused = false;
            src.Play();
        }
    }

    public void ResumeGame()
    {
        ToggleMenu();
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("highscore", ScoreManager.HighScore);
        PlayerPrefs.SetInt("farthestlevel", GameManager.FarthestLevel);
        // PlayerPrefs.SetInt("sector", GameManager.SectorCount);
        PlayerPrefs.Save();

        SceneManager.LoadScene("MainMenu");
    }
}
