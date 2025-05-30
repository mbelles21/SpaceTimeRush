using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsMenu : MonoBehaviour
{
    public List<Button> buttonsList;
    private List<Upgrade> upgradeChoices;

    // Start is called before the first frame update
    void Start()
    {
        upgradeChoices = new List<Upgrade>();

        // set up so we can skip this if not getting an upgrade from level (mainly for healing levels)
        if (buttonsList.Count > 0)
        {
            foreach (Button button in buttonsList)
            {
                int r = GenerateRandomNumber(Upgrade.upgradesList.Count);
                Upgrade upgrade = Upgrade.upgradesList[r];
                upgradeChoices.Add(upgrade); // will be at index corresponding to button (i think)

                TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
                text.text = upgrade.name;
            }
        }
    }

    int GenerateRandomNumber(int size)
    {
        return Random.Range(0, size);
    }

    public void SelectUpgrade(Button clickedButton)
    {
        int i = buttonsList.IndexOf(clickedButton);
        if(i != -1) {
            int id = upgradeChoices[i].id;
            Upgrade.GetUpgrade(id);
            DisableButtons();
        }
        else {
            Debug.Log("button not in list");
        }
    }

    void DisableButtons()
    {
        foreach(Button button in buttonsList) {
            button.interactable = false;
        }
    }
}
