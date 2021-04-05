using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Views.SearchDashboard
{
    public class Searching : MonoBehaviour
    {
        public static event Action<FilterType, string> OnSearch; 
        [SerializeField] [Required] private Toggle onlyEnabledToggle;
        [SerializeField] [Required] private InputField searchTxt;
        [SerializeField] [Required] private RectTransform filtersHolder;
        [SerializeField] private string filterItemName;

        private Dashboard _dashboard;
        private FilterType _filters;
        private List<FilterItem> _items;
        public void Initialize(Dashboard inDashboard)
        {
            onlyEnabledToggle.isOn = false;
            onlyEnabledToggle.onValueChanged.AddListener(OnEnabledToggleChanged);
            searchTxt.onValueChanged.AddListener(Search);
            _dashboard = inDashboard;
        }

        private void OnEnabledToggleChanged(bool inNewValue)
        {
            if (inNewValue) _filters |= FilterType.Enabled;
            else _filters ^= FilterType.Enabled;
            Search(searchTxt.text);
        }

        public void BindData(FilterType inFilters)
        {
            _filters = FilterType.None;
            GenerateFilterItems(inFilters);
        }

        private void Search(string inString)
        {
            OnSearch?.Invoke(_filters, inString);
        }

        private void GenerateFilterItems(FilterType inFilters)
        {
            ClearItems();
            if(_filters == FilterType.None) return;
            var newItem = GetItem();
            newItem.onSelect += OnSelectFilterItem;
            newItem.Initialize();
            
            if(_filters.HasFlag(FilterType.Color)) newItem.BindData(FilterType.Color);
            if(_filters.HasFlag(FilterType.Numeric)) newItem.BindData(FilterType.Numeric);
            if(_filters.HasFlag(FilterType.String)) newItem.BindData(FilterType.String);
            if(_filters.HasFlag(FilterType.Enabled)) newItem.BindData(FilterType.Enabled);
        }

        private void OnSelectFilterItem(FilterItem inFilter)
        {
            if (inFilter.IsSelected)
            {
                _filters ^= inFilter.Type;
                inFilter.Unselect();
            }
            else
            {
                _filters |= inFilter.Type;
                inFilter.Select();
            }
        }

        private FilterItem GetItem()
        {
            return _dashboard.Pool.Spawn(filterItemName, filtersHolder).gameObject
                .GetComponent<FilterItem>();
        }

        private void RemoveItem(Component inItem)
        {
            _dashboard.Pool.Despawn(inItem.transform, _dashboard.Pool.transform);
        }

        private void ClearItems()
        {
            if (_items == null)
            {
                _items = new List<FilterItem>();
                return;
            }

            foreach (var item in _items)
                RemoveItem(item);
            _items.Clear();
        }
        
        public void Clear()
        {
            onlyEnabledToggle.isOn = false;
            _filters = FilterType.None;
            ClearItems();
        }
    }
}
