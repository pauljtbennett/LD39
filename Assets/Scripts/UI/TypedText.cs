using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LD39.UI
{
    public class TypedText : MonoBehaviour
    {
        public float delay = 0.02f;
        
        private Text textbox;
        private AudioSource audio;
        private char[] charArray;
        private int charIndex;

        private void Awake()
        {
            textbox = GetComponent<Text>();
            audio = GetComponent<AudioSource>();
        }

        public void UpdateText(string text)
        {
            if (textbox != null && !string.IsNullOrEmpty(text))
            {
                StopAllCoroutines();
                charIndex = 0;
                charArray = text.ToCharArray();
                textbox.text = string.Empty;
                StartCoroutine(DelayedUpdate());
            }
        }

        private IEnumerator DelayedUpdate()
        {
            while (charIndex < charArray.Length)
            {
                audio.Play();
                textbox.text += charArray[charIndex];
                charIndex++;
                yield return new WaitForSeconds(delay);
            }
        } 
    }
}