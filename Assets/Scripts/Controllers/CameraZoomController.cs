using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraZoomController : MonoBehaviour {

    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin camNoise;
    private float timer;


    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void CameraShake(float amplitude, float frequency)

    {
        camNoise.m_AmplitudeGain = amplitude;
        camNoise.m_FrequencyGain = frequency;
    }

    public IEnumerator CameraShakeTiming(float amplitude, float frequency, float duration)
    {
        camNoise.m_AmplitudeGain = amplitude;
        camNoise.m_FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        camNoise.m_AmplitudeGain = 0;
        camNoise.m_FrequencyGain = 0;
    }


}
