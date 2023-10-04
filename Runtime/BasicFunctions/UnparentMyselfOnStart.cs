using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentMyselfOnStart : MonoBehaviour
{
    public float delay = 0;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);


        Unparent();
    }

    private void Unparent()
    {
        transform.parent = null;
    }
}
