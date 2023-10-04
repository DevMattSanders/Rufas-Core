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

        bool ignoreVariableEvents = false;
        bool ignoreSliderEvents = false;
        private void Awake()
        {
            slider = GetComponent<Slider>();            
        }

        private void Start()
        {
            //slider.value = variable.Value;

            VariableChanged(variable.Value);

            slider.onValueChanged.AddListener(SliderChanged);

            variable.AddListener(VariableChanged);

            //RefreshValue(slider.);
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(SliderChanged);

            variable.RemoveListener(VariableChanged);
        }

        private void VariableChanged(float val)
        {
            if (ignoreVariableEvents == false)
            {
                ignoreSliderEvents = true;
                slider.value = val;
                slider.Rebuild(CanvasUpdate.Prelayout);
                ignoreSliderEvents = false;
            }
        }

        public void SliderChanged(float value)
        {
            if (ignoreSliderEvents == false)
            {
                ignoreVariableEvents = true;
                variable.Value = value;
                ignoreVariableEvents = false;
            }
        }
    }
}
