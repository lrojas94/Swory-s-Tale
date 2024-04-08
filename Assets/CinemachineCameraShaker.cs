using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraShaker : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    public static CinemachineCameraShaker Instance { get; private set; }

    [SerializeField]
    private float timer = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

    }

    public void SetCamera(CinemachineVirtualCamera camera)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = camera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        timer = 0;

        this.virtualCamera = camera;
    }

    public void Shake(float intensity, float time)
    {
        Debug.Log(virtualCamera.gameObject.name);
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        Debug.Log(cinemachineBasicMultiChannelPerlin);
        if (cinemachineBasicMultiChannelPerlin != null)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            timer = time; 
        }
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }
}
