using Rufas;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class DatabaseEntry
{
    //[ReadOnly]
    //public string key;
    private object value;

   // public event Action<object> onValueChanged;

    [ShowInInspector]
    public object Value
    {
        get { return value; }
        set
        {
            if (value != this.value)
            {
                this.value = value;
             //   onValueChanged?.Invoke(this.value);
            }
        }
    }      
}
