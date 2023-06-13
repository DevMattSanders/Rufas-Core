using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;

namespace Rufas
{
#if UNITY_EDITOR
    public class UpdateSuperScriptablesBeforeBuild : IPreprocessBuildWithReport
    {

        public int callbackOrder => Ignore();// OnPreprocessBuild(new BuildReport); throw new System.NotImplementedException();

        private int Ignore()
        {
            return 0;
        }

        public void OnPreprocessBuild(BuildReport report)
        {
           // throw new System.NotImplementedException();

            //Refresh update list. Run pre build behaviour
        }

    }
#endif
}