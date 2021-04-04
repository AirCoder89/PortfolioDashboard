using System.Collections.Generic;
using AirCoder.ItemsMaker.Components;
using AirCoder.ItemsMaker.Items;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Views.Blocks.Fields;

namespace AirCoder.ItemsMaker.Inspector
{
    public class Inspector : MonoBehaviour
    {
        [SerializeField][Required] private Transform holder;
        
        private HashSet<AppComponent> _components;
        private ItemsMaker _maker;
        private Item _currentItem;
        
        public void Initialize(ItemsMaker inMaker)
        {
            _maker = inMaker;
            AppComponent.OnUpdate += UpdateItem;
            /* ClearInspector();
             foreach (Transform trComponent in holder)
             {
                 var component = trComponent.gameObject.GetComponent<AppComponent>();
                 if(component == null) continue;
                 _components.Add(component);
                 component.Initialize(this);
             }*/
        }

        private void UpdateItem(ComponentData inUpdatedComponent)
        {
            _currentItem?.UpdateField(inUpdatedComponent);
        }

        public void Inspect(Item inItem)
        {
            _currentItem = inItem;
            ClearInspector();
            _currentItem.GetComponentsData()?.ForEach(data =>
            {
                switch (data.type)
                {
                    case SupportedFieldType.Text:
                        var textComponent = GetAppComponent<TextComponent>(data.type);
                        textComponent.Initialize(this);
                        textComponent.BindData(data);
                        AddAppComponent(textComponent);
                        break;
                }
            });
        }

        private void ClearInspector()
        {
            if (_components == null)
            {
                _components = new HashSet<AppComponent>();
                return;
            }

            _components.ForEach(component =>
            {
                component.ResetComponent();
                _maker.Pool.Despawn(component.transform, _maker.Pool.transform);
            });
            _components.Clear();
        }
        private void AddAppComponent(AppComponent inComponent)
        {
            _components.Add(inComponent);
        }

        private T GetAppComponent<T>(SupportedFieldType inType) where T : AppComponent
        {
            return _maker.Pool.Spawn($"{inType}Component", holder).gameObject.GetComponent<T>();
        }

    }
}