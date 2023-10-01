using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rufas.BasicFunctions
{
    [RequireComponent(typeof(Slider))] public class FloatVariableSliderBridge : MonoBehaviour
    {
        private Slider slider;
        [SerializeField] private FloatVariable variable;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.SetValueWithoutNotify(variable.Value);
        }

        private void Start()
        {
            slider.onValueChanged.AddListener(RefreshValue);
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(RefreshValue);
        }

        public void RefreshValue(float value)
        {
            variable.Value = value;
        }
    }
}
