using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GroupPosition : MonoBehaviour
{
    public bool isFollowing;
    public int position;
    public GameObject traceTarget;
    float originRange;
    public float range;
    public float speed;
    Vector3 tracePos;
    public PartyManager partyManager;

    // Start is called before the first frame update
    void Start()
    {
        isFollowing = true;
        speed = 5f;
    }

    void Update()
    {
        Check();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 velo = Vector3.zero;

        tracePos = traceTarget.transform.position;
        originRange = Vector3.Distance(tracePos, transform.position);
        range = (float)Math.Truncate(originRange * 10) / 10;

        if(position == 1)
            transform.position = tracePos;

        if (isFollowing)
        {
            transform.position = Vector3.MoveTowards(transform.position, tracePos, (partyManager.speed * Time.fixedDeltaTime));
        }
    }

    void Check()
    {
        //포지션 체크
        //tracePos = traceTarget.transform.position;
        //range = Vector3.Distance(tracePos, transform.position);

        //팔로잉 여부 체크
        if(position == 1)
        {
            isFollowing = false;
        }
        if (position != 1) {
            if (range >= 1.2f)
            {
                isFollowing = true;
            }
            if (range < 1.2f)
            {
                isFollowing = false;
            }
        }
    }
}
