using Sirenix.OdinInspector;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

using UnityEngine;

namespace Rufas
{
    public class GameSystemParentClass : SerializedScriptableObject
    {
#if UNITY_EDITOR
        [HideInInspector]
        public bool showInManager = false;
        public virtual EditorIcon EditorIcon()
        {
            return EditorIcons.ArrowRight;
        }
#endif

        public virtual void OnEnable()
        {
            
        }
        /*
        /// <summary>
        /// Internal use! This will hide the system in the manager editor (revealed with the 'showRufasHiddenSystems' bool on the manager)
        /// </summary>
        /// <returns></returns>
        public virtual bool RufasBackendSystem()
        {
            return false;
        }
        */

        /// <summary>
        /// Autogenerates this scriptable manager without asking
        /// </summary>
        /// <returns></returns>
        public virtual bool AutogenerateGameSystem()
        {
            return false;
        }

        /// <summary>
        /// This method is called on initialization during the first frame. Before any monobehaviour callbacks!
        /// </summary>
        public virtual void BehaviourToRunBeforeAwake()
        {

        }


        public virtual string DesiredName()
        {
            string[] names = this.GetType().ToString().Split(".");
            return names[names.Length - 1];
        }

        public virtual string DesiredPath()
        {
            return this.name;
        }

        public virtual void OnCreatedByEditor()
        {

        }

    }
}
