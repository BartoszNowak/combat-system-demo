using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{
    private const float shakeDuration = 0.2f;
    private const float shakeFrequency = 2f;

    private float shakeAmplitude = 0f;

    private float shakeElapsedTime = 0f;

    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    void Start()
    {
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (VirtualCamera != null && virtualCameraNoise != null)
        {
            if (shakeElapsedTime > 0)
            {
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;

                shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                virtualCameraNoise.m_AmplitudeGain = 0f;
                shakeElapsedTime = 0f;
                shakeAmplitude = 0f;
            }
        }
    }

    public void TriggerShake(float power)
	{
        shakeAmplitude = Mathf.Max(shakeAmplitude, power);
        shakeElapsedTime = shakeDuration;
    }
}