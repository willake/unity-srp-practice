using UnityEngine;
using UnityEngine.Rendering;

namespace WillakeD.CustomRP
{
    public partial class CameraRenderer
    {
        const string BUFFER_NAME = "Render Camera";
        static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

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

            PrepareBuffer();
            PrepareForSceneWindow();
            if (!Cull())
            {
                return;
            }

            Setup();
            DrawUnsupportedShaders();
            DrawGizmos();
            DrawVisibleGeometry();
            Submit();
        }

        void Setup()
        {
            _context.SetupCameraProperties(_camera);
            _buffer.ClearRenderTarget(true, true, Color.clear);
            _buffer.BeginSample(SampleName);
            ExecuteBuffer();
        }

        void Submit()
        {
            _buffer.EndSample(SampleName);
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
                unlitShaderTagId, sortingSettings
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