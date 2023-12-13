using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Rufas
{
    public class GameSystemGroup : ScriptableObject
    {
        [HideInInspector] public SdfIconType iconType;

        // [TitleGroup("$Title",indent: true,AnimateVisibility = true)]
        //[TableList]
        //[TableMatrix()]
        [InlineEditor(InlineEditorModes.GUIOnly, InlineEditorObjectFieldModes.Hidden), HideDuplicateReferenceBox]
        [ListDrawerSettings(ShowFoldout = false, ShowIndexLabels = false, ShowItemCount = false, ShowPaging = false, HideRemoveButton = true)]        
        public List<GameSystemParentClass> gameSystems = new List<GameSystemParentClass>();

        private string Title()
        {
            return this.name;
        }
    }
}
