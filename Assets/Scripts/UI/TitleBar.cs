using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LD39.UI
{
    public class TitleBar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform windowTransform;

        private void Start()
        {
            windowTransform = transform.parent.GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            windowTransform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            windowTransform.anchoredPosition = new Vector2(eventData.position.x - (Screen.width / 2), eventData.position.y - (Screen.height / 2) - (windowTransform.sizeDelta.y / 2) + 12f);
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
}
