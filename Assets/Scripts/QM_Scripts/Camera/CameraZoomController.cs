using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraZoomController : MonoBehaviour {

    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin camNoise;
    private Vector3 offsetCamPlayer;
    private float timer;
    public GameObject player;



    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        CameraRay();
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

    private void CameraRay()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, player.transform.position * 100, Color.red); // Permet de voir le ray dans la scène lorsque c'est lancé.
        if (Physics.Raycast(transform.position, player.transform.position, out hit))
        {
         
        }
    }

    private void ObjectBetweenPlayerAndCam()
    {

        offsetCamPlayer = player.transform.position - transform.position;
    }

}
