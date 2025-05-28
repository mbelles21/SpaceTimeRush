using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public List<GameObject> pickupsList;
    
    // Start is called before the first frame update
    void Start()
    {
        Enemy.OnEnemyDied += EnemyOnOnEnemyDied;
    }

    void OnDestroy()
    {
        Enemy.OnEnemyDied -= EnemyOnOnEnemyDied;
    }

    void EnemyOnOnEnemyDied(int points, Transform t) 
    {
        // Debug.Log(t.position);
        SpawnPickup(t);
    }

    void SpawnPickup(Transform t)
    {
        int r = Random.Range(1, 101);
        if(r <= 10) {
            Debug.Log(pickupsList[0].name + " spawned");
            Instantiate(pickupsList[0], t.position, Quaternion.identity);
        }
        else if(r > 10 && r <= 20) {
            Debug.Log(pickupsList[1].name + " spawned");
            Instantiate(pickupsList[1], t.position, Quaternion.identity);
        }
        else if(r > 20 && r <= 30) {
            Debug.Log(pickupsList[2].name + " spawned");
            Instantiate(pickupsList[2], t.position, Quaternion.identity);
        }
        else if(r > 30 && r <= 40) {
            Debug.Log(pickupsList[3].name + " spawned");
            Instantiate(pickupsList[3], t.position, Quaternion.identity);
        }
        else {
            Debug.Log("no pickup spawned");
        }
    }
}
