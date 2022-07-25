using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;
    ScreenShakeController shake;
    public Transform player;

    Vector3 target, mousePos, refvel, shakeOffset;
    float normalCameradist = 2f;
    float farCameradist = 10f;
    float smoothTime = 0.2f, zStart;
    float shakeMag, shakeTimeEnd;
    Vector3 shakeVector;
    bool shaking;
    bool ctrl;
    public bool dying = false;
    public bool t = false;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        shake = GetComponent<ScreenShakeController>();
        target = player.position;
        zStart = transform.position.z;
    }

    void FixedUpdate()
    {
        mousePos = CaptureMousePos();
        shakeOffset = UpdateShake();
        target = UpdateTargetPos();
        UpdateCameraPosition();
        GetInput();
    }
    void GetInput()
    {
        ctrl = Input.GetButton("farCamera");
    }

    Vector3 CaptureMousePos()
    {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        float max = 0.9f;
        if(Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized;
        }
        return ret;
    }
    
    Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset;
        Vector3 ret;

        if (ctrl)
            mouseOffset = mousePos * farCameradist;
        else
            mouseOffset = mousePos * normalCameradist;

        ret = player.position + mouseOffset;
        //ret += shakeOffset;
        ret.z = zStart;
        return ret;
    }

    void UpdateCameraPosition()
    {
        Vector3 tempPos;
        Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);

        if (dying) //사망시 줌인+플레이어로 시선 고정 or 무기 변환중
        {
            tempPos = Vector3.SmoothDamp(transform.position, playerPos, ref refvel, smoothTime);
            if(mainCamera.orthographicSize > 6f)
                mainCamera.orthographicSize -= 0.15f;
        }
        else if (t)
        {
            tempPos = Vector3.SmoothDamp(transform.position, playerPos, ref refvel, smoothTime);
            if (mainCamera.orthographicSize < 10f)
                mainCamera.orthographicSize += 0.15f;
        }
        else //평시
        {
            tempPos = Vector3.SmoothDamp(transform.position, target, ref refvel, smoothTime);
            if (mainCamera.orthographicSize < 10f)
                mainCamera.orthographicSize += 0.15f;
        }
        transform.position = tempPos;
    }

    public void Shake(float length, float power)
    {
        shake.StartShake(length, power);
    }

    Vector3 UpdateShake()
    {
        if (!shaking || Time.time > shakeTimeEnd)
        {
            shaking = false;
            return Vector3.zero;
        }
        Vector3 tempOffset = shakeVector;
        tempOffset *= shakeMag;
        return tempOffset;
    }
}
