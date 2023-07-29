using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurPass : ScriptableRenderPass {

    public BlurPass(Material matToBlit, RenderPassEvent renderPassEvent, int blurSize, int sampleCount)
    {
        _profilerTag = "BlurPass";
        _materialToBlit = matToBlit;
        _blurSize = blurSize;
        _sampleCount = sampleCount;
        this.renderPassEvent = renderPassEvent;
    }

    private string _profilerTag;
    private Material _materialToBlit;
    private int _blurSize = 0;
    private int _sampleCount = 1;

    private ScriptableRenderer _renderer;
    private RenderTargetIdentifier _cameraColorTargetIdent;
    private RenderTargetHandle _tempTextureRT;

    public void Setup(ScriptableRenderer renderer)
    {
        _renderer = renderer;
    }

    public override void Configure(CommandBuffer cmd,
        RenderTextureDescriptor cameraTextureDescriptor)
    {
        _cameraColorTargetIdent = _renderer.cameraColorTargetHandle;
        cmd.GetTemporaryRT(_tempTextureRT.id, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context,
        ref RenderingData renderingData)
    {
        _materialToBlit.SetInt("_BlurSize", _blurSize);
        _materialToBlit.SetInt("_SampleCount", _sampleCount);
        CommandBuffer cmd = CommandBufferPool.Get(_profilerTag);
        cmd.Clear();

        cmd.Blit(_cameraColorTargetIdent, _tempTextureRT.Identifier(), _materialToBlit, 0);
        cmd.Blit(_tempTextureRT.Identifier(), _cameraColorTargetIdent);

        context.ExecuteCommandBuffer(cmd);

        cmd.Clear();
        CommandBufferPool.Release(cmd);

    }
}
