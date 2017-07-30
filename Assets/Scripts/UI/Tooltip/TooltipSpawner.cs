using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LD39.UI.Tooltip
{
    public class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string content;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.instance.tooltip.Show(content, gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.instance.tooltip.Hide();
        }

        public void SetContent(string content)
        {
            this.content = content;
        }
    }
}