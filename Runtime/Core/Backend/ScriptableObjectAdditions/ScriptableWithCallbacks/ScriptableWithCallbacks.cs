using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class ScriptableWithCallbacks : SerializedScriptableObject
    {
        public virtual void SoOnAwake() { }

        public virtual void SoOnStart() { }

        public virtual void SoOnEnd() { }

        public virtual void SoOnBeforeBuild() { }
    }
}
