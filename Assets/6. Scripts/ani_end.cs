using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ani_end : MonoBehaviour
{

    public GameObject wakeup_obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void wake_up()
    {
        Destroy(gameObject);
        wakeup_obj.SetActive(true);
    }
}
