using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineFeature : ScriptableRendererFeature {

    OutlinePass _outlinePass;

    [System.Serializable]
    public class OutlineFeatureSettings {
        public bool IsEnabled = true;
        public RenderPassEvent WhenToInsert = RenderPassEvent.AfterRendering;
        public Material MaterialToBlit;
        public LayerMask LayerMask;
    }

    public OutlineFeatureSettings settings = new OutlineFeatureSettings();

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

        if (!settings.IsEnabled) {
            return;
        }

        RenderTargetIdentifier cameraColorTargetIdent = renderer.cameraColorTargetHandle;
        _outlinePass.Setup(cameraColorTargetIdent);

        renderer.EnqueuePass(_outlinePass);
    }

    public override void Create()
    {
        _outlinePass = new OutlinePass("My Outline Pass", settings.WhenToInsert,
            settings.MaterialToBlit);
    }
}
