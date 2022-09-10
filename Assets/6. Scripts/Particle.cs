////////////////////////////////////////////사용방법//////////////////////////////////////////////////////
//파티클 시스템은 트랜스폼 로테이션으로 돌려도 안돌려집니다.. 그래서 아예 스크립트로 돌리게끔 만들었으요//
//로테이션(회전)이 필요한 파티클을 사용할 때 장착하셔서 쓰시면 됩니다                                   //
//앵간해선 터치하지 말 것!                                                                              //
//////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public bool canFlipFlop;
    public bool playAura = true; //파티클 제어 bool
    public ParticleSystem particleObject; //파티클시스템
    public GameObject Mother;
    public float z;

    void Start()
    {
        particleObject = GetComponent<ParticleSystem>();
        playAura = true;
        //particleObject.Play();
    }

    
    void Update()
    {
        var main = particleObject.main;
        z = Mother.transform.eulerAngles.z;
        float ladianZ = -(z * Mathf.Deg2Rad);

        if (canFlipFlop)
        {
            if (z >= 0 && z < 90 || z > 270 && z <= 360) //flip-flop
                particleObject.startRotation3D = new Vector3(0, 0, ladianZ);//StartRotation3D 사용 시
            else
            {
                particleObject.startRotation3D = new Vector3((180 * Mathf.Deg2Rad), 0, -ladianZ);
            }
        }
        else
        {
            particleObject.startRotation3D = new Vector3(0, 0, ladianZ);//StartRotation3D 사용 시
        }

        if (playAura) particleObject.Play();
        else if (!playAura) particleObject.Stop();
    }
}
