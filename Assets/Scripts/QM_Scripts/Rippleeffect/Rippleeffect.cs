using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rippleeffect : MonoBehaviour
{
    public Material RippleMaterial;

    [Range(0, 1)]
    public float Friction = .9f;

    private float Amount = 0f;

    void Update()
    {
        //    if (Input.GetMouseButton(0))
        //    {
        //        RippleEff();
        //    }

        this.RippleMaterial.SetFloat("_Amount", this.Amount);
        this.Amount *= this.Friction;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, this.RippleMaterial);
    }

    public void RippleEff(Transform transformPo, float maxAmount, float friction)
    {
            this.Friction = friction;
            this.Amount = maxAmount;
            Vector3 pos = transformPo.position;
            this.RippleMaterial.SetFloat("_CenterX", pos.x);
            this.RippleMaterial.SetFloat("_CenterY", pos.y);
        
    }


    
}
