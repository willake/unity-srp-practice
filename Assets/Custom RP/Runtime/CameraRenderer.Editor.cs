using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace WillakeD.CustomRP
{
    public partial class CameraRenderer : MonoBehaviour
    {
        partial void PrepareBuffer();
        partial void PrepareForSceneWindow();
        partial void DrawGizmos();
        partial void DrawUnsupportedShaders();
#if UNITY_EDITOR

        static ShaderTagId[] legacyShaderTagIds = {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };

        static Material errorMaterial;

        partial void PrepareForSceneWindow()
        {
            if (_camera.cameraType == CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(_camera);
            }
        }

        partial void DrawGizmos()
        {
            if (Handles.ShouldRenderGizmos())
            {
                _context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
                _context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);
            }
        }

        partial void DrawUnsupportedShaders()
        {
            if (errorMaterial == null)
            {
                errorMaterial =
                    new Material(Shader.Find("Hidden/InternalErrorShader"));
            }

            var drawingSettings = new DrawingSettings(
                legacyShaderTagIds[0], new SortingSettings(_camera)
            )
            {
                overrideMaterial = errorMaterial
            };

            for (int i = 1; i < legacyShaderTagIds.Length; i++)
            {
                drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
            }

            var filteringSettings = FilteringSettings.defaultValue;
            _context.DrawRenderers(
                _cullingResults, ref drawingSettings, ref filteringSettings
            );
        }

        string SampleName { get; set; }

        partial void PrepareBuffer()
        {
            _buffer.name = SampleName = _camera.name;
        }
#else
	    const string SampleName = BUFFER_NAME;
#endif
    }
}