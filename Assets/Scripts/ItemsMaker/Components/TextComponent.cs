using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AirCoder.ItemsMaker.Components
{
    public class TextComponent : AppComponent
    {
        [SerializeField][Required] private InputField inputText;

        public override void Initialize(Inspector.Inspector inInspector)
        {
            base.Initialize(inInspector);
            inputText.onEndEdit.AddListener(UpdateText);
        }

        private void UpdateText(string inText)
        {
            _data.value = inText;
            base.UpdateData();
        }

        public override void BindData(ComponentData inData)
        {
            _data = inData;
            label.text = _data.label;
            inputText.text = _data.value as string;
        }
    }
}
