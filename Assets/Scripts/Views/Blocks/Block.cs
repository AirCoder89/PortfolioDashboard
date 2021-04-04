using System;
using System.Collections.Generic;
using System.Linq;
using AirCoder;
using AirCoder.Extensions;
using AirCoder.ItemsMaker.Components;
using Models;
using PathologicalGames;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Views.Blocks.Fields;

namespace Views.Blocks
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private Text titleTxt;
        [SerializeField] private string prefix = "{";
        [SerializeField] private string suffix = "}";
        [SerializeField] private RectTransform holder;
        
        public string blockName;
        private SpawnPool _pool;
        private Dictionary<string, BlockField> _fields;
        private Model _model;
        private CustomSizeFitter _holderSizeFitter;
        public CustomSizeFitter HolderSizeFitter
        {
            get
            {
                if (_holderSizeFitter == null) _holderSizeFitter = holder.GetComponent<CustomSizeFitter>();
                return _holderSizeFitter;
            }
        }
        public void Initialize(SpawnPool inPool, string inTitle, Model inModel)
        {
            this._pool = inPool;
            _model = inModel;
            titleTxt.text = $"{inTitle} Item";
            ResetItem();
        }

        public T GetModel<T>() where T : Model => _model as T;
        
        public void AddField(string inLabel, object inValue, Type inType)
        {
            BlockField field;
            var isDefault = false;
            if (inType == typeof(string)){ field = GetField(SupportedFieldType.Text);
                isDefault = string.IsNullOrEmpty(inValue.ToString());
            }
            else if (inType == typeof(float)){ field = GetField(SupportedFieldType.Numeric);}
            else if (inType == typeof(bool)) field = GetField(SupportedFieldType.Toggle);
            else if (inType == typeof(DBColor))
            {
                field = GetField(SupportedFieldType.Color);
                isDefault = ((DBColor)inValue) == null;
            }
            else throw new Exception($"Unsupported type [{inType.Name}] !");

            field.Initialize();
            field.BindData(inLabel, inValue,isDefault);
            _fields.Add(field.FieldId, field);
        }

        private BlockField GetField(SupportedFieldType inType)
            => _pool.Spawn($"{inType}Field", holder).gameObject.GetComponent<BlockField>();
        
        public IEnumerable<ComponentData> GetComponentsData()
        {
            if (_fields == null || _fields.Count == 0) return null;
            ComponentData Getter(KeyValuePair<string, BlockField> field) => field.Value.GetData();
            return _fields.Select(Getter);
        }
        
        public void UpdateField(ComponentData inUpdatedComponent)
        {
            if(!_fields.ContainsKey(inUpdatedComponent.fieldId)) return;
            _fields[inUpdatedComponent.fieldId].UpdateField(inUpdatedComponent);
        }
        
        [Button("Update Size")]
        public void UpdateSize()
        {
            holder.SetAnchorsByType(FreeAnchorsTypes.MiddleCenter);
            HolderSizeFitter.ResetSize();
            HolderSizeFitter.UpdateSize();
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
                _pool.Despawn(itemField.Value.transform, _pool.transform);
            }
            _fields.Clear();
            
            HolderSizeFitter.ResetSize();
        }
    }
}