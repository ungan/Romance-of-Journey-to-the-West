using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    LineRenderer lr; //라인 렌더러
    public GameObject obj1, obj2; //연결할 오브젝트 1, 2
    Vector3 obj1Pos, obj2Pos; //연결할 오브젝트 포지션 1, 2

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = .05f;
        lr.endWidth = .05f;
    }

    void Update()
    {
        obj1Pos = obj1.gameObject.transform.position;
        obj2Pos = obj2.gameObject.transform.position;

        lr.SetPosition(0, obj1Pos);
        lr.SetPosition(1, obj2Pos);
    }
}
