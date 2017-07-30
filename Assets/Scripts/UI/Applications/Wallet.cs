using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD39.UI.Applications
{
    public class Wallet : MonoBehaviour, IApp
    {
        public float powerUsage { get { return 5f; } set { } }

        public Text funds;

        private float oldFunds;
        private float newFunds;

        private float t = 0.0f;

        private void Start()
        {
            GameManager.instance.RegisterApplication(this);
            GameManager.instance.OnFundsUpdated += HandleFundsUpdated;
            funds.text = string.Format("Your wallet contains {0} PowerCoinz.", 0);
        }

        private void OnDestroy()
        {
            GameManager.instance.UnregisterApplication(this);
            GameManager.instance.OnFundsUpdated -= HandleFundsUpdated;
        }

        private void Update()
        {
            if (Mathf.Approximately(oldFunds, newFunds))
            {
                funds.text = funds.text = string.Format("Your wallet contains {0} PowerCoinz.", newFunds);
                t = 0.0f;
            }
            else
            {
                t += Time.deltaTime;
                funds.text = string.Format("Your wallet contains {0} PowerCoinz.", Mathf.Lerp(oldFunds, newFunds, t));
            }
        }

        private void HandleFundsUpdated(float funds)
        {
            oldFunds = float.Parse(this.funds.text);
            newFunds = funds;
        }
    }
}