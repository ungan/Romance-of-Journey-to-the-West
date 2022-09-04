using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_nachal : MonoBehaviour
{
    float radiius = 2.0f;
    float maxlightning = 0.25f;
    float curlightning = 0;

    int numchild = 12;
    int lightning_count=0;

    bool can_lightning = true;
    bool can_lightning_read = false;
    public GameObject lightning;
    public GameObject lightning_preview;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(lightning_count <=5)
        {
            Light();
        }
    }

    public void Light()     // 전기 파지직
    {

        if (can_lightning == true)
        {
            can_lightning = false;
            numchild = (int)(radiius * 3.14 * 2);

            for (int i = 0; i < numchild; i++)
            {
                float angle = i * (Mathf.PI * 2.0f) / numchild;
                if(can_lightning_read == true)
                {
                    Instantiate(lightning, transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0)) * radiius, Quaternion.identity);
                }
                else if(can_lightning_read == false)
                {
                    Instantiate(lightning_preview, transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0)) * radiius, Quaternion.identity);
                }
            }
            radiius += 2.0f;
            lightning_count++;
        }
        else if(can_lightning == false)
        {
            cooltime_to_lightning();
        }

        if(can_lightning_read == false && lightning_count == 6)
        {
            can_lightning_read = true;
            lightning_count = 0;
            radiius = 2.0f;
        }
    }
    void cooltime_to_lightning()
    {
        if (maxlightning <= curlightning)
        {
            curlightning = 0f;
            can_lightning = true;
        }
        else
        {
            curlightning += Time.deltaTime;
        }
    }



}
