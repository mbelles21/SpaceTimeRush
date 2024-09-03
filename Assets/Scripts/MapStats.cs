using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapStats : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI sectorText;

    // Start is called before the first frame update
    void Start()
    {
        string formattedScore = ScoreManager.TotalScore.ToString("D4");
        scoreText.text = $"Score: {formattedScore}";

        sectorText.text = "Sector: " + GameManager.SectorCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
