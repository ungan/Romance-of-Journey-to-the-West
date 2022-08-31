using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemey_ani : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject attack_mark;      // 공격 이펙트
    public Enemy_ai enemy;          // enemy_ai
    public GameObject attack_range;     // 공격시 공격 범위 on
    public PolygonCollider2D pc;

    float particle_position_right;
    float particle_position_left;
    float maxonDelay;               // 공격 이펙트가 보이는 시간
    float curonDelay;               // 공격 이펙트가 얼마나 보였는지 세어주는 변수
    void Start()
    {
        
        particle_position_right = attack_mark.transform.localPosition.x * -1;
        particle_position_left = attack_mark.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void particle_start()
    {
        if (enemy.sight_right == true)
        {
            
            if(attack_mark.transform.localScale.x<0) attack_mark.transform.localScale = new Vector3(attack_mark.transform.localScale.x*-1, attack_mark.transform.localScale.y, attack_mark.transform.localScale.z);
            if (attack_mark.transform.localPosition.x <  0) attack_mark.transform.localPosition = new Vector3(particle_position_right, attack_mark.transform.localPosition.y, 1f);

            attack_mark.SetActive(true);
        }
        else if (enemy.sight_right == false)
        {
            if (attack_mark.transform.localScale.x > 0) attack_mark.transform.localScale = new Vector3(attack_mark.transform.localScale.x * -1, attack_mark.transform.localScale.y, attack_mark.transform.localScale.z);
            if (attack_mark.transform.localPosition.x > 0) attack_mark.transform.localPosition = new Vector3(particle_position_left, attack_mark.transform.localPosition.y, 1f);
            attack_mark.SetActive(true);
        }
    }

    void atk_end()
    {
        
        attack_range.SetActive(false);
        attack_mark.SetActive(false);
        if (enemy.issight_range == true && enemy.inrange_dash == true)
        {
            enemy.enemy_state = e_state.attack_ready;
        }
        else if (enemy.issight_range == false)
        {
            enemy.enemy_state = e_state.Follow;
        }

       
    }
    void atk_start()
    {
        attack_range.SetActive(true);
        
    }

    void atkready_end()
    {
        enemy.enemy_state = e_state.attack;     // atk ready 가 끝났으므로 attack 상태로 넘어가줌
                                                // 
    }

}
