using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD39.UI.Applications
{
    public class Battery : MonoBehaviour, IApp
    {
        public float powerUsage { get { return 0f; } set { } }

        public Text percentageRemaining;
        public Image batteryImage;

        public Sprite battery0;
        public Sprite battery25;
        public Sprite battery50;
        public Sprite battery75;
        public Sprite battery100;

        private void Start()
        {
            GameManager.instance.OnPowerUpdated += HandlePowerUpdated;
            GameManager.instance.RegisterApplication(this);
        }

        private void OnDestroy()
        {
            GameManager.instance.OnPowerUpdated -= HandlePowerUpdated;
            GameManager.instance.UnregisterApplication(this);
        }

        private void HandlePowerUpdated(float power, float percentage)
        {
            percentageRemaining.text = string.Format("{0}%", Mathf.Round(percentage * 100f));

            batteryImage.sprite = battery0;
            if (percentage >= 0.25f) batteryImage.sprite = battery25;
            if (percentage >= 0.50f) batteryImage.sprite = battery50;
            if (percentage >= 0.75f) batteryImage.sprite = battery75;
            if (percentage >= 1.00f) batteryImage.sprite = battery100;
        }
    }
}
