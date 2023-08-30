using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Rufas
{
    public class Condition_ObjectsInList : GenericCondition
    {
        [HorizontalGroup("V", Width = 80)]
        [HideLabel]
        [GUIColor("$GuiColour")]
        [DisableInPlayMode]
        public ActiveState compareTo = ActiveState.Active;

        public enum ActiveState
        {
            Active,
            NotActive
        }

        [SerializeField] private List<Object> conditionList = new List<Object>();

        public void AddListener()
        {

        }

        public void RemoveListener()
        {

        }
    }
}
