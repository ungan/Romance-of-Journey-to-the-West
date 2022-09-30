using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_zone : MonoBehaviour
{
    public float damage = 20f;

    public PartyManager partymanager;
    public lightning_manager lightning_manager;
    public GameObject zone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Foot")
        {
            if(partymanager == null) partymanager = GameObject.Find("Party").GetComponent<PartyManager>();

            if (partymanager.isDashing == false && lightning_manager.isdamaging == false)
            {
                partymanager.onDamabe_bullet_party(damage);
                lightning_manager.isdamaging = true;
            }

            Destroy(zone);
        }
    }

    private void OnEnable()
    {
        partymanager = GameObject.Find("Party").GetComponent<PartyManager>();
        lightning_manager = GameObject.Find("lightning_manager").GetComponent<lightning_manager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        partymanager = GameObject.Find("Party").GetComponent<PartyManager>();
        lightning_manager = GameObject.Find("lightning_manager").GetComponent <lightning_manager>();
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
