using Views.Blocks.Fields;

namespace AirCoder.ItemsMaker.Components
{
    [System.Serializable]
    public struct ComponentData
    {
        public SupportedFieldType type;
        public object value;
        public string fieldId;
        public string label;

        public ComponentData(SupportedFieldType inType, object inValue,string inLabel, string inId)
        {
            type = inType;
            value = inValue;
            fieldId = inId;
            label = inLabel;
        }
    }
}