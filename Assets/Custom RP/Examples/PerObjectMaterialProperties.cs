using UnityEngine;

namespace WillakeD.CustomRP.Examples
{
    [DisallowMultipleComponent]
    public class PerObjectMaterialProperties : MonoBehaviour
    {

        static readonly int baseColorId = Shader.PropertyToID("_BaseColor");
        static readonly int cutoffId = Shader.PropertyToID("_Cutoff");
        static readonly int metallicId = Shader.PropertyToID("_Metallic");
        static readonly int smoothnessId = Shader.PropertyToID("_Smoothness");
        static MaterialPropertyBlock block;

        [SerializeField]
        Color baseColor = Color.white;

        [SerializeField, Range(0f, 1f)]
        float alphaCutoff = 0.5f, metallic = 0f, smoothness = 0.5f;

        void Awake()
        {
            OnValidate();
        }

        void OnValidate()
        {
            if (block == null)
            {
                block = new MaterialPropertyBlock();
            }
            block.SetColor(baseColorId, baseColor);
            block.SetFloat(cutoffId, alphaCutoff);
            block.SetFloat(metallicId, metallic);
            block.SetFloat(smoothnessId, smoothness);
            GetComponent<Renderer>().SetPropertyBlock(block);
        }
    }
}