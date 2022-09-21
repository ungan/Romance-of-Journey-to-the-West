using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoStatue : MonoBehaviour
{
    public EventManager eventManager;
    public AudioManager audioManager;

    public GameObject StatueGuard;
    public GameObject MagicLine;

    public List<GameObject> mobs = new List<GameObject>();

    public float health;

    public bool isBreaking = false;
    bool end = false;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        for(int i=0; i<StatueGuard.transform.childCount; i++)
        {
            if(StatueGuard.transform.GetChild(i)!=null)
            {
                mobs.Add(StatueGuard.transform.GetChild(i).gameObject);
            }
        }
    }

    private void Update()
    {
        if(isBreaking && !end)
            CheckMobs();
    }
    void CheckMobs()
    {
        for(int i=0; i<mobs.Count; i++)
        {
            if (mobs[i].activeSelf == false)      // mobs[i].activeSelf == false OR mobs[i]==null
            {
                mobs.Remove(mobs[i]);
            }
            if (mobs.Count == 0)
            {
                eventManager.curEChangeDelay = eventManager.maxEChangeDelay + 1;
                end = true;
            }

        }
        
    }
    void Break()
    {
        //eventManager.curEChangeDelay = eventManager.maxEChangeDelay + 1;
        audioManager.StopBgm("StatueAttack");
        audioManager.PlayBgm("StatueBreak");
        MagicLine.SetActive(true);
        anim.SetBool("isBreak", true);
        Invoke("GuardSpawn", 1.85f);
        Invoke("MagicLineFalse", 2f);
    }

    void MagicLineFalse()
    {
        MagicLine.SetActive(false);
    }

    void GuardSpawn()
    {
        StatueGuard.SetActive(true);
    }

    IEnumerator OnDamage(int damage)
    {
        audioManager.PlayBgm("StatueAttack");
        if (health > 0)
            health -= damage;
        if (health <= 0)
        {
            health = 0;
            isBreaking = true;
            Break();
            GetComponent<Collider2D>().enabled = false;
        }
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "PlayerBullet" || collision.gameObject.tag == "PlayerSwing" || collision.gameObject.tag == "Magicline" || collision.gameObject.tag == "Explosive") && !isBreaking)
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            StartCoroutine(OnDamage(bullet.damage));
        }
    }
}
