using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentMyselfOnStart : MonoBehaviour
{
    void Start()
    {
        transform.parent = null;
    }
}
