using System;
using UnityEngine;

namespace HoloSplat
{
    /// <summary>Yaw-only least-squares registration from paired floor-plane landmarks.</summary>
    public sealed class GaussianSceneRegistration : MonoBehaviour
    {
        [SerializeField] Transform gaussianRoot;
        [SerializeField] Transform robotMapRoot;
        [SerializeField] bool estimateUniformScale;

        public void Register(Vector3[] modelPoints, Vector3[] worldPoints)
        {
            if (gaussianRoot == null) throw new InvalidOperationException("Assign the Gaussian scene root.");
            if (robotMapRoot != null && (robotMapRoot == gaussianRoot || robotMapRoot.IsChildOf(gaussianRoot) || gaussianRoot.IsChildOf(robotMapRoot)))
                throw new InvalidOperationException("Scene and robot roots must be independent siblings.");
            Solve(modelPoints, worldPoints, estimateUniformScale, out var position, out var rotation, out var scale);
            Apply(gaussianRoot, position, rotation, scale);
            if (robotMapRoot != null) Apply(robotMapRoot, position, rotation, scale);
        }

        static void Apply(Transform root, Vector3 position, Quaternion rotation, float scale)
        {
            if (root.parent != null && (root.parent.lossyScale-Vector3.one).sqrMagnitude > 1e-6f)
                throw new InvalidOperationException("Registration roots require unit-scale parents.");
            root.SetPositionAndRotation(position, rotation);
            root.localScale = Vector3.one * scale;
        }

        public static void Solve(Vector3[] source, Vector3[] target, bool fitScale,
            out Vector3 translation, out Quaternion rotation, out float scale)
        {
            if (source == null || target == null || source.Length != target.Length || source.Length < 2)
                throw new ArgumentException("At least two paired landmarks are required.");
            Vector3 a=Vector3.zero, b=Vector3.zero;
            for (int i=0;i<source.Length;i++)
            {
                if (!Finite(source[i]) || !Finite(target[i])) throw new ArgumentException("Landmarks must be finite.");
                a+=source[i]; b+=target[i];
            }
            a/=source.Length; b/=source.Length;
            double dot=0, cross=0, energy=0;
            for(int i=0;i<source.Length;i++)
            {
                var x=source[i]-a; var y=target[i]-b;
                dot+=x.x*y.x+x.z*y.z; cross+=x.z*y.x-x.x*y.z;
                energy+=x.x*x.x+x.z*x.z;
            }
            double correlation=Math.Sqrt(dot*dot+cross*cross);
            if(energy<1e-8 || correlation<1e-8) throw new ArgumentException("Degenerate floor-plane landmarks.");
            rotation=Quaternion.AngleAxis((float)(Math.Atan2(cross,dot)*180/Math.PI),Vector3.up);
            scale=fitScale?(float)(correlation/energy):1f;
            translation=b-rotation*(a*scale);
        }
        static bool Finite(Vector3 v) => !(float.IsNaN(v.x)||float.IsInfinity(v.x)||float.IsNaN(v.y)||float.IsInfinity(v.y)||float.IsNaN(v.z)||float.IsInfinity(v.z));
    }
}
