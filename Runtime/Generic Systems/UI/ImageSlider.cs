
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rufas
{
    public class ImageSlider : MonoBehaviour
    {
        [Range(0f, 1f)] public float value;
        [Space]
        [SerializeField] private Image imageSlider;
        [SerializeField] private Gradient colourGradient;


        private void OnValidate()
        {
            if (imageSlider != null) { UpdateSlider(); }
        }

        private void UpdateSlider()
        {
            imageSlider.fillAmount = value;
            imageSlider.color = colourGradient.Evaluate(value);
        }
    }
}
