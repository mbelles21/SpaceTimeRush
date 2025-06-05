using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public List<GameObject> bossPrefabs = new List<GameObject>();
    public Vector3 startPos;

    public void SpawnBoss()
    {
        int r = Random.Range(0, bossPrefabs.Count);
        GameObject bossPrefab = bossPrefabs[r];
        Instantiate(bossPrefab, startPos, bossPrefab.transform.rotation);
    }
}
