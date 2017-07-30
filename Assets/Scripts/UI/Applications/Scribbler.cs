using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace LD39.UI.Applications
{
    public class Scribbler : MonoBehaviour, IApp
    {
        private const int ROTATE_AMOUNT = 90;

        public float powerUsage { get { return 25f; } set { } }

        public RawImage image;

        public Button saveButton;
        public Button rotateButton;
        public Button invertButton;
        public Button helpButton;

        public Shader invertShader;

        private Document openedDoc;

        private void Start()
        {
            GameManager.instance.RegisterApplication(this);

            saveButton.onClick.AddListener(delegate
            {
                SaveDocument(openedDoc.name.Substring(0, 8));
                WindowManager.instance.Close(transform.parent.parent.GetComponent<Window>());
                AudioManager.instance.PlaySound("saveDoc");
            });

            rotateButton.onClick.AddListener(delegate
            {
                openedDoc.rotation += ROTATE_AMOUNT;
                image.GetComponent<RectTransform>().Rotate(0, 0, ROTATE_AMOUNT);
                AudioManager.instance.PlaySound("rotateImage");
            });

            invertButton.onClick.AddListener(delegate
            {
                openedDoc.inverted = !openedDoc.inverted;
                /*
                if (image.material.HasProperty("_Color"))
                {
                    image.material.color = InvertColor(image.material.color);
                }
                */
                AudioManager.instance.PlaySound("invertImage");
            });
        }

        private void OnDestroy()
        {
            GameManager.instance.UnregisterApplication(this);
        }

        private Color InvertColor(Color color)
        {
            return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
        }

    public void SaveDocument(string name)
        {
            Document doc = new Document(name, DocumentType.BMP);
            GameManager.instance.AddDocument(doc);
        }

        public void LoadDocument(Document doc)
        {
            openedDoc = doc;
            image.texture = Resources.Load<Texture>(Path.Combine("Images", doc.imagePath));
        }
    }
}