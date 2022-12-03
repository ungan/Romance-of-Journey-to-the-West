using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    ObjectManager objectManager;

    public enum Type { Character, Item, AutoTakeItem };
    public Type type;
    public int value;


    public bool isFollowing; //이끌려감
    public float speed; //속도

    //경험치양
    public int expAmount = 0;

    Vector3 followPos;
    public GameObject AreaPoint;
    Rigidbody2D rigid;

    private void Awake()
    {
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

        if (type == Type.AutoTakeItem)
        {
            switch (value)
            {
                case 0: //Soul
                    expAmount = 1;
                    break;
                case 1: //BigSoul
                    expAmount = 5;
                    break;

            }
        }
    }

    void OnEnable()
    {
        if (type == Type.AutoTakeItem)
        {
            switch (value)
            {
                case 0: //Soul
                    isFollowing = false;
                    AreaPoint = null;
                    break;
                case 1: //Big Soul
                    isFollowing = false;
                    AreaPoint = null;
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        if (isFollowing) //이동
        {
            followPos = AreaPoint.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, followPos, (speed * Time.fixedDeltaTime));
        }
    }

    void ActiveFalse()
    {
        this.gameObject.SetActive(false);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    void Dequeue() //이거 사용 권장!
    {
        if (!this.gameObject.activeSelf) return;

        StartCoroutine(objectManager.ObjReturn(this.gameObject));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (type)
        {
            case Type.AutoTakeItem:
                switch (value)
                {
                    case 0: //Soul
                    case 1: //Big Soul
                        if (collision.gameObject.name == "SoulHarvestArea" || collision.gameObject.name == "MagicLine")
                        {
                            isFollowing = true;
                            //CancelInvoke("Dequeue");
                            speed = 13f;
                        }
                        if(collision.gameObject.name == "SoulHarvestPoint")
                        {
                            PartyManager partyManager = GameObject.Find("Party").GetComponent<PartyManager>();
                            AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                            audioManager.PlayBgm("EXP");
                            partyManager.curEXP += expAmount;
                            Dequeue();
                        }
                        AreaPoint = GameObject.Find("SoulHarvestPoint");
                        break;
                }
                break;
            case Type.Character:
                break;
        }
    }
}
