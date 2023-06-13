using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class SkyboxManager : MonoBehaviour
    {


        [Button]
        public void SetSkyboxAndReflections(Material material, Cubemap reflectionMap)
        {
            if (material == null) return;

            RenderSettings.skybox = material;

            RenderSettings.customReflection = reflectionMap;

            //RenderSettings.
        }
    }
}
