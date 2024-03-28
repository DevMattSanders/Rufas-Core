using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/GameContent/GameContentReference")]
    public class GameContentReference : ScriptableWithUniqueID
    {
        [HideLabel,PreviewField(alignment: ObjectFieldAlignment.Left,height: 100)]
        [VerticalGroup("Left", order: 1)]
        public GameObject reference;

        [VerticalGroup("Right", order: 2)]
        [DisableInPlayMode, Toggle("Value")]
        [GUIColor("IsOwnableColour")]
        public MyToggleable isOwnable;
        private Color IsOwnableColour() { if (isOwnable.Value) { return new Color(0.804f, 0.918f, 1); } else { return new Color(1, 0.878f, 0.886f); } }

        [VerticalGroup("Right", order: 2)]
        [DisableInPlayMode, Toggle("Value")]
        [GUIColor("CanBeSavedAndLoadedColour")]
        public MyToggleable canBeSavedAndLoaded;
        private Color CanBeSavedAndLoadedColour() { if (canBeSavedAndLoaded.Value) { return new Color(0.804f, 0.918f, 1); } else { return new Color(1, 0.878f, 0.886f); } }


        [System.Serializable]
        public class MyToggleable { public bool Value; }
    }
}
