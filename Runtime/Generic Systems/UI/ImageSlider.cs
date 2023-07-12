
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rufas
{
    public class ImageSlider : MonoBehaviour
    {
        [Range(0f, 1f), SerializeField] private float value;
        [Space]
        [SerializeField] private Image imageSlider;
        [SerializeField] private Gradient colourGradient;


        //private void OnValidate()
        //{
        //    if (imageSlider != null) { UpdateSlider(value); }
        //}


        public void UpdateSlider(float _value)
        {
            Debug.Log("Update slider");

            value = _value;
            imageSlider.fillAmount = _value;
            imageSlider.color = colourGradient.Evaluate(value);
        }
    }
}
