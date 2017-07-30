using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD39.UI.Applications
{
    public class WritePad : MonoBehaviour, IApp
    {
        public float powerUsage { get { return 20f; } set { } }

        public InputField inputField;
        public Text charCount;
        public Button saveButton;
        public Button helpButton;

        private string output;

        private void Start()
        {
            GameManager.instance.RegisterApplication(this);

            output = GameManager.instance.textsManager.texts[UnityEngine.Random.Range(0, GameManager.instance.textsManager.texts.Count)];

            inputField.onValueChanged.AddListener(delegate
            {
                // Maybe?
                if (output.Length > inputField.text.Length)
                {
                    inputField.text = output.Substring(0, inputField.text.Length);
                    charCount.text = string.Format("Chars: {0}", inputField.text.Length);
                }
            });

            saveButton.onClick.AddListener(delegate
            {
                SaveDocument(inputField.text.Substring(0, 8));
                WindowManager.instance.Close(transform.parent.parent.GetComponent<Window>());
                AudioManager.instance.PlaySound("saveDoc");
            });
        }

        private void OnDestroy()
        {
            GameManager.instance.UnregisterApplication(this);
        }

        public void SaveDocument(string name)
        {
            Document doc = new Document(name, DocumentType.DOC);
            doc.writingText = output;
            doc.contentLength = inputField.text.Length;
            GameManager.instance.AddDocument(doc);
        }

        public void LoadDocument(Document doc)
        {
            output = doc.writingText;
            inputField.text = output.Substring(0, doc.contentLength);
        }
    }
}