using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    AudioManager audioManager;

    public enum Type { Swing, Shoot, Magic, Magicline, Trap, Explosive, Effect }
    public Type type;
    public int value;
    public int damage;

    public GameObject[] skillObject;


    bool trapOn = false; //트랩 활성화 bool

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        switch (value)
        {
            case 9:
                Invoke("TrapOn", 1f);
                break;
            case 11:
                audioManager.PlayBgm("Sa Member 2");
                break;
            case 99999:
                audioManager.PlayBgm("Sa Member 3");
                break;

        }
    }
    void Update()
    {
        if (type == Type.Swing)
        {
            if(value == 2)
              Invoke("Gone", 0.6f);
        }
        else if (type == Type.Shoot)
        {
            Invoke("Gone", 3f);
        }
        else if (type == Type.Trap)
        {
            if (value == 9)
                Invoke("Gone", 10f);
            else
                Invoke("Gone", 5f);
        }
        else if (type == Type.Explosive)
        {
            Invoke("Gone", 0.1f);
        }
        else if(type == Type.Effect)
        {
            Invoke("Gone", 1.1f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject bullet;

        if (collision.gameObject.tag == "Border" && type != Type.Swing && type != Type.Magicline)
        {
            Gone();
        }
        if (type == Type.Trap && collision.gameObject.tag == "Enemy" && value == 9 && trapOn)
        {
            bullet = Instantiate(skillObject[0], this.transform.position, Quaternion.Euler(0, 0, 0));
            bullet = Instantiate(skillObject[1], this.transform.position, Quaternion.Euler(0, 0, 0));
            Gone();
        }
        if(type == Type.Trap && value == 11 && collision.gameObject.tag == "Magicline" && collision.gameObject.name == "DodgePushZone")
        {
            bullet = Instantiate(skillObject[0], this.transform.position, Quaternion.Euler(0, 0, 0));
            Instantiate(skillObject[1], this.transform.position, Quaternion.Euler(0, 0, 0));
            Gone();
        }
    }

    void Gone()
    {
        Destroy(gameObject);
    }

    void ActiveFalse()
    {
        this.gameObject.SetActive(false);
    }

    void TrapOn() //트랩 활성화
    {
        trapOn = true;
    }
}