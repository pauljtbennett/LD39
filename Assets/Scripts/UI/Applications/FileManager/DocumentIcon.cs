using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace LD39.UI.Applications
{
    public class DocumentIcon : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image appBG;
        public Text appText;

        public bool hasFocus;

        private int tap;
        private float interval = 0.75f;
        private float timer = 0f;
        private bool readyForDoubleTap;

        private Document doc;
        private Action<Document> clickAction;

        public void Setup(Document doc, FileManager fileManager, Action<Document> clickAction = null)
        {
            appText.text = doc.name;

            this.doc = doc;
            this.clickAction = clickAction;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Crudely lose focus on other icons
            foreach (var icon in FindObjectsOfType<DocumentIcon>())
            {
                icon.hasFocus = false;
            }

            tap++;
            hasFocus = true;

            if (tap == 1)
            {
                readyForDoubleTap = true;
                timer = interval;
            }
            else if (tap > 1 && readyForDoubleTap)
            {
                if (clickAction != null) clickAction(doc);

                tap = 0;
                readyForDoubleTap = false;
                hasFocus = false;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnDrag(PointerEventData eventData)
        {
            //GetComponent<RectTransform>().anchoredPosition = new Vector2(eventData.position.x - (Screen.width / 2), eventData.position.y - (Screen.height / 2));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            hasFocus = false;
        }

        private void Update()
        {
            if (readyForDoubleTap)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    tap = 0;
                    readyForDoubleTap = false;
                }
            }

            if (hasFocus)
            {
                appBG.color = new Color(0, 0, 1, 1);
                appText.color = new Color(1, 1, 1, 1);
            }
            else
            {
                appBG.color = new Color(0, 0, 1, 0);
                appText.color = new Color(0, 0, 0, 1);
            }
        }
    }
}