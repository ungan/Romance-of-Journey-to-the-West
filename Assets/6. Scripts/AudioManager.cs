using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Bgm
{
    public string name;
    public AudioClip clip;
    private AudioSource source;

    public float Volume;
    public float maxVolume;
    public bool mute;
    public bool loop;


    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.mute = mute;
        source.loop = loop;
        maxVolume = Volume;
    }
    public void SetVolume()
    {
        source.volume = Volume;
    }
    public void Play()
    {
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }
    public void SetMute()
    {
        source.mute = mute;
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager amanager = null;


    public float vol;
    public bool mute;
    public GameObject Bgmscroll;
    public GameObject BgmMute;


    [SerializeField]
    public Bgm[] bgms;


    private void Awake()
    {
        if (amanager == null)
        {
            amanager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayBgm(string _name)
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            if (_name == bgms[i].name)
            {
                bgms[i].Play();
                return;
            }
        }
    }
    public void StopBgm(string _name)
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            if (_name == bgms[i].name)
            {
                bgms[i].Stop();
                return;
            }
        }
    }
    public void StopAllBgm()
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            bgms[i].Stop();
        }
    }


    public void SetVolume()
    {
        float maxv = 0;
        for (int i = 0; i < bgms.Length; i++)
        {
            maxv = bgms[i].maxVolume;
            bgms[i].Volume = vol * maxv;
            bgms[i].SetVolume();
        }
    }
    public void SetMute()
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            bgms[i].mute = mute;
            bgms[i].SetMute();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            GameObject bgmObject = new GameObject("Audio No." + i + " " + bgms[i].name);
            bgms[i].SetSource(bgmObject.AddComponent<AudioSource>());
            bgmObject.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //vol = Bgmscroll.GetComponent<Slider>().value;
        //mute = BgmMute.GetComponent<Toggle>().isOn;
        SetVolume();
        SetMute();
    }
}

