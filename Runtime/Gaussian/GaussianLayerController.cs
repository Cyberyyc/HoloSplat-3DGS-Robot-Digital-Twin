using UnityEngine;
using GaussianSplatting.Runtime;

namespace HoloSplat
{
    /// <summary>Presentation controls for the external Unity Gaussian Splatting renderer.</summary>
    public sealed class GaussianLayerController : MonoBehaviour
    {
        [SerializeField] GaussianSplatRenderer rendererComponent;
        [SerializeField,Range(0,1)] float opacity=1;
        [SerializeField,Range(1,8)] int sortEveryNthFrame=1;
        [SerializeField,Range(0,3)] int sphericalHarmonicsOrder=3;
        void OnEnable() => Apply();
        void OnValidate() => Apply();
        public void SetOpacity(float value) { opacity=Mathf.Clamp01(value); Apply(); }
        public void SetVisible(bool visible) { if(rendererComponent!=null) rendererComponent.enabled=visible; }
        public void Apply()
        {
            if(rendererComponent==null) return;
            rendererComponent.m_OpacityScale=opacity;
            rendererComponent.m_SortNthFrame=Mathf.Max(1,sortEveryNthFrame);
            rendererComponent.m_SHOrder=Mathf.Clamp(sphericalHarmonicsOrder,0,3);
        }
    }
}
