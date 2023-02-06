using UnityEngine;
using UnityEngine.Rendering;

namespace WillakeD.CustomRP
{
    public class CameraRenderer
    {
        const string BUFFER_NAME = "Render Camera";

        ScriptableRenderContext _context;
        Camera _camera;

        CommandBuffer buffer = new CommandBuffer
        {
            name = BUFFER_NAME
        };

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this._context = context;
            this._camera = camera;

            Setup();
            DrawVisibleGeometry();
            Submit();
        }

        void DrawVisibleGeometry()
        {
            _context.DrawSkybox(_camera);
        }

        void Submit()
        {
            _context.Submit();
        }

        void Setup()
        {
            _context.SetupCameraProperties(_camera);
        }
    }
}