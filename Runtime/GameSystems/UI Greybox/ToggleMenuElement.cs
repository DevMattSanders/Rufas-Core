using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class ToggleMenuElement : MonoBehaviour
    {
        [SerializeField] private GameObject menuElement;

        public void ToggleElement(bool toggle)
        {
            menuElement.SetActive(toggle);
        }
    }
}
