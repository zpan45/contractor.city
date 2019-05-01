using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public AudioClip backgroundMusic;
    public AudioClip worksiteSound;
    public AudioClip moneyEarning;
    public AudioClip readyButtonSound;
    public AudioClip buttonSound;
    public AudioClip jobCompleted;
    public AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        /*
        backgroundMusic = Resources.Load<AudioClip>("background_music");
        worksiteSound = Resources.Load<AudioClip>("worksite_sounds");
        moneyEarning = Resources.Load<AudioClip>("money_earned");
        readyButtonSound = Resources.Load<AudioClip>("ready_button");
        buttonSound = Resources.Load<AudioClip>("button_onclick");
        jobCompleted = Resources.Load<AudioClip>("job_completed");
        audioSrc = GetComponent<AudioSource>();
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string clip)
    {
        Debug.Log(clip);
        switch (clip)
        {
            case "background":
                audioSrc.PlayOneShot(backgroundMusic);
                break;
            case "working":
                audioSrc.PlayOneShot(worksiteSound);
                break;
            case "earning money":
                audioSrc.PlayOneShot(moneyEarning);
                break;
            case "ready button":
                audioSrc.PlayOneShot(readyButtonSound);
                break;
            case "button":
                audioSrc.PlayOneShot(buttonSound);
                break;
            case "job completed":
                audioSrc.PlayOneShot(jobCompleted);
                break;
        }
    }

    public void StopSound(string clip)
    {
        switch (clip)
        {
            case "working":
                //audioSrc.Stop(worksiteSound);
                break;
        }
    }

    public void ButtonSound() {
        PlaySound("button");
    }
}
