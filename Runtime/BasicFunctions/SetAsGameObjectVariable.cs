using Rufas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAsGameObjectVariable : RufasMonoBehaviour
{
    public GameObjectVariable variable;
    public override void Awake()
    {
        base.Awake();

        if (variable == null) Debug.LogError("GameObjectVariable is null!");      
    }

    public override void Awake_AfterInitialisation()
    {
        base.Awake_AfterInitialisation();

        if (variable.value != null)
        {
            Debug.Log("Replacing GameObjectVariable Value - " + variable.value.name + " TO " + gameObject.name);
        }

        variable.value = gameObject;
    }


}
