using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace WillakeD.CustomRP
{
    public class CustomRenderPipeline : RenderPipeline
    {
        CameraRenderer _renderer = new CameraRenderer();

        public CustomRenderPipeline()
        {
            GraphicsSettings.useScriptableRenderPipelineBatching = true;
        }

        protected override void Render(
            ScriptableRenderContext context, Camera[] cameras
        )
        {
            foreach (Camera camera in cameras)
            {
                _renderer.Render(context, camera);
            }
        }
    }
}
