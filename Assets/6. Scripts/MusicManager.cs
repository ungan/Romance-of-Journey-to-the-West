using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public EventManager eventManager; //이벤트매니저
    public GameManager gameManager; //게임매니저
    AudioSource audioSource;

    //OST
    public AudioClip hordeTheme; //호드테마

    //play
    public bool playMusic;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (eventManager.hordeEvent)
        {
            audioSource.clip = hordeTheme;
        }

        if(playMusic == true)
        {
            audioSource.Play();
            playMusic = false;
        }
    }
}
