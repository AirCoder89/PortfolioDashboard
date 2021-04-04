using AirCoder.ItemsMaker.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Blocks.Fields
{
    public class NumericField : BlockField
    {
        [SerializeField][Required] private Slider slider;
        [SerializeField][Required] private InputField numericValue;

        private float _float;

        public override void Initialize()
        {
            base.Initialize();
            slider.onValueChanged.AddListener(UpdateTextValue);
            numericValue.onEndEdit.AddListener(UpdateValueByInput);
        }

        private void UpdateTextValue(float inVal)
        {
            _float = inVal;
            numericValue.text = _float.ToString();
        }

        public override void BindData(string inLabel, object inValue, bool isDefault)
        {
            label.text = inLabel;
            _float = isDefault ? 0f : (float) inValue;
            
            numericValue.text = _float.ToString();
            UpdateSliderValue();
        }

        private void UpdateValueByInput(string arg0)
        {
            float.TryParse(arg0, out _float);
            UpdateSliderValue();
        }
        
        private void UpdateSliderValue()
        {
            slider.minValue = _float - (_float * 10);
            slider.maxValue = _float + (_float * 10);
            slider.value = _float;
        }

        public override ComponentData GetData()
        {
            return new ComponentData(base.component, _float, label.text, FieldId);
        }
        
        public override string ToJson()
        {
            return $"\"{label.text}\": \"{_float}\"";
        }

        public override void UpdateField(ComponentData inComponentData)
        {
            _float =  (float) inComponentData.value; 
        }

        public override void ResetField()
        {
            _float = 0f;
            label.text = "";
        }
    }
}
