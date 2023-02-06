using UnityEngine;
using UnityEngine.Rendering;

namespace WillakeD.CustomRP
{
    public class CameraRenderer
    {
        ScriptableRenderContext context;

        Camera camera;

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this.context = context;
            this.camera = camera;
        }
    }
}