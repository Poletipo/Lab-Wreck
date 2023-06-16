using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcess : MonoBehaviour {


    [SerializeField] Material postProcessMaterial;

    [Range(0, 100)]
    [SerializeField] int _blurSize = 0;
    [Range(1, 100)]
    [SerializeField] int _sampleCount = 1;



    private void Start()
    {
        Camera cam = GetComponent<Camera>();

        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.DepthNormals;
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postProcessMaterial);
    }


    private void Update()
    {
        postProcessMaterial.SetInt("_BlurSize", _blurSize);
        postProcessMaterial.SetInt("_SampleCount", _sampleCount);
    }

}
