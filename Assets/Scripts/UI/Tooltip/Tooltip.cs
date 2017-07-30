using System;
using UnityEngine;
using UnityEngine.UI;

namespace LD39.UI.Tooltip
{
    public class Tooltip : MonoBehaviour
    {
        public Text textbox;

        public void Show(string text, GameObject go)
        {
            textbox.text = text;
            transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 2.5f, 0);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}