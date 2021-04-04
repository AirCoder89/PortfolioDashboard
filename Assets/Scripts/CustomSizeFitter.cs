using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AirCoder
{
    [RequireComponent(typeof(RectTransform))][ExecuteInEditMode]
    public class CustomSizeFitter : MonoBehaviour
    {
        public string ignoreTag;
        public float extraSpacing;
        public float spacing;
        public List<CustomSizeFitter> allParent;

        private RectTransform _rt;
        private float _startSize;
    
        private RectTransform _rectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }

        private void OnEnable()
        {
            _startSize = _rectTransform.sizeDelta.y;
            UpdateSize();
        }

        public void ResetSize()
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _startSize);
            if (allParent != null && allParent.Count > 0)
            {
                foreach (var parent in allParent)
                {
                    parent.ResetSize();
                }
            }
        }
        
        public void UpdateSize(float delay)
        {
            Invoke(nameof(UpdateSize), delay);
        }

        [Button("Update Size", ButtonSizes.Medium)]
        public void UpdateSize()
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _startSize);
            var totalWidth = 0f;
            if (_rectTransform.childCount > 0)
            {
                foreach (RectTransform child in _rectTransform)
                {
                    if(!child.gameObject.activeSelf) continue;
                    if(!string.IsNullOrEmpty(ignoreTag) && child.gameObject.CompareTag(ignoreTag)) continue;
                    totalWidth += child.rect.height + spacing;
                }
                totalWidth += extraSpacing;
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, totalWidth);
            }
        
            if (allParent != null && allParent.Count > 0)
            {
                foreach (var parent in allParent)
                {
                    parent.UpdateSize();
                }
            }
        }
    }
}