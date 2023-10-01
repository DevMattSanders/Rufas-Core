using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rufas.BasicFunctions
{
    [RequireComponent(typeof(Slider))] public class FloatVariableSlider : MonoBehaviour
    {
        private Slider slider;
        [SerializeField] private FloatVariable variable;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.value = variable.Value;
        }

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(SetVariableValue);
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(SetVariableValue);
        }

        public void SetVariableValue(float value)
        {
            variable.Value = value;
        }
    }
}
