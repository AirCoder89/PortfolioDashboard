using AirCoder.ItemsMaker.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Blocks.Fields
{
    public class ToggleField : BlockField
    {
        [SerializeField][Required] private Toggle toggle;

        public override void BindData(string inLabel, object inValue, bool isDefault)
        {
            label.text = inLabel;
            toggle.isOn = !isDefault && (bool) inValue;
        }

        public override ComponentData GetData()
        {
            return new ComponentData(base.component, toggle.isOn, label.text, FieldId);
        }
        
        public override string ToJson()
        {
            return $"\"{label.text}\": \"{toggle.isOn.ToString()}\"";
        }

        public override void UpdateField(ComponentData inComponentData)
        {
            toggle.isOn =  (bool) inComponentData.value; 
        }

        public override void ResetField()
        {
            toggle.isOn = false;
            label.text = "";
        }
    }
}
