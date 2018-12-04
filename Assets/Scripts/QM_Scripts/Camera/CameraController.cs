using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class CameraController : MonoBehaviour {
    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin camNoise;
    public Camera camNonVirtual;
    
    private float horizontal2;
    private float vertical2;
    private float rotation;
    private float currentRotation;
    private float smoothRotationPositif;
    private float smoothRotationNegatif;
    public float smoothRotationSpeed;
    public float rotationSpeed;

    public GameObject targetRotation;

    private float rotationX;

    private float smoothRotationCam;

    public GameObject returnToRotationCam;
    private float smoothReturn;
    public float speedReturn;

    

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
    }

    private void Update()
    {
        horizontal2 = Input.GetAxis("Horizontal2");
        vertical2 = Input.GetAxis("Vertical2");
        rotationX = transform.rotation.x;
        //RotationCam2();
        RotationCam();
        StartCoroutine( ReturnBehindPlayer());


    }

    public IEnumerator CameraShakeMindManipulation(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1 ,1) * magnitude;
            float z = Random.Range(-1,1) * magnitude;

            

            transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

            elapsed += Time.deltaTime;

            yield return null;

        }

        transform.position = originalPos;
    }

   void RotationCam2()
    {
        if (horizontal2 != 0 || vertical2 != 0)
        {
            transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(45,
                                                                                            targetRotation.transform.rotation.eulerAngles.y + 90,
                                                                                            targetRotation.transform.rotation.eulerAngles.z)
                                                                                            , smoothRotationCam);
            smoothRotationCam += .1f * Time.deltaTime;
        }

        else if ( horizontal2 == 0 && vertical2 == 0)
        {
            smoothRotationCam = 0;
        }
    }

    void RotationCam()
    {



        if (horizontal2 > 0)
        {
            smoothRotationNegatif = 0;
            transform.Rotate(Vector3.up * Mathf.Lerp(0, rotationSpeed, smoothRotationPositif), Space.World);
            smoothRotationPositif += smoothRotationSpeed * Time.deltaTime;

        }


        else if (horizontal2 < 0)
        {
            smoothRotationPositif = 0;
            transform.Rotate(-Vector3.up * Mathf.Lerp(0, rotationSpeed, smoothRotationNegatif), Space.World);
            smoothRotationNegatif += smoothRotationSpeed * Time.deltaTime;
        }

        else if (horizontal2 == 0)
        {
            smoothRotationNegatif = 0;
            smoothRotationPositif = 0;
        }
    
        //if (vertical2 > 0)
        //{
        //    if (transform.eulerAngles.x < 45)
        //    {
        //        transform.Rotate(new Vector3(.4f, 0, 0));
        //    }
        //}

        //else if (vertical2 < 0)
        //{
        //    if (transform.eulerAngles.x > 15)
        //    {
        //        transform.Rotate(new Vector3(-.4f, 0, 0));
        //    }
           
        //}


    }

    private IEnumerator ReturnBehindPlayer()
    {

        if (vertical2 == 0 && horizontal2 == 0 && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            yield return new WaitForEndOfFrame();
            transform.rotation = Quaternion.Slerp(transform.rotation, returnToRotationCam.transform.rotation, smoothReturn);
            smoothReturn += speedReturn * Time.deltaTime;
        }

        else
        {
            smoothReturn = 0;
        }
    }


   public void CameraZoomFocus()
    {
        
            cam.enabled = false;
            
       
        
    }

    public void CameraDeZoomFocus()
    {
            cam.enabled = true;
       
    }

    public void Follow(Transform follow)
    {
        cam.m_Follow = follow;
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
