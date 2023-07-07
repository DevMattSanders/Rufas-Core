using UnityEngine;

namespace Rufas
{
    public class DragBuildTriggerPoint : MonoBehaviour
    {
        public bool canBuild = true;
        [Space]
        [SerializeField] private int validLayer;

        public CodeEvent OnBuiltInTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if (!canBuild) { return; }
            if (other.gameObject.layer != validLayer) { return; }
            DragBuildTool.Instance.UpdateToolTransformAndConnection(this);
        }
    }
}