using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlinePass : ScriptableRenderPass {

    string profilerTag;
    Material materialToBlit;
    private RenderTargetIdentifier cameraColorTargetIdent;
    private RenderTargetHandle tempTextureRT;

    LayerMask mask;
    int maskValue;
    FilteringSettings filters;

    private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

    public OutlinePass(string profilerTag, RenderPassEvent renderPassEvent, Material materialToBlit, LayerMask layerMask) {
        this.profilerTag = profilerTag;
        this.renderPassEvent = renderPassEvent;
        this.materialToBlit = materialToBlit;
        mask = layerMask;

    }

    public void Setup(RenderTargetIdentifier cameraColorTargetIdent) {
        this.cameraColorTargetIdent = cameraColorTargetIdent;

        maskValue = 1 << mask.value;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
        cmd.GetTemporaryRT(tempTextureRT.id, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

        CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
        cmd.Clear();

        cmd.Blit(cameraColorTargetIdent, tempTextureRT.Identifier(), materialToBlit, 0);
        cmd.Blit(tempTextureRT.Identifier(), cameraColorTargetIdent);



        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.all, mask.value);
        DrawingSettings drawingSettings = CreateDrawingSettings(ShaderTagId.none, ref renderingData, SortingCriteria.CommonOpaque);
        drawingSettings.overrideMaterialPassIndex = 0;
        context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);


        context.ExecuteCommandBuffer(cmd);

        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd) {
        cmd.ReleaseTemporaryRT(tempTextureRT.id);
    }
}
