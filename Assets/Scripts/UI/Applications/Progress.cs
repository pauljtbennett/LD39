using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD39.UI.Applications
{
    public class Progress : MonoBehaviour, IApp
    {
        public float powerUsage { get { return 50f; } set { } }

        public Text progressText;
        public Image barFill;

        private float speed = 0f;
        private float progress = 0f;

        private void Start()
        {
            GameManager.instance.RegisterApplication(this);
        }

        private void OnDestroy()
        {
            GameManager.instance.UnregisterApplication(this);
        }

        private void Update()
        {
            progress += (Time.deltaTime * (speed / 100f));
            progress = Mathf.Clamp(progress, 0f, 1f);
            barFill.fillAmount = progress;
            progressText.text = string.Format("Processing... {0}%", Mathf.Round(progress * 100f));

            if (progress >= 1f)
            {
                WindowManager.instance.Close(transform.parent.parent.GetComponent<Window>(), true);
            }
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }
    }
}