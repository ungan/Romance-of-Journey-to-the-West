using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    AudioManager audioManager;
    ObjectManager objectManager;
    Character curChar;

    public enum Type { Swing, Shoot, Magic, Magicline, Trap, Explosive, Effect }
    public enum Owner { Son, Jeo, Sa, Enemy}
    public Type type;
    public Owner owner;
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

    private void Start()
    {
        if (owner == Owner.Son) curChar = GameObject.Find("Son-Wokong").GetComponent<Character>();
        else if (owner == Owner.Jeo) curChar = GameObject.Find("Jeo-PalGye").GetComponent<Character>();
        else if (owner == Owner.Sa) curChar = GameObject.Find("Sa-OJeong").GetComponent<Character>();
    }

    void Update()
    {
        Upgrade();

        if (this.gameObject.activeSelf) active = true;
        else if (!this.gameObject.activeSelf) active = false;

        if (active && activeCheck)
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
                switch (value)
                {
                    case 2:
                        Invoke("Dequeue", 0.6f);
                        break;
                }
            }
            else if (type == Type.Shoot)
            {
                Invoke("Dequeue", 3f);
            }
            else if (type == Type.Trap)
            {
                if (value == 9)
                    Invoke("Dequeue", 10f);
                else if (value == 11)
                {
                    if (curChar.curUpgradeLV < 4)
                        Invoke("Dequeue", 5f);
                    else if (curChar.curUpgradeLV >= 4)
                        Invoke("Dequeue", 8f);
                }
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
    
    void Upgrade()
    {
        switch (type)
        {
            case Type.Swing:
                switch (value)
                {
                    case 0: //손오공 일반공격
                        if (curChar.curUpgradeLV >= 2)
                            damage = 50;
                        break;
                    case 40: //손오공 차지공격
                        if (curChar.curUpgradeLV >= 5)
                            damage = 90;
                        break;
                    case 50: //저팔계 차지공격
                        if (curChar.curUpgradeLV >= 5)
                            damage = 120;
                        break;
                }
                break;
            case Type.Shoot:
                switch (value)
                {
                    case 2: //저팔계 일반공격
                        if (curChar.curUpgradeLV >= 2)
                            damage = 15;
                        break;
                }
                break;
            case Type.Magicline:
                switch (value)
                {
                    case 1: //사오정 일반공격
                        if (curChar.curUpgradeLV == 1)
                            damage = 25;
                        else if (curChar.curUpgradeLV >= 2)
                            damage = 40;
                        break;
                    case 2: //사오정 PushZoneRight
                        if (curChar.curUpgradeLV >= 5)
                            damage = 25;
                        else if (curChar.curUpgradeLV < 5)
                            damage = 0;
                        break;
                    case 3: //사오정 PushZoneLeft
                        if (curChar.curUpgradeLV >= 5)
                            damage = 25;
                        else if (curChar.curUpgradeLV < 5)
                            damage = 0;
                        break;
                    case 4: //사오정 Damage Zone
                        if (curChar.curUpgradeLV >= 5)
                            damage = 250;
                        else if (curChar.curUpgradeLV < 5)
                            damage = 200;
                        break;
                }
                break;
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