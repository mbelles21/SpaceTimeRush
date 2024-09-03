using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGManager : MonoBehaviour
{
    public List<GameObject> backgrounds;
    public float bgSpeed = 1.5f;
    public float starSpeed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenu.GamePaused = false;
        Time.timeScale = 1f;
        
        foreach(GameObject g in backgrounds) {
            StartCoroutine(MoveBG(g));
        }
    }

    IEnumerator MoveBG(GameObject bg)
    {
        float yCoord = bg.transform.position.y;

        float speed;
        if(bg.tag == "background") {
            speed = bgSpeed;
        }
        else {
            // will be star objects
            speed = starSpeed;
        }

        while(true) {
            bg.transform.position = Vector3.MoveTowards(bg.transform.position, new Vector3(-45, yCoord, 0), speed * Time.deltaTime);
            if(bg.transform.position.x <= -42) {
                if(bg.tag != "background") {
                    float var = GetRandVar();
                    yCoord += var; 
                    bg.transform.position = new Vector3(40.96f, yCoord, 0); // different x coord if star obj
                }
                else {
                    bg.transform.position = new Vector3(120.88f, yCoord, 0);
                }
            }
            yield return null;
        }
    }

    float GetRandVar()
    {
        return Random.Range(-5f, 5f);
    }
}
