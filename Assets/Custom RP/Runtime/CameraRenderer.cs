using UnityEngine;
using UnityEngine.Rendering;

namespace WillakeD.CustomRP
{
    public class CameraRenderer
    {
        const string BUFFER_NAME = "Render Camera";
        static ShaderTagId UNLIT_SHADER_TAG_ID = new ShaderTagId("SRPDefaultUnlit");

        ScriptableRenderContext _context;
        Camera _camera;

        CommandBuffer _buffer = new CommandBuffer
        {
            name = BUFFER_NAME
        };

        CullingResults _cullingResults;

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this._context = context;
            this._camera = camera;

            if (!Cull())
            {
                return;
            }

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

        bool Cull()
        {
            if (_camera.TryGetCullingParameters(out ScriptableCullingParameters p))
            {
                _cullingResults = _context.Cull(ref p);
                return true;
            }
            return false;
        }

        void DrawVisibleGeometry()
        {
            var sortingSettings = new SortingSettings(_camera)
            {
                criteria = SortingCriteria.CommonOpaque
            };
            var drawingSettings = new DrawingSettings(
                UNLIT_SHADER_TAG_ID, sortingSettings
            );
            var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

            _context.DrawRenderers(
                _cullingResults, ref drawingSettings, ref filteringSettings
            );
            _context.DrawSkybox(_camera);

            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSettings.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;

            _context.DrawRenderers(
                _cullingResults, ref drawingSettings, ref filteringSettings
            );
        }
    }
}