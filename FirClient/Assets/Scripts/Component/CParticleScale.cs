using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Component
{
    public class CParticleScale : MonoBehaviour
    {
        private bool isScaled = false;
        private ParticleSystem[] mParticles;
        private List<float> initSizes = new List<float>();

        // Start is called before the first frame update
        void Awake()
        {
            InitScale();
        }

        void InitScale()
        {
            initSizes.Clear();
            mParticles = GetComponentsInChildren<ParticleSystem>(true);
            for (int i = 0; i < mParticles.Length; i++)
            {
                var particle = mParticles[i];
                initSizes.Add(particle.startSize);
                initSizes.Add(particle.startSpeed);

                var renderer = particle.GetComponent<ParticleSystemRenderer>();
                if (renderer)
                {
                    initSizes.Add(renderer.lengthScale);
                    initSizes.Add(renderer.velocityScale);
                }
                switch(particle.shape.shapeType)
                {
                    case ParticleSystemShapeType.Sphere:
                    case ParticleSystemShapeType.Cone:
                    case ParticleSystemShapeType.Hemisphere:
                    case ParticleSystemShapeType.Circle:
                    case ParticleSystemShapeType.CircleEdge:
                        initSizes.Add(particle.shape.radius);
                        break;
                    case ParticleSystemShapeType.Box:
                        initSizes.Add(particle.shape.scale.x);
                        initSizes.Add(particle.shape.scale.y);
                        initSizes.Add(particle.shape.scale.z);
                    break;
                    case ParticleSystemShapeType.Mesh:
                    case ParticleSystemShapeType.MeshRenderer:
                    case ParticleSystemShapeType.SkinnedMeshRenderer:
                        initSizes.Add(particle.shape.normalOffset);
                    break;
                }
            }
        }

        void SetScale(float size)
        {
            if (transform == null || mParticles == null)
            {
                return;
            }
            isScaled = true;
            transform.localScale = new Vector3(size, size, size);

            int arrayIndex = 0;
            for(int i = 0; i < mParticles.Length; i++)
            {
                var particle = mParticles[i];
                if (particle == null) continue;
                particle.startSize = initSizes[arrayIndex++] * size;
                particle.startSpeed = initSizes[arrayIndex++] * size;
                
                var renderer = particle.GetComponent<ParticleSystemRenderer>();
                if (renderer)
                {
                    renderer.lengthScale = initSizes[arrayIndex++] * size;
                    renderer.velocityScale = initSizes[arrayIndex++] * size;
                }
                var newShape = particle.shape;
                switch(newShape.shapeType)
                {
                    case ParticleSystemShapeType.Sphere:
                    case ParticleSystemShapeType.Cone:
                    case ParticleSystemShapeType.Hemisphere:
                    case ParticleSystemShapeType.Circle:
                    case ParticleSystemShapeType.CircleEdge:
                        newShape.radius = initSizes[arrayIndex++] * size;   
                    break;
                    case ParticleSystemShapeType.Box:
                        var x = initSizes[arrayIndex++] * size;
                        var y = initSizes[arrayIndex++] * size;
                        var z = initSizes[arrayIndex++] * size;
                        newShape.scale = new Vector3(x, y, z);
                    break;
                    case ParticleSystemShapeType.Mesh:
                    case ParticleSystemShapeType.MeshRenderer:
                    case ParticleSystemShapeType.SkinnedMeshRenderer:
                        newShape.normalOffset = initSizes[arrayIndex++] * size;
                    break;
                }
            }
        }

        public void ResetScale()
        {
            if (isScaled)
            {
                SetScale(1f);
            }
            isScaled = false;
        }
    }
}

