using System;
using System.Collections.Generic;
using System.Linq;
using AirCoder.Extensions;
using AirCoder.ItemsMaker.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Views.Blocks.Fields;

namespace AirCoder.ItemsMaker.Items
{
    public class Item : MonoBehaviour
    {
        public static event Action<Item> OnInspect;

        [SerializeField] private Text titleTxt;
        [SerializeField] private string prefix = "{";
        [SerializeField] private string suffix = "}";
        [SerializeField] private Button button;
        [SerializeField] private RectTransform holder;

        private ItemsMaker _maker;
        private Dictionary<string, BlockField> _fields;

        private CustomSizeFitter _holderSizeFitter;

        public CustomSizeFitter holderSizeFitter
        {
            get
            {
                if (_holderSizeFitter == null) _holderSizeFitter = holder.GetComponent<CustomSizeFitter>();
                return _holderSizeFitter;
            }
        }
        private RectTransform _rt;
        private RectTransform _rectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }
        
        public void Initialize(ItemsMaker inMaker, string inTitle)
        {
            button.onClick.AddListener((() =>
            {
                OnInspect?.Invoke(this);
            }));

            titleTxt.text = $"{inTitle} Item";
            this._maker = inMaker;
            ResetItem();
            /*
            _fields = new Dictionary<string, ItemField>();
            foreach (Transform trField in holder)
            {
                var field = trField.gameObject.GetComponent<ItemField>();
                if(field == null) continue;
                _fields.Add(field.FieldId, field);
                field.Initialize(this);
            }*/
        }

        public void AddField(string inLabel, object inValue, Type inType)
        {
            BlockField field;
            if (inType == typeof(string)) field = GetField(SupportedFieldType.Text);
            else throw new Exception($"Unsupported type [{inType.Name}] !");
            field.Initialize();
            
            _fields.Add(field.FieldId, field);
        }

        private BlockField GetField(SupportedFieldType inType)
            => _maker.Pool.Spawn($"{inType}Field", holder).gameObject.GetComponent<BlockField>();
        
        public IEnumerable<ComponentData> GetComponentsData()
        {
            if (_fields == null || _fields.Count == 0) return null;
            ComponentData Getter(KeyValuePair<string, BlockField> field) => field.Value.GetData();
            return _fields.Select(Getter);
        }
        
        public void Select() =>  button.interactable = false;
        public void Unselect() => button.interactable = true;

        public void UpdateField(ComponentData inUpdatedComponent)
        {
            if(!_fields.ContainsKey(inUpdatedComponent.fieldId)) return;
            _fields[inUpdatedComponent.fieldId].UpdateField(inUpdatedComponent);
        }

        [Button("Update Size")]
        public void UpdateSize()
        {
            holder.SetAnchorsByType(FreeAnchorsTypes.MiddleCenter);
            holderSizeFitter.ResetSize();
            holderSizeFitter.UpdateSize();
            StretchToParent(holder);
        }
        
        public string ToJson()
        {
            var json = prefix + "\n";
            for (var i = 0; i < _fields.Count; i++)
            {
                json += _fields.ElementAt(i).Value.ToJson();
                if (i < (_fields.Count - 1)) json += ",\n";
            }
            json += "\n" + suffix;
            return json;
        }
        
        public void StretchToParent(RectTransform rt)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        public void ResetItem()
        {
            if (_fields == null)
            {
                _fields = new Dictionary<string, BlockField>();
                return;
            }

            foreach (var itemField in _fields)
            {
                itemField.Value.ResetField();
                _maker.Pool.Despawn(itemField.Value.transform, _maker.Pool.transform);
            }
            _fields.Clear();
            
            holderSizeFitter.ResetSize();
        }
    }
}