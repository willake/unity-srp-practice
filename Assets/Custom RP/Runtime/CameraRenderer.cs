using UnityEngine;
using UnityEngine.Rendering;

namespace WillakeD.CustomRP
{
    public class CameraRenderer
    {
        const string BUFFER_NAME = "Render Camera";

        ScriptableRenderContext _context;
        Camera _camera;

        CommandBuffer _buffer = new CommandBuffer
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

        void Setup()
        {
            _context.SetupCameraProperties(_camera);
            _buffer.ClearRenderTarget(true, true, Color.clear);
            _buffer.BeginSample(BUFFER_NAME);
            ExecuteBuffer();
        }

        void Submit()
        {
            _buffer.EndSample(BUFFER_NAME);
            ExecuteBuffer();
            _context.Submit();
        }

        void ExecuteBuffer()
        {
            _context.ExecuteCommandBuffer(_buffer);
            _buffer.Clear();
        }

        void DrawVisibleGeometry()
        {
            _context.DrawSkybox(_camera);
        }
    }
}