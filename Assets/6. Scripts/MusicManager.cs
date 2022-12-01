using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public EventManager eventManager; //이벤트매니저
    public GameManager gameManager; //게임매니저
    AudioSource audioSource;

    //OST
    public AudioClip[] Theme; //호드테마

    //play
    public bool playMusic;
    public bool pauseMusic;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //loop 관리
        if (eventManager.hordeEvent == true) audioSource.loop = true; 
        else audioSource.loop = false;

        if (eventManager.normalEvent)
        {
            audioSource.Stop();
            playMusic = false;
        }
        if (eventManager.hordeEventIntro)
        {
            audioSource.clip = Theme[0];
        }
        if (eventManager.hordeEvent)
        {
            audioSource.clip = Theme[1];
        }
        if (eventManager.bossEvent)
        {
            audioSource.clip = Theme[2];
        }

        if (playMusic == true)
        {
            audioSource.Play();
            playMusic = false;
        }

        if(pauseMusic == true)
        {
            audioSource.Pause();
            pauseMusic = false;
        }
    }
}
