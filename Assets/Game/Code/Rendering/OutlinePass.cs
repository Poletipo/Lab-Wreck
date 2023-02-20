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

    public OutlinePass(string profilerTag, RenderPassEvent renderPassEvent, Material materialToBlit) {
        this.profilerTag = profilerTag;
        this.renderPassEvent = renderPassEvent;
        this.materialToBlit = materialToBlit;
    }

    public void Setup(RenderTargetIdentifier cameraColorTargetIdent) {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
        cmd.GetTemporaryRT(tempTextureRT.id, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

        CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
        cmd.Clear();

        cmd.Blit(cameraColorTargetIdent, tempTextureRT.Identifier(), materialToBlit, 0);
        cmd.Blit(tempTextureRT.Identifier(), cameraColorTargetIdent);

        context.ExecuteCommandBuffer(cmd);

        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd) {
        cmd.ReleaseTemporaryRT(tempTextureRT.id);
    }
}
