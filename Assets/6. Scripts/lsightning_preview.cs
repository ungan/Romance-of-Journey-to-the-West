using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lsightning_preview : MonoBehaviour
{
    SpriteRenderer sprite;
    float max_lifetime= 1.0f;
    float cur_lifetime;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    private void FixedUpdate()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g,sprite.color.b, sprite.color.a + 0.01f);
        life_time();
    }
    void life_time()
    {
        if (max_lifetime <= cur_lifetime)
        {
            cur_lifetime = 0f;
            Destroy(gameObject);
        }
        else
        {
            cur_lifetime += Time.deltaTime;
        }
    }
}
