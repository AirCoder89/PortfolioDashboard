using System;
using AirCoder.ItemsMaker.Components;
using Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Blocks.Fields
{
    public class ColorField : BlockField
    {
        [SerializeField][Required] private Image color;
        [SerializeField][Required] private Button button;

        private DBColor _dbColor;
        public override void Initialize()
        {
            base.Initialize();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            Debug.Log($"Open Color : {_dbColor.ToColor()}");
        }
        
        public override void BindData(string inLabel, object inValue, bool isDefault)
        {
            label.text = inLabel;
            _dbColor = isDefault ? new DBColor(Color.white) : (DBColor) inValue;
            if(_dbColor == null) throw new NullReferenceException($"DBColor is null");
            color.color = _dbColor.ToColor();
        }
        
        public override ComponentData GetData()
        {
            return new ComponentData(base.component, _dbColor, label.text, FieldId);
        }
        
        public override string ToJson()
        {
            return $"\"{label.text}\": {_dbColor.ToJson()}";
        }
        
        public override void UpdateField(ComponentData inComponentData)
        {
            _dbColor =  (DBColor) inComponentData.value; 
        }

        public override void ResetField()
        {
            _dbColor = null;
            label.text = "";
        }
    }
}
