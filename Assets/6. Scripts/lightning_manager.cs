using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning_manager : MonoBehaviour
{

    public bool isdamage = false;
    public bool isdamaging = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isdamage == true) StartCoroutine("damaging");   
    }

    IEnumerator damaging()
    {
        if(isdamage == true) yield return 0;
        isdamage = true;

        yield return new WaitForSeconds(0.1f);
        isdamage = false;
    }
}
