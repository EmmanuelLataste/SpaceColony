using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraZoomController : MonoBehaviour {

    private CinemachineVirtualCamera cam;
    private CinemachineFramingTransposer camTransposer;
    private CinemachineBasicMultiChannelPerlin camNoise;
    public GameObject player;
    private RaycastHit hit;
    public float rayLength;



    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        camTransposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        CameraRay();
        //Rotation();
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
        Debug.DrawRay(player.transform.position, transform.position - player.transform.position, Color.red); // Permet de voir le ray dans la scène lorsque c'est lancé.

        if (Physics.Raycast(player.transform.position, /*transform.forward * 5.2f - transform.up * 1.4f*/ transform.position - player.transform.position, out hit, rayLength ))
        {

            if (hit.collider != null && camTransposer.m_CameraDistance > 1.2f && Input.GetAxis("Fire2") > 0)
            {
                camTransposer.m_CameraDistance -= .1f;
               
            }

           
            //hit.transform.GetComponent<MeshRenderer>().enabled = false;
            //Instantiate(random, hit.point, Quaternion.identity);

        }

        if (hit.collider == null && camTransposer.m_CameraDistance < 0 )
        {
            camTransposer.m_CameraDistance += .1f;

        }


    }


    private void Rotation()
    {
        if (Input.GetAxis("Fire2") > 0)
        {
          
                if (Input.GetAxis("Vertical") > 0)
                {

                    if (transform.localRotation.eulerAngles.x >= 345 || transform.localRotation.eulerAngles.x <= 36)
                    {
                        transform.Rotate(new Vector3(-.6f, 0, 0));
                    }

                }

                else if (Input.GetAxis("Vertical") < 0)
                {

                    if (transform.localRotation.eulerAngles.x >= 344 || transform.localRotation.eulerAngles.x < 35)
                    {
                        transform.Rotate(new Vector3(.6f, 0, 0));
                    }

                }

            if (Input.GetAxis("Fire2") == 0)
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(5, 0, 0));
                }
            
           
        }

   
      
    }

}
