using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD39
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public AudioClip newEmail;
        public AudioClip replyEmail;
        public AudioClip saveDoc;
        public AudioClip attachDoc;
        public AudioClip rotateImage;
        public AudioClip invertImage;
        public AudioClip calcButton;
        public AudioClip login;

        private AudioSource audioSource;
        private Dictionary<string, AudioClip> clips;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            audioSource = GetComponent<AudioSource>();
            clips = new Dictionary<string, AudioClip>();
        }

        private void Start()
        {
            clips.Add("newEmail", newEmail);
            clips.Add("replyEmail", replyEmail);
            clips.Add("saveDoc", saveDoc);
            clips.Add("attachDoc", attachDoc);
            clips.Add("rotateImage", rotateImage);
            clips.Add("invertImage", invertImage);
            clips.Add("calcButton", calcButton);
            clips.Add("login", login);
        }

        public void PlaySound(string soundName)
        {
            if (clips.ContainsKey(soundName) && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(clips[soundName]);
            }
        }
    }
}