using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource src;
    public List<AudioClip> tracks;
    public bool playSelf;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();

        if(playSelf) {
            PlayMusic();
        }
    }

    void Update()
    {
        if(PauseMenu.GamePaused && !playSelf) {
            if(src.isPlaying) {
                src.Pause();
            }
        }
        else {
            if(!src.isPlaying) {
                src.UnPause();
            }
        }
    }

    public void PlayMusic()
    {
        int i = Random.Range(0, tracks.Count);
        AudioClip track = tracks[i];
        src.clip = track;
        src.loop = true;
        if(!playSelf) {
            src.volume = 0.4f; // only adjust volume if regular level
        }
        src.Play();
    }

    public void StopMusic()
    {
        src.Stop();
    }
}
