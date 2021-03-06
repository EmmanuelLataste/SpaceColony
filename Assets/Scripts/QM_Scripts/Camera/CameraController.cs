﻿using System.Collections;
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
    public static CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin camNoise;
    CinemachineFramingTransposer camTranspo;
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

    private float smoothReturn;

    public GameObject player;
    private RaycastHit hit;
    public float rayLength;


    public List<Collider> collidList;
    public RaycastHit[] hits;
    public bool transparencyActivated = false;
    RaycastHit[] transparencyCollidersSaved;
    int lengthHits;
    RaycastHit[] saveColliderHits;

    public float speedMouseX;
    public float speedMouseY;

    private  float initialMouseX = 0;
    private float initialMouseY = 0;

    private float initialHorizontal;
    private float initialVertical;

    Transform followMM;

    [SerializeField] float smooth;
    Vector3 dollyDir;
    [SerializeField] float maxDist;
    [SerializeField] float minDist;
    [SerializeField] float distance;
    [SerializeField] LayerMask camFrontMask;
    public LayerMask layerMask;

    public static bool isControllerConnected;
    [SerializeField] float startingX;
    [SerializeField] float startingY;
    CharacterController playerCC;

    private void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;


    }

    private void Start()
    {

        playerCC = player.GetComponent<CharacterController>();
        saveColliderHits = hits;
        cam = GetComponent<CinemachineVirtualCamera>();
        camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camTranspo = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        transform.eulerAngles = new Vector3(initialVertical, initialHorizontal, 0);
        followMM = player.transform;
        initialMouseY = startingX;
        initialMouseX = startingY;
        transform.eulerAngles = new Vector3(initialMouseY, initialMouseX, 0);

    }

    private void Update()
    {
        horizontal2 = Input.GetAxis("Horizontal2");
        vertical2 = Input.GetAxis("Vertical2");
        rotationX = transform.rotation.x;
        //RotationCam2();

        RotationCam();
        //StartCoroutine(ReturnBehindPlayer());
        //CameraRay2();
        //CameraFrontObjects();
        CameraMouse();


    }

    void CameraMouse()
    {
       
        if (isControllerConnected == false)
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
            if (Input.GetAxis("Vertical2") != 0 || Input.GetAxis("Horizontal2") != 0)
            {
                if (Input.GetAxis("Fire2") == 0 || MindPower.isMindManipulated == true)
                {

                    if (transform.rotation.eulerAngles.x <= 40 && transform.rotation.eulerAngles.x >= 0)
                    {

                        initialVertical -= speedMouseY * -Input.GetAxis("Vertical2");
                    }


                    else if (transform.rotation.eulerAngles.x >= 359 && transform.rotation.eulerAngles.x < 360 || transform.rotation.eulerAngles.x < 0)
                    {
                        initialVertical -= speedMouseY * -Input.GetAxis("Vertical2");
                    }

                    else if (transform.rotation.eulerAngles.x > 40 && transform.rotation.eulerAngles.x < 200 && Input.GetAxis("Vertical2") < 0)
                    {
                        initialVertical -= speedMouseY * -Input.GetAxis("Vertical2");
                    }

                    else if (transform.rotation.eulerAngles.x < 359 && transform.rotation.eulerAngles.x > 250 && Input.GetAxis("Vertical2") > 0)
                    {

                        initialVertical -= speedMouseY * -Input.GetAxis("Vertical2");
                    }

                    initialHorizontal += speedMouseX * Input.GetAxis("Horizontal2");
                    transform.eulerAngles = new Vector3(initialVertical, initialHorizontal, 0);


                }
            }



            else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
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

    //private IEnumerator ReturnBehindPlayer()
    //{

    //    if (vertical2 == 0 && horizontal2 == 0 && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
    //    {
    //        yield return new WaitForEndOfFrame();
    //        transform.rotation = Quaternion.Slerp(transform.rotation, returnToRotationCam.transform.rotation, smoothReturn);
    //        smoothReturn += speedReturn * Time.deltaTime;
    //    }

    //    else
    //    {
    //        smoothReturn = 0;
    //    }
    //}


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
        followMM = follow;
    }

    public void CameraShake(float amplitude, float frequency )
    {
        camNoise.m_AmplitudeGain = amplitude;
        camNoise.m_FrequencyGain = frequency;
    }

    public IEnumerator CameraShakeTiming(float amplitude, float frequency, float duration)
    {
        float timer = 0;

        camNoise.m_AmplitudeGain = amplitude;
        camNoise.m_FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        timer += Time.time;
        camNoise.m_AmplitudeGain = 0;
        camNoise.m_FrequencyGain = 0;
        yield return null;
    }
    [SerializeField] CinemachineVirtualCamera secondCamNormal;
    [SerializeField] bool isActivatedCam;
    bool isClampCam;
    Vector3 posCamBeforeClamp;
    [SerializeField] Collider[] frontCollider;
    void CameraFrontObjects()
    {


        if (isActivatedCam == true)
        {
            //secondCamNormal.transform.position = transform.position;
            secondCamNormal.m_Follow = cam.m_Follow;
            Debug.DrawLine(transform.position, cam.m_Follow.transform.position);
            Vector3 desiredPos = transform.TransformPoint(dollyDir * maxDist);

            if (Physics.Linecast(secondCamNormal.transform.position, cam.m_Follow.transform.position, out hit, camFrontMask))
            {
                if (isClampCam == false)
                {
                    posCamBeforeClamp = transform.position;
                    isClampCam = true;

                }
                Debug.Log(hit.collider.name);
                distance = Mathf.Clamp(hit.distance, minDist, maxDist);

            }

            else
            {
                distance = maxDist;
            }
            //else if (Physics.Linecast(transform.position, posCamBeforeClamp, out hit, camFrontMask) && !Physics.Linecast(transform.position, cam.m_Follow.transform.position, out hit, camFrontMask))
            //{
            //    distance = maxDist;
            //}
            camTranspo.m_CameraDistance = Mathf.Lerp(camTranspo.m_CameraDistance, distance, Time.deltaTime * smooth);
        }
        
    }


    //void CameraFrontObjects()
    //{
    //    Debug.DrawLine(transform.position, player.transform.position);
    //    Vector3 desiredPos = transform.TransformPoint(dollyDir * maxDist);

    //    if(Physics.Linecast(transform.position, cam.m_Follow.transform.position, out hit, camFrontMask ))
    //    {
    //        Debug.Log(hit.collider.name);
    //        distance = Mathf.Clamp(hit.distance, minDist, maxDist);

    //    }

    //    else
    //    {
    //        distance = Mathf.Lerp(camTranspo.m_CameraDistance, maxDist, Time.deltaTime * smooth) ;
    //    }
    //    camTranspo.m_CameraDistance = Mathf.Lerp(camTranspo.m_CameraDistance, distance, Time.deltaTime * smooth);
    //}

    Collider currentHit;
    bool isSaveColliderHits;
   
    private void CameraRay2()
    {
       
        hits = Physics.RaycastAll(followMM.transform.position, transform.position - followMM.transform.position, rayLength, layerMask);
        if (isSaveColliderHits == false)
        {
            saveColliderHits = hits;
            isSaveColliderHits = true;
        } 

        if (lengthHits != hits.Length)
        {
            
             lengthHits = hits.Length;
            
                foreach (RaycastHit rh in saveColliderHits)
            {
                //rh.collider.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                Color32 col = rh.collider.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color");
                col.a = 255;
                rh.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", col);
                ChangeRenderMode(rh.collider.gameObject.GetComponent<MeshRenderer>().material, BlendMode.Opaque);

            }
            
          

            foreach (RaycastHit rh in hits)
            {
                
                //rh.collider.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                Color32 col = rh.collider.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color");
                col.a = 70;
                rh.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", col);
                ChangeRenderMode(rh.collider.gameObject.GetComponent<MeshRenderer>().material, BlendMode.Transparent);


            }


            saveColliderHits = hits;

        }

    }

    private void CameraRay()
    {
        
        hits = Physics.RaycastAll(followMM.transform.position, transform.position - followMM.transform.position, rayLength, layerMask);


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
