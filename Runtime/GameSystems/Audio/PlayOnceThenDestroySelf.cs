using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class PlayOnceThenDestroySelf : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Start()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
            }

            audioSource.Stop();
            audioSource.PlayOneShot(audioSource.clip);
            float clipLength = audioSource.clip.length;
            this.CallWithDelay(DestroySelf, clipLength + 0.5f);
        }

        private void DestroySelf()
        {
            GameObject.Destroy(gameObject);
        }
    }
}