﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    AudioManager audioManager;
    ObjectManager objectManager;

    public enum Type { Swing, Shoot, Magic, Magicline, Trap, Explosive, Effect }
    public Type type;
    public int value;
    public int damage;

    public GameObject[] skillObject;

    bool active;
    bool activeCheck = true;

    bool trapOn = false; //트랩 활성화 bool

    void Awake()
    {
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (this.gameObject.activeSelf) active = true;
        else if (!this.gameObject.activeSelf) active = false;

        if(active && activeCheck)
        {
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

            if (type == Type.Swing)
            {
                if (value == 2)
                    Invoke("Dequeue", 0.6f);
            }
            else if (type == Type.Shoot)
            {
                Invoke("Dequeue", 3f);
            }
            else if (type == Type.Trap)
            {
                if (value == 9)
                    Invoke("Dequeue", 10f);
                else
                    Invoke("Dequeue", 5f);
            }
            else if (type == Type.Explosive)
            {
                Invoke("Dequeue", 0.1f);
            }
            else if (type == Type.Effect)
            {
                Invoke("Dequeue", 1.1f);
            }

            activeCheck = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject bullet;

        if ((collision.gameObject.tag == "Border" || collision.gameObject.tag == "Enemy") && type == Type.Shoot)
        {
            Dequeue();
        }
        if (type == Type.Trap && collision.gameObject.tag == "Enemy" && value == 9 && trapOn)
        {
            bullet = objectManager.MakeObj("Boom Plant", this.transform.position, Quaternion.Euler(0, 0, 0));
            bullet = objectManager.MakeObj("Trap Plant", this.transform.position, Quaternion.Euler(0, 0, 0));
            Dequeue();
        }
        if(type == Type.Trap && value == 11 && collision.gameObject.tag == "Magicline" && (collision.gameObject.name == "DodgePushZone" || collision.gameObject.name == "FireRail(Clone)"))
        {
            bullet = objectManager.MakeObj("Fire Boom Plant", this.transform.position, Quaternion.Euler(0, 0, 0));
            objectManager.MakeObj("Fire Boom Plant Effect", this.transform.position, Quaternion.Euler(0, 0, 0));
            Dequeue();
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

    void Dequeue() //이거 사용 권장!
    {
        if (!this.gameObject.activeSelf) return;

        StartCoroutine(objectManager.ObjReturn(this.gameObject));
        ActiveFalse();
        activeCheck = true;
    }

    void TrapOn() //트랩 활성화
    {
        trapOn = true;
    }
}