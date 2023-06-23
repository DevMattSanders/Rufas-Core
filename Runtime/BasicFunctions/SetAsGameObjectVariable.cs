using Rufas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAsGameObjectVariable : MonoBehaviour
{
    public GameObjectVariable variable;
    private void Awake()
    {
        if (variable == null) Debug.LogError("GameObjectVariable is null!");

        if(variable.value != null)
        {
            Debug.Log("Replacing GameObjectVariable Value - " + variable.value.name + " TO " + gameObject.name);
        }

        variable.value = gameObject;
    }
}
