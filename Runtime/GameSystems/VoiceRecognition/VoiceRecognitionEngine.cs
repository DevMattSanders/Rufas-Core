using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Rufas
{
    public class VoiceRecognitionEngine : MonoBehaviour
    {
        public string[] keywords = new string[] { };
        public ConfidenceLevel confidence = ConfidenceLevel.Low;
        public float speed = 1;

        protected PhraseRecognizer recognizer; //
        protected string word;

        public CodeEvent<string> OnWordRecognised;

        private void Start()
        {
            if (keywords != null)  //makes sure there is actually stuff in array
            {
                recognizer = new KeywordRecognizer(keywords, confidence);  //init speech rec with keywords string array & con lvl
                recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized; //c# event (when its rec it shows its been rec)
                recognizer.Start(); //basically un mutes the mic
            }
        }

        private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args) //args is the rec word
        {
            word = args.text; //save rec word to strng
            Debug.Log("Word Heard: " +  word);
            OnWordRecognised.Raise(word);
            //results.SetText("Word Recognised: " + word);
        }

        private void OnApplicationQuit()
        {
            if (recognizer != null && recognizer.IsRunning)
            {
                recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
                recognizer.Stop();
            }
        }
    }
}
