using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Rufas.Quests
{

    public class TextFadeOut : MonoBehaviour
    {
        private bool shouldFade;
        public float duration = 2f; // Duration of the fade-out effect in seconds
        public AnimationCurve curve; // Curve for controlling the fade-out effect

        private TextMeshProUGUI textMeshPro;
        private float elapsedTime = 0f;
        private Color initialColor;

        public void FadeText()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            textMeshPro.enabled = true;
            Color newColour = textMeshPro.color;
            newColour.a = 1;
            initialColor = newColour;
            elapsedTime = 0;
            shouldFade = true;
        }

        private void Update()
        {
            if (!shouldFade) { return; }

            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration)
            {
                // Calculate the normalized time
                float normalizedTime = elapsedTime / duration;

                // Evaluate the curve to get the fade-out factor
                float fadeOutFactor = curve.Evaluate(normalizedTime);

                // Set the text color with the fade-out factor
                Color textColor = initialColor;
                textColor.a = 1f - fadeOutFactor;
                textMeshPro.color = textColor;
            }
            else
            {
                // Fade-out completed, disable the text element
                elapsedTime = 0;
                shouldFade = false;
                textMeshPro.enabled = false;
            }
        }
    }
}
