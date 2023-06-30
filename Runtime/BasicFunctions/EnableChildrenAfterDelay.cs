using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.BasicFunctions
{
    public class EnableChildrenAfterDelay : MonoBehaviour
    {
        [SerializeField] private float delay;

        [SerializeField] private bool disableChildrenFirst;
        [SerializeField] private int childCount;// = transform.childCount;
        private IEnumerator Start()
        {
            childCount = transform.childCount;

            if (disableChildrenFirst)
            {
                foreach (Transform next in transform.GetComponentsInChildren<Transform>(false))
                {
                    if (next == transform) continue;

                    next.gameObject.SetActive(false);
                    // transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            yield return new WaitForSeconds(delay);

            //  for (int i = 0; i < childCount; i++)

            foreach (Transform next in transform.GetComponentsInChildren<Transform>(true))

            {
                if (next == transform) continue;

                next.gameObject.SetActive(true);
                //Debug.Log("Here " + transform.GetChild(i).name);
                //transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}