using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraZoomController : MonoBehaviour {
    public GameObject normalCam;
    private CinemachineVirtualCamera cam;
    private CinemachineFramingTransposer camTransposer;
    private CinemachineBasicMultiChannelPerlin camNoise;
    public GameObject player;
    private RaycastHit hit;
    public float rayLength;
    [SerializeField] GameObject targetRot;

    float speedMouseX;
    float speedMouseY;

    private float initialMouseX = 0;
    private float initialMouseY = 0;
    private float initialHorizontal;
    private float initialVertical;
    private bool isAxisF2inUse;
    float timerF2;
    public float timerF2Offset;
    CameraController cameraController;

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        camTransposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cameraController = normalCam.GetComponent<CameraController>();
       

    }

    private void Update()
    {
        speedMouseX = cameraController.speedMouseX;
        speedMouseY = cameraController.speedMouseY;
        if (Input.GetAxis("Fire2") == 1 || Input.GetButton("Fire2") == true)
        {
            
            CameraMouse();

        }

        else
        {
            transform.rotation = targetRot.transform.rotation;
            initialMouseY = transform.rotation.eulerAngles.x;
            initialMouseX = transform.rotation.eulerAngles.y;
        }



        CameraRay();
        //Rotation();

        
    }
    void CameraMouse()
    {
        if (CameraController.isControllerConnected == false)
        {
            if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
            {
                if (transform.rotation.eulerAngles.x <= 40 && transform.rotation.eulerAngles.x >= 0)
                {

                    initialMouseY -= speedMouseY * Input.GetAxis("Mouse Y");
                }


                else if (transform.rotation.eulerAngles.x >= 359 && transform.rotation.eulerAngles.x < 360 || transform.rotation.eulerAngles.x < 0)
                {
                    initialMouseY -= speedMouseY * Input.GetAxis("Mouse Y");

                }

                else if (transform.rotation.eulerAngles.x > 40 && transform.rotation.eulerAngles.x < 200 && Input.GetAxis("Mouse Y") > 0)
                {
                    initialMouseY -= speedMouseY * Input.GetAxis("Mouse Y");
                }


                else if (transform.rotation.eulerAngles.x < 359 && transform.rotation.eulerAngles.x > 250 && Input.GetAxis("Mouse Y") < 0)
                {

                    initialMouseY -= speedMouseY * Input.GetAxis("Mouse Y");

                }

                initialMouseX += speedMouseX * Input.GetAxis("Mouse X");
                transform.eulerAngles = new Vector3(initialMouseY, initialMouseX, 0);

            }
        }
       
         else
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                if (Input.GetAxis("Fire2") == 1 && MindPower.isMindManipulated == false)
                {
                    if (transform.rotation.eulerAngles.x <= 40 && transform.rotation.eulerAngles.x >= 0)
                    {

                        initialVertical -= speedMouseY * Input.GetAxis("Vertical");
                    }


                    else if (transform.rotation.eulerAngles.x >= 359 && transform.rotation.eulerAngles.x < 360 || transform.rotation.eulerAngles.x < 0)
                    {
                        initialVertical -= speedMouseY * Input.GetAxis("Vertical");
                    }

                    else if (transform.rotation.eulerAngles.x > 40 && transform.rotation.eulerAngles.x < 200 && Input.GetAxis("Vertical") > 0)
                    {
                        initialVertical -= speedMouseY * Input.GetAxis("Vertical");
                    }

                    else if (transform.rotation.eulerAngles.x < 359 && transform.rotation.eulerAngles.x > 250 && Input.GetAxis("Vertical") < 0)
                    {

                        initialVertical -= speedMouseY * Input.GetAxis("Vertical");
                    }

                    initialHorizontal += speedMouseX * Input.GetAxis("Horizontal");
                    transform.eulerAngles = new Vector3(initialVertical, initialHorizontal, 0);


                }

            }


        }

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
