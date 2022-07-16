using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.UI
{
    public static class Utility
    {
        private static PointerEventData _eventDataCurrentPosition;
        private static List<RaycastResult> _results;
        public static bool IsOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition,_results);
            return _results.Count > 0;
        }

        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera.main, out var result);
            return result;
        }
        
        // public static Vector2 GetCanvasPositionOfWorldPoint(Vector3 worldPoint)
        // {
        //     return RectTransformUtility.WorldToScreenPoint(Camera.main, worldPoint);
        // }

        public static void DeleteChildren(this Transform t)
        {
            foreach (Transform child in t)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}