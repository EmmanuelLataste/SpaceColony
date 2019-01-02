using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class CameraController : MonoBehaviour
{
    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }
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

    public GameObject player;
    private RaycastHit hit;
    public float rayLength;


    public List<Collider> collidList;
    public RaycastHit[] hits;
    public bool transparencyActivated = false;
    RaycastHit[] transparencyCollidersSaved;

    public float speedMouseX;
    public float speedMouseY;

    private float initialMouseX = 0;
    private float initialMouseY = 0;

    public LayerMask layerMask;
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
        //StartCoroutine(ReturnBehindPlayer());
        CameraRay();
        CameraMouse();
       

    }

    void CameraMouse()
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

    public IEnumerator CameraShakeMindManipulation(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float z = Random.Range(-1, 1) * magnitude;



            transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

            elapsed += Time.deltaTime;

            yield return null;

        }

        transform.position = originalPos;
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

        if (vertical2 > 0)
        {

            if (transform.eulerAngles.x < 20 && transform.eulerAngles.x > 0)
            {
                transform.Rotate(new Vector3(.2f, 0, 0));
            }
        }

        else if (vertical2 < 0)
        {
            if (transform.eulerAngles.x < 21 && transform.eulerAngles.x > 1)
            {
                transform.Rotate(new Vector3(-.2f, 0, 0));
            }

        }


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
    Collider currentHit;



    private void CameraRay()
    {
        
        hits = Physics.RaycastAll(player.transform.position, transform.position - player.transform.position, rayLength, layerMask);


        if (hits.Length > 0)
        {
            transparencyActivated = true;
            
        }
        else
        {
            transparencyActivated = false;
          
            
               
                foreach (Collider collider in collidList)
                {
                    Color32 col = collider.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color");
                    col.a = 255;
                    collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", col);
                    ChangeRenderMode(collider.gameObject.GetComponent<MeshRenderer>().material, BlendMode.Opaque);

                }
        }

        if (transparencyActivated)
        {
            foreach (RaycastHit rayCast in hits)
            {
                if (!collidList.Contains(rayCast.collider))
                {
                    collidList.Add(rayCast.collider);
                }

                Color32 col = rayCast.collider.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color");
                col.a = 70;
                rayCast.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", col);
                ChangeRenderMode(rayCast.collider.gameObject.GetComponent<MeshRenderer>().material, BlendMode.Transparent);
                

            }
        }
        else
        {
            if (transparencyCollidersSaved != null && transparencyCollidersSaved.Length > 0)
            {
                foreach (RaycastHit rayCast in transparencyCollidersSaved)
                {
                    if (collidList.Contains(rayCast.collider))
                    {
                        collidList.Remove(rayCast.collider);
                    }
                    Color32 col = rayCast.collider.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color");
                    col.a = 255;
                    rayCast.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", col);
                    ChangeRenderMode(rayCast.collider.gameObject.GetComponent<MeshRenderer>().material, BlendMode.Opaque);

                }
            }
        }

       
        transparencyCollidersSaved = hits;

        Debug.DrawRay(player.transform.position, transform.position - player.transform.position, Color.red); // Permet de voir le ray dans la scène lorsque c'est lancé.


    }


    public void ChangeRenderMode( Material standardShaderMaterial, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
               
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite",0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
        }

    }

  
}
