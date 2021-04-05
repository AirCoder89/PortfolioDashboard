using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views.SearchDashboard
{
    [RequireComponent(typeof(Button))]
    public class FilterItem : MonoBehaviour
    {
        public event Action<FilterItem> onSelect; 
        private bool _isInitialized;
        private FilterType _filterType;
        public FilterType Type => _filterType;
        private Button _btn;
        private Button _button
        {
            get
            {
                if (_btn == null) _btn = GetComponent<Button>();
                return _btn;
            }
        }
        public bool IsSelected { get; private set; }
        public void Initialize()
        {
            if(_isInitialized) return;
            _isInitialized = true;
            IsSelected = false;
            _button.onClick.AddListener((() =>
            {
                onSelect?.Invoke(this);
            }));
        }

        public void BindData(FilterType inFilter)
        {
            _filterType = inFilter;
            Unselect();
        }

        public void Select()
        {
            IsSelected = true;
        }

        public void Unselect()
        {
            IsSelected = false;
        }
    }
}