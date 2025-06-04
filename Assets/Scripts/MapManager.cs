using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform startPoint;
    public int columns = 3; // max should be 8
    public static int MaxColumns;
    public static int ColIndex;
    public float columnSpacing = 5f;
    public float rowSpacing = 2f;

    // index 0: disabled, 1: cleared, 2: level
    [Header("Icon Prefabs")]
    public List<GameObject> icons;
    public Vector2 ChanceOfBattle;
    public Vector2 ChanceOfMeteor;
    public Vector2 ChanceOfHealing;

    // public static int SelectedIconID;
    public static List<int> PrevIcons = new List<int>(); // stores prev icon types
    public static int NumOfCols = 0; 

    private void Start()
    {
        MaxColumns = columns;
        ColIndex++;
        Debug.Log("ColIndex=" + ColIndex);
        Vector3 startPos = startPoint.position;
        
        if (ColIndex == 1) // TODO: will probably need to change this logic
        {
            SpawnIcons(startPos);
        }
        else
        {
            SpawnDisabled(startPos);
        }
    }

    public void SpawnIcons(Vector3 startPos)
    {
        NumOfCols++;

        if(PrevIcons.Count < NumOfCols*3) {
            // check for boss level 
            if (ColIndex == MaxColumns)
            {
                SpawnBoss(startPos);
                return;
            }
            
            float iconSpacing = 0f;
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    iconSpacing = rowSpacing;
                }
                if (i == 1)
                {
                    iconSpacing = 0f;
                }
                if (i == 2)
                {
                    iconSpacing = -rowSpacing;
                }

                Vector3 spawnPos = new Vector3(startPos.x + columnSpacing, iconSpacing, startPos.z);
                GameObject newIcon = Instantiate(icons[2], spawnPos, Quaternion.identity);

                // to keep track of which icon gets clicked
                Icon iconComp = newIcon.GetComponent<Icon>();
                iconComp.colID = i;
                iconComp.type = GenerateIconTypes();
                iconComp.SetIconColor();

                // to remember prev icon type
                int iconType = iconComp.type;
                PrevIcons.Add(iconType);
            }
        }
        else {
            // spawn prev saved icons

            if (ColIndex == MaxColumns) // not sure if this condition will ever be true in this context but I'm leaving it here
            {
                SpawnBoss(startPos);
                return;
            }

            float iconSpacing = 0f;
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    iconSpacing = 3f;
                }
                if (i == 1)
                {
                    iconSpacing = 0f;
                }
                if (i == 2)
                {
                    iconSpacing = -3f;
                }

                Vector3 spawnPos = new Vector3(startPos.x + columnSpacing, iconSpacing, startPos.z);
                GameObject newIcon = Instantiate(icons[2], spawnPos, Quaternion.identity);

                // to keep track of which icon gets clicked
                Icon iconComp = newIcon.GetComponent<Icon>();
                iconComp.colID = i;

                // set prev icon types
                iconComp.type = PrevIcons[i + (NumOfCols * 3)]; // last 3 indexes in list for icons that spawn
                iconComp.SetIconColor();
            }
        }
    }

    public void SpawnBoss(Vector3 startPos)
    {
        Vector3 spawnPos = new Vector3(startPos.x + columnSpacing, startPos.y, startPos.z);
        GameObject bossIcon = Instantiate(icons[2], spawnPos, Quaternion.identity);
        // TODO: boss icon
        Icon iconComp = bossIcon.GetComponent<Icon>();
        iconComp.type = 1; // last 3 indexes in list for icons that spawn
        iconComp.SetIconColor();
    }

    public void SpawnDisabled(Vector3 startPos)
    {
        float iconSpacing = 5f;
        int iconsSpawned = 0;

        for(int i = 0; i < PrevIcons.Count; i++) {
            if(i % 3 == 0) {
                iconSpacing = 3f;
            }
            if(i % 3 == 1) {
                iconSpacing = 0f;
            }
            if(i % 3 == 2) {
                iconSpacing = -3f;
            }
            
            Vector3 spawnPos = new Vector3(startPos.x + columnSpacing, iconSpacing, startPos.z);
            if(PrevIcons[i] == 8) {
                Instantiate(icons[0], spawnPos, Quaternion.identity); // instantiate disabled icon
            }
            else if(PrevIcons[i] == 9) {
                Instantiate(icons[1], spawnPos, Quaternion.identity); // instantiate cleared icon
            }
            iconsSpawned++;

            if(iconsSpawned % 3 == 0) {
                // update startPos
                startPos = new Vector3(startPos.x + columnSpacing, startPos.y, startPos.z);
            }
        }

        SpawnIcons(startPos);
    }

    public void UpdatePrevIcons(int selectedID)
    {
        // 8 == disabled, 9 == cleared (for now -- type values not yet decided)
        int startIndex = (NumOfCols * 3) - 3;
        for(int i = startIndex; i < PrevIcons.Count; i++) {
            if(i == selectedID + startIndex) {
                PrevIcons[i] = 9;
            }
            else {
                PrevIcons[i] = 8;
            }
        } 
    }

    int GenerateIconTypes()
    {
        int type = 0;
        int r = Random.Range(1, 101);
        if(r >= ChanceOfBattle.x && r < ChanceOfBattle.y) {
            type = 0;
        }
        else if(r >= ChanceOfMeteor.x && r < ChanceOfMeteor.y) {
            type = 2;
        }
        else if(r >= ChanceOfHealing.x && r < ChanceOfHealing.y) {
            type = 3;
        }

        return type;
    }
}
