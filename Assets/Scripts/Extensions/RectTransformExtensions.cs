using System.Collections.Generic;
using UnityEngine;

namespace AirCoder.Extensions
{
    public enum FreeAnchorsTypes
    {
        Undefined, MiddleCenter, TopCenter, BottomCenter, TopLeft, TopRight, MiddleLeft,MiddleRight ,BottomLeft , BottomRight 
    }
    
    public static class RectTransformExtensions
    {
        public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
        {
            Vector3 deltaPosition = rectTransform.pivot - pivot;    // get change in pivot
            deltaPosition.Scale(rectTransform.rect.size);           // apply sizing
            deltaPosition.Scale(rectTransform.localScale);          // apply scaling
            deltaPosition = rectTransform.rotation * deltaPosition; // apply rotation
     
            rectTransform.pivot = pivot;                            // change the pivot
            rectTransform.localPosition -= deltaPosition;           // reverse the position change
        }
    
        public static void SetAnchorMax(this RectTransform rectTransform, Vector2 anchor)
        {
            Vector3 deltaPosition = rectTransform.anchorMax - anchor;    // get change in pivot
            deltaPosition.Scale(rectTransform.rect.size);           // apply sizing
            deltaPosition.Scale(rectTransform.localScale);          // apply scaling
            deltaPosition = rectTransform.rotation * deltaPosition; // apply rotation
     
            rectTransform.anchorMax = anchor;                            // change the pivot
            rectTransform.localPosition -= deltaPosition;           // reverse the position change
        }
    
        public static void SetAnchorMin(this RectTransform rectTransform, Vector2 anchor)
        {
            Vector3 deltaPosition = rectTransform.anchorMin - anchor;    // get change in pivot
            deltaPosition.Scale(rectTransform.rect.size);           // apply sizing
            deltaPosition.Scale(rectTransform.localScale);          // apply scaling
            deltaPosition = rectTransform.rotation * deltaPosition; // apply rotation
     
            rectTransform.anchorMin = anchor;                            // change the pivot
            rectTransform.localPosition -= deltaPosition;           // reverse the position change
        }
    
        public static void ReverseSiblingIndexes(this RectTransform rt)
        {
            var children = new List<Transform>();
            foreach (RectTransform item in rt) children.Add(item);
            foreach (var t in children) t.SetSiblingIndex(0);
        }
    
        public static void ShuffleSiblingIndexes(this RectTransform rt){
        
            var children = new List<Transform>();
            foreach (RectTransform item in rt) children.Add(item);
            var count = children.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i) {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = children[i].GetSiblingIndex();
                children[i].SetSiblingIndex(children[r].GetSiblingIndex());
                children[r].SetSiblingIndex(tmp);
            }
        }
    
        /// <summary>
        /// Converts the anchoredPosition of the first RectTransform to the second RectTransform,
        /// taking into consideration offset, anchors and pivot, and returns the new anchoredPosition
        /// </summary>
        public static Vector2 SwitchToRectTransform(this RectTransform from, RectTransform to)
        {
            Vector2 localPoint;
            var fromPivotDerivedOffset = new Vector2(from.rect.width * from.pivot.x + from.rect.xMin, from.rect.height * from.pivot.y + from.rect.yMin);
            var screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
            screenP += fromPivotDerivedOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
            var pivotDerivedOffset = new Vector2(to.rect.width * to.pivot.x + to.rect.xMin, to.rect.height * to.pivot.y + to.rect.yMin);
            return to.anchoredPosition + localPoint - pivotDerivedOffset;
        }
    
        public static void StretchToDown(this RectTransform rt)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.pivot = new Vector2(0.5f,0.5f);
            rt.anchorMax = new Vector2(1,0);
            rt.anchorMin = new Vector2(0,0);
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;
        }
    
        public static void StretchToRight(this RectTransform rt)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.pivot = new Vector2(0.5f,0.5f);
            rt.anchorMax = new Vector2(1,1);
            rt.anchorMin = new Vector2(1,0);
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;
        }
    
        public static void StretchToLeft(this RectTransform rt)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.pivot = new Vector2(0.5f,0.5f);
            rt.anchorMax = new Vector2(0,1);
            rt.anchorMin = new Vector2(0,0);
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;
        }
    
        public static void StretchToTop(this RectTransform rt)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.pivot = new Vector2(0f, 1f);
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    
        public static void StretchToParent(this RectTransform rt)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        public static void SetAnchorsByType(this RectTransform rt, FreeAnchorsTypes anchorType)
        {
            switch (anchorType)
            {
                case FreeAnchorsTypes.Undefined: return;
            
                case FreeAnchorsTypes.MiddleCenter:
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    return;
            
                case FreeAnchorsTypes.TopCenter:
                    rt.anchorMin = new Vector2(0.5f, 1f);
                    rt.anchorMax = new Vector2(0.5f, 1f);
                    return;
            
                case FreeAnchorsTypes.BottomCenter:
                    rt.anchorMin = new Vector2(0.5f, 0f);
                    rt.anchorMax = new Vector2(0.5f, 0f);
                    return;
            
            
                case FreeAnchorsTypes.TopLeft:
                    rt.anchorMin = new Vector2(0f,1f);
                    rt.anchorMax = new Vector2(0f,1f);
                    return;
            
                case FreeAnchorsTypes.TopRight:
                    rt.anchorMin = new Vector2(1f, 1f);
                    rt.anchorMax = new Vector2(1f, 1f);
                    return;
            
                case FreeAnchorsTypes.MiddleLeft:
                    rt.anchorMin = new Vector2(0f, 0.5f);
                    rt.anchorMax = new Vector2(0f,0.5f);
                    return;
                
                case FreeAnchorsTypes.MiddleRight :
                    rt.anchorMin = new Vector2(1f, 0.5f);
                    rt.anchorMax = new Vector2(1f, 0.5f);
                    return;
                
                case FreeAnchorsTypes.BottomLeft:
                    rt.anchorMin = Vector2.zero;
                    rt.anchorMax = Vector2.zero;
                    return;
                
                case FreeAnchorsTypes.BottomRight:
                    rt.anchorMin = new Vector2(1f, 0f);
                    rt.anchorMax = new Vector2(1f, 0f);
                    return ;
            }
        }
    
        public static FreeAnchorsTypes GetFreeAnchorsType(this RectTransform rt)
        {
            if (rt.anchorMin == new Vector2(0.5f,0.5f) && rt.anchorMax == new Vector2(0.5f,0.5f))
                return FreeAnchorsTypes.MiddleCenter;
            if (rt.anchorMin == new Vector2(0.5f,1f) && rt.anchorMax == new Vector2(0.5f,1f))
                return FreeAnchorsTypes.TopCenter;
            if (rt.anchorMin == new Vector2(0.5f,0f) && rt.anchorMax == new Vector2(0.5f,0f))
                return FreeAnchorsTypes.BottomCenter;
            if (rt.anchorMin == new Vector2(0f,1f) && rt.anchorMax == new Vector2(0f,1f))
                return FreeAnchorsTypes.TopLeft;
            if (rt.anchorMin == new Vector2(1f,1f) && rt.anchorMax == new Vector2(1f,1f))
                return FreeAnchorsTypes.TopRight;
            if (rt.anchorMin == new Vector2(0f,0.5f) && rt.anchorMax == new Vector2(0f,0.5f))
                return FreeAnchorsTypes.MiddleLeft;
            if (rt.anchorMin == new Vector2(1f,0.5f) && rt.anchorMax == new Vector2(1f,0.5f))
                return FreeAnchorsTypes.MiddleRight;
            if (rt.anchorMin == Vector2.zero && rt.anchorMax == Vector2.zero)
                return FreeAnchorsTypes.BottomLeft;
            if (rt.anchorMin == new Vector2(1f,0f) && rt.anchorMax == new Vector2(1f,0f))
                return FreeAnchorsTypes.BottomRight;
        
            return FreeAnchorsTypes.Undefined;
        }
        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }
        public static float GetLeft(this RectTransform rt)
        {
            return rt.offsetMin.x;
        }
    
        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }
        public static float GetRight(this RectTransform rt)
        {
            return rt.offsetMin.y;
        }
 
        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }
 
        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        public static void SetAnchorX(this RectTransform rt, float xPos)
        {
            rt.anchoredPosition = new Vector2(xPos, rt.anchoredPosition.y);
        }
    
        public static void SetAnchorY(this RectTransform rt, float yPos)
        {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, yPos);
        }
    }
}