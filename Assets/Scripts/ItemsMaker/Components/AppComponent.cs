using System;
using UnityEngine;
using UnityEngine.UI;

namespace AirCoder.ItemsMaker.Components
{
    public abstract class AppComponent : MonoBehaviour
    {
        public static event Action<ComponentData> OnUpdate;
        
        public bool Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                gameObject.SetActive(_visibility);
            }
        }

        [SerializeField] protected Text label;
        private bool _isInitialized;
        private bool _visibility;
        protected Inspector.Inspector inspector;
        protected ComponentData _data;
        
        public virtual void Initialize(Inspector.Inspector inInspector)
        {
            if(_isInitialized) return;
            _isInitialized = true;
            inspector = inInspector;
        }

        protected void UpdateData()
        {
            OnUpdate?.Invoke(_data);
        }
        
        public virtual void ResetComponent()
        {
            
        }
        
        public abstract void BindData(ComponentData inData);
    }
}
