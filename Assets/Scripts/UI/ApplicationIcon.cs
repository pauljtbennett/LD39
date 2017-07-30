using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LD39.UI
{
    public class ApplicationIcon : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public GameObject appToOpen;
        public Image appBG;
        public Text appText;
        public int windowWidth = 400;
        public int windowHeight = 300;
        public bool resizable = true;

        public bool hasFocus;

        private int tap;
        private float interval = 0.75f;
        private float timer = 0f;
        private bool readyForDoubleTap;

        public void OnPointerClick(PointerEventData eventData)
        {
            // Crudely lose focus on other icons
            foreach (var icon in FindObjectsOfType<ApplicationIcon>())
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
                if (appToOpen != null)
                {
                    WindowManager.instance.Open(appText.text, appToOpen, windowWidth, windowHeight);
                }
                else
                {
                    // Assume log off
                    GameManager.instance.EndGame();
                }

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
            GetComponent<RectTransform>().anchoredPosition = new Vector2(eventData.position.x - (Screen.width / 2), eventData.position.y - (Screen.height / 2));
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