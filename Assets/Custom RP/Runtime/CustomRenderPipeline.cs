using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace WillakeD.CustomRP
{
    public class CustomRenderPipeline : RenderPipeline
    {
        bool useDynamicBatching, useGPUInstancing;
        CameraRenderer _renderer = new CameraRenderer();

        public CustomRenderPipeline(
            bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher
        )
        {
            this.useDynamicBatching = useDynamicBatching;
            this.useGPUInstancing = useGPUInstancing;
            GraphicsSettings.useScriptableRenderPipelineBatching = true;
        }

        protected override void Render(
            ScriptableRenderContext context, Camera[] cameras
        )
        {
            foreach (Camera camera in cameras)
            {
                _renderer.Render(context, camera, useDynamicBatching, useGPUInstancing);
            }
        }

        protected override void Render(
            ScriptableRenderContext context, List<Camera> cameras
        )
        {
            foreach (Camera camera in cameras)
            {
                _renderer.Render(context, camera, useDynamicBatching, useGPUInstancing);
            }
        }
    }
}
