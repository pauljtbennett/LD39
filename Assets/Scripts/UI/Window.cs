using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LD39.UI
{
    public class Window : MonoBehaviour, IPointerClickHandler
    {
        public Text titleText;
        public Button closeButton;
        public GameObject contentPane;

        public bool resizable;
        public bool maximizable;
        public bool canClose;

        private int width;
        private int height;
        private bool maximized;

        private void Start()
        {
            closeButton.onClick.AddListener(delegate { WindowManager.instance.Close(this); });
        }

        private void Update()
        {
            closeButton.gameObject.SetActive(canClose);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GetComponent<RectTransform>().SetAsLastSibling();
        }

        public void Setup(string title, GameObject innerContent)
        {
            titleText.text = title;
            GameObject go = contentPane.AddChild(innerContent);
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}