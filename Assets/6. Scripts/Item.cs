using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Character, Item, AutoTakeItem };
    public Type type;
    public int value;

    public bool isFollowing; //이끌려감
    public float speed; //속도

    Vector3 followPos;
    public GameObject AreaPoint;
    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        if (type == Type.AutoTakeItem)
        {
            switch (value)
            {
                case 0: //Soul
                    Invoke("Destroy", 5f);
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (type)
        {
            case Type.AutoTakeItem:
                switch (value)
                {
                    case 0: //Soul
                        if (collision.gameObject.name == "SoulHarvestArea" || collision.gameObject.name == "MagicLine")
                        {
                            isFollowing = true;
                            CancelInvoke("Destroy");
                            speed = 13f;
                        }
                        if(collision.gameObject.name == "SoulHarvestPoint")
                        {
                            Character character = GameObject.Find("Jeo-PalGye").GetComponent<Character>();
                            character.curSoul++;
                            Destroy();
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
