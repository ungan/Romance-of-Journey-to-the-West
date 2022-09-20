using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_zone : MonoBehaviour
{
    public float damage = 20f;

    public PartyManager partymanager;
    public GameObject zone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Party")
        {
            if(partymanager == null) partymanager = GameObject.Find("Party").GetComponent<PartyManager>();
            partymanager.onDamabe_bullet_party(damage);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        partymanager = GameObject.Find("Party").GetComponent<PartyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("lifetime");
    }

    IEnumerator lifetime()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(zone);
    }
}
