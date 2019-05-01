using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{


    public AudioClip moneyEarning;
    public AudioClip star;
    public AudioClip readyButtonSound;
    public AudioClip buttonSound;
    public AudioClip jobCompleted;
    public AudioClip singleDig;
    public AudioClip excavate;
    public AudioClip lose;
    public AudioClip swipeSound;
    public AudioClip upgradeSound;
    public AudioClip moleSound;
    public AudioClip hireSound;

    public AudioSource audioSrc;
    public AudioSource bgmAudioSrc;
    public GameController gameController;

    public bool bgmOn     { get; set; }
    public bool sfxOn     { get; set; }



// Start is called before the first frame update
void Start()
    {
        bgmOn = true;
        sfxOn = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void PlaySound(string clip)
    {


        if (sfxOn)
        {
            //Debug.Log(clip);
            switch (clip)
            {
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
                case "swipe":
                    audioSrc.PlayOneShot(swipeSound);
                    break;
                case "upgrade":
                    audioSrc.PlayOneShot(upgradeSound);
                    break;
                case "mole":
                    audioSrc.PlayOneShot(moleSound);
                    break;
                case "lose":
                    audioSrc.PlayOneShot(lose);
                    break;
                case "dig":
                    audioSrc.PlayOneShot(singleDig);
                    break;
                case "excavate":
                    audioSrc.PlayOneShot(excavate);
                    break;
                case "star":
                    audioSrc.PlayOneShot(star);
                    break;
                case "hire":
                    audioSrc.PlayOneShot(hireSound);
                    break;
            }
        }
    }

    public void ToogleBGM()
    {
        bgmAudioSrc.mute = !bgmAudioSrc.mute;
    }

    public void ButtonSound() {
        if (sfxOn)
        PlaySound("button");
    }
}
