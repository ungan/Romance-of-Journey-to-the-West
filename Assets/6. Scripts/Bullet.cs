using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Type { Swing, Shoot, Magic, Magicline, Trap, Explosive }
    public Type type;
    public int value;
    public int damage;

    public GameObject[] skillObject;

    void Update()
    {
        if (type == Type.Swing)
        {
            Invoke("ActiveFalse", 0.15f);
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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject bullet;

        if (collision.gameObject.tag == "Border" && type != Type.Swing && type != Type.Magicline)
        {
            Gone();
        }
        if (type == Type.Trap && collision.gameObject.tag == "Enemy" && value == 9)
        {
            bullet = Instantiate(skillObject[0], this.transform.position, Quaternion.Euler(0, 0, 0));
            bullet = Instantiate(skillObject[1], this.transform.position, Quaternion.Euler(0, 0, 0));
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
}