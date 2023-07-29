using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlurFeature : ScriptableRendererFeature {

    BlurPass _blurPass;

    [System.Serializable]
    public class BlurFeatureSettings {
        public bool IsEnabled = true;
        public RenderPassEvent WhenToInsert = RenderPassEvent.AfterRendering;
        public Material MaterialToBlit;
        [Min(0)]
        public int BlurSize = 0;
        [Min(1)]
        public int SampleCount = 1;
    }

    public BlurFeatureSettings settings = new BlurFeatureSettings();


    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!settings.IsEnabled || settings.MaterialToBlit == null) {
            return;
        }

        _blurPass.Setup(renderer);

        renderer.EnqueuePass(_blurPass);
    }

    public override void Create()
    {
        _blurPass = new BlurPass(settings.MaterialToBlit, settings.WhenToInsert, settings.BlurSize,
            settings.SampleCount);
    }
}
