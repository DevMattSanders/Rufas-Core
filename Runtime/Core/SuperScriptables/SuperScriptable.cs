using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [Obsolete]
    public class SuperScriptable : SerializedScriptableObject
    {
        public virtual void SoOnAwake()
        {

        }

        public virtual void SoOnStart()
        {

        }

        public virtual void SoOnEnd()
        {

        }


        //A method that is found and called before a build is started
        public virtual void SoOnBeforeBuild()
        {
        }

    }
}