using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nachal_ani : MonoBehaviour
{
    public boss_nachal nachal;
    public Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ligntning_ani_end()
    {
        nachal.ani_end = true;
        ani.Play("idle");
        nachal.lightning_patton_phase1();
    }
}
