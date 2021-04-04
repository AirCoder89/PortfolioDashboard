using AirCoder.ItemsMaker.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Blocks.Fields
{
    public class TextField : BlockField
    {
        [SerializeField][Required] private InputField textField;
        [SerializeField][Required] private Button expandBtn;

        public override void Initialize()
        {
            base.Initialize();
            expandBtn.onClick.AddListener(ExpandText);
        }

        private void ExpandText()
        {
            Debug.Log($"Expand Text");
        }

        public override void BindData(string inLabel, object inValue, bool isDefault)
        {
            label.text = inLabel;
            textField.text = isDefault ? string.Empty : inValue as string;
        }

        public override ComponentData GetData()
        {
            return new ComponentData(base.component, textField.text, label.text, FieldId);
        }
        
        public override string ToJson()
        {
            return $"\"{label.text}\": \"{textField.text}\"";
        }

        public override void UpdateField(ComponentData inComponentData)
        {
            textField.text = inComponentData.value as string;
        }

        public override void ResetField()
        {
            textField.text = "";
            label.text = "";
        }
    }
}