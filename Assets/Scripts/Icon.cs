using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Icon : MonoBehaviour
{
    // type 0 = normal, 1 = boss, 2 = meteor shower, 3 = healing level
    public int type;
    public Color hoverColor;
    public Color battleColor;
    public Color meteorColor;
    public Color healingColor;
    public Color bossColor;
    private Color iconColor;
    private Renderer iconRenderer;
    public SpriteRenderer childRenderer;
    public TextMeshProUGUI difficultyText; 

    public int colID;

    public AudioClip iconSelectSFX;
    private AudioSource src;

    private int iconDifficulty = 0; // might need to be made public eventually 
    private MapManager mapManager;

    private Vector3 defaultScale; // to check & limit scaling

    // public GameObject infoUI;
    // public TextMeshProUGUI infoText;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        src.clip = iconSelectSFX; // TODO: change if adding more sfx to icon
        iconRenderer = GetComponent<Renderer>();
        childRenderer = GetComponentInChildren<SpriteRenderer>();
        iconColor = battleColor; // default color

        mapManager = FindAnyObjectByType<MapManager>();
        iconDifficulty = GenerateDifficulty();

        defaultScale = transform.localScale;
    }

    /*
    // update used to prevent icons from being too small or too big
    void Update()
    {
        if(transform.rotation.z < 0) {
            transform.rotation = Quaternion.identity;
        }
        if(transform.rotation.z > 45) {
            transform.rotation = Quaternion.Euler(0, 0, 45);
        }

        // should only need to compare 1 value since scales uniformally
        if(transform.localScale.x < defaultScale.x) {
            transform.localScale = defaultScale;
        }
        if(transform.localScale.x > defaultScale.x * 1.5f) {
            transform.localScale = defaultScale * 1.5f;
        }
    }
    */

    private void OnMouseEnter()
    {
        // infoUI.SetActive(true);
        // GetLevelName();

        iconRenderer.material.color = hoverColor;
        // StartCoroutine(TransformIcon(1));
        transform.rotation = Quaternion.Euler(0, 0, 45);
        transform.localScale = defaultScale * 1.5f;
    }

    private void OnMouseExit()
    {
        // infoUI.SetActive(false);

        iconRenderer.material.color = Color.clear;
        // StartCoroutine(TransformIcon(-1));
        transform.rotation = Quaternion.identity;
        transform.localScale = defaultScale;
    }

    /* 
    private IEnumerator TransformIcon(int dir)
    {
        float duration = 0.25f;
        float elapsedTime = 0f;

        float curZRotation = transform.rotation.eulerAngles.z;
        float targetZRotation = Mathf.Clamp(curZRotation + 45 * dir, 0, 45);
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 0, targetZRotation);

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale;
        if(dir == 1) {
            targetScale = Vector3.Min(initialScale * 1.5f, defaultScale * 1.5f);
        }
        else {
            targetScale = Vector3.Max(initialScale / 1.5f, defaultScale);
        }

        while(elapsedTime < duration) {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime/duration);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime/duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        transform.localScale = targetScale;
    }
    */ 

    private void OnMouseDown()
    {
        // GameManager.LevelCount++;
        GameManager.LevelDifficulty = iconDifficulty; // set level difficulty
        mapManager.UpdatePrevIcons(colID);

        Debug.Log("level difficulty: " + iconDifficulty);
        Debug.Log("colID: " + colID);

        // Scene currentScene = SceneManager.GetActiveScene();
        // SceneManager.LoadScene(currentScene.name);
        src.Play();
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(1f); // wait for sfx to finish
        if(type == 0) {
            SceneManager.LoadScene("Level");
        }
        else if(type == 1) {
            SceneManager.LoadScene("Boss");
        }
        else if(type == 2) {
            SceneManager.LoadScene("MeteorLevel");
        }
        else if(type == 3) {
            SceneManager.LoadScene("HealingLevel");
        }
    }

    private int GenerateDifficulty()
    {
        int r = Random.Range(1, 4);
        difficultyText.text = r.ToString(); // to display difficulty number on the icon
        return r;
    }

    public void SetIconColor()
    {
        if(type == 0) {
            iconColor = battleColor;
            childRenderer.material.color = iconColor; // error here
        }
        else if(type == 1) {
            iconColor = bossColor;
            childRenderer.material.color = iconColor;
        }
        else if(type == 2) {
            iconColor = meteorColor;
            childRenderer.material.color = iconColor;
        }
        else if(type == 3) {
            iconColor = healingColor;
            childRenderer.material.color = iconColor;
        }
    }

    /*
    void GetLevelName()
    {
        if(type == 0) {
            infoText.text = "BATTLE";
        }
        else if(type == 1) {
            infoText.text = "BOSS";
        }
        else if(type == 2) {
            infoText.text = "METEOR SHOWER";
        }
        else if(type == 3) {
            infoText.text = "HEALING ZONE";
        }
    }
    */
}
