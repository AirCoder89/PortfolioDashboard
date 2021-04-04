using AirCoder.ItemsMaker.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Blocks.Fields
{
    public abstract class BlockField : MonoBehaviour
    {
        public SupportedFieldType component;
        [SerializeField][Required] protected Text label; 

        private string _id;
        public string FieldId
        {
            get
            {
                if (string.IsNullOrEmpty(_id)) _id = $"Field[{GetInstanceID()}]";
                return _id;
            }
        }

        private bool _isInitialized;
        
        public virtual void Initialize()
        {
            if(_isInitialized) return;
            _isInitialized = true;
            ResetField();
        }
        
        public abstract void BindData(string inLabel, object inValue, bool isDefault);
        public abstract ComponentData GetData();
        public abstract string ToJson();

        public abstract void UpdateField(ComponentData inComponentData);

        public abstract void ResetField();
    }
}