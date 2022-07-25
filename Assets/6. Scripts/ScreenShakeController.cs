using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    float shakeTimeRemaining, shakePower, shakeFadeTime;
    public float a, b;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartShake(a, b);
        }
    }

    void LateUpdate()
    {
        if(shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-shakeTimeRemaining, shakeTimeRemaining) * shakePower;
            float yAmount = Random.Range(-shakeTimeRemaining, shakeTimeRemaining) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        }
    }

    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;
        shakeFadeTime = power / length;
    }
}
