using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class CameraController : MonoBehaviour {
    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin camNoise;
    
    private float horizontal2;
    private float vertical2;
    private float rotation;
    private float currentRotation;
    private float smoothRotationPositif;
    private float smoothRotationNegatif;
    public float smoothRotationSpeed;

    public GameObject targetRotation;

    private float rotationX;

    

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

        RotationCam();


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

   

    void RotationCam()
    {

        //transform.Rotate(new Vector3(vertical2, 0, 0) * 10f, Space.World);
        //transform.Rotate(new Vector3(0,horizontal2,0) * 10f, Space.World);


        if (horizontal2 > 0)
        {
            //transform.Rotate(Vector3.up * 1f, Space.World);
            smoothRotationNegatif = 0;
            transform.Rotate(Vector3.up * Mathf.Lerp(0, 2f, smoothRotationPositif), Space.World);
            smoothRotationPositif += smoothRotationSpeed * Time.deltaTime;

        }


        else if (horizontal2 < 0)
        {

            //transform.Rotate(-Vector3.up * 1f, Space.World);
            smoothRotationPositif = 0;
            transform.Rotate(-Vector3.up * Mathf.Lerp(0, 2f, smoothRotationNegatif), Space.World);
            smoothRotationNegatif += smoothRotationSpeed * Time.deltaTime;
        }

        else if (horizontal2 == 0)
        {
            smoothRotationNegatif = 0;
            smoothRotationPositif = 0;
        }
        //if (horizontal2 != 0 || vertical2 != 0)
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation.transform.rotation, smoothRotation);
        //    smoothRotation += smoothRotationTimer * Time.deltaTime;
        //}

        //else if (horizontal2 == 0 && vertical2 == 0)
        //{
        //    smoothRotation = 0;
        //}


        //transform.Rotate(new Vector3(vertical2, horizontal2, 0) * 40, Space.World);
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
