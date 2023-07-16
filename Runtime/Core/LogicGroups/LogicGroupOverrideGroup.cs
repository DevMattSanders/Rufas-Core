using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/LogicGroups/LogicGroupOverrideGroup")]
    public class LogicGroupOverrideGroup : SuperScriptable
    {
        
        public LogicGroup overridingLogicGroup { get; private set; }

        private string CurrentOverride()
        {
            if(overridingLogicGroup == null)
            {
                return ("Current Override: None");
            }
            else
            {
               return "Current Override: " + overridingLogicGroup.name;
            }
        }

        [Title("$CurrentOverride", subtitle:"Higer overrides lower", titleAlignment: TitleAlignments.Left, horizontalLine: true, bold: true)]
        [ListDrawerSettings(Expanded = true)]
        public List<LogicGroup> logicGroups = new List<LogicGroup>();

        public override void SoOnAwake()
        {
            base.SoOnAwake();

            ResetValues();

            foreach (LogicGroup nextGroup in logicGroups)
            {
                nextGroup.IsEnabled.AddListener(LogicGroupEnabledChanged);
            }
        }

        public override void SoOnStart()
        {
            base.SoOnStart();

            LogicGroupEnabledChanged(false);
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();

            ResetValues();

            foreach (LogicGroup nextGroup in logicGroups)
            {
                nextGroup.IsEnabled.RemoveListener(LogicGroupEnabledChanged);
            }
        }


        private void ResetValues()
        {
            overridingLogicGroup = null;
        }

        private void LogicGroupEnabledChanged(bool ignore)
        {
            bool overrideNextGroup = false;
            overridingLogicGroup = null;


            foreach (LogicGroup nextGroup in logicGroups)
            {
                if(overrideNextGroup == false)
                {
                    nextGroup.UnoverrideLogicGroup();

                    if (nextGroup.IsEnabled.Value)
                    {
                        overrideNextGroup = true;

                        if (nextGroup != overridingLogicGroup)
                        {
                             //Debug.Log("Override Applied: " + nextGroup.name);
                            overridingLogicGroup = nextGroup;
                        }
                    }
                    else if(overridingLogicGroup == nextGroup && overridingLogicGroup != null)
                    {
                        //Debug.Log("Override Removed: " + overridingLogicGroup.name);
                    }
                }
                else
                {
                    nextGroup.OverrideLogicGroup();
                }
            }
        }
    }
}
