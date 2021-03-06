using System;
using Interfaces;
using Models;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Button))]
    public class MenuButton : MonoBehaviour, IToggle
    {
        public event Action<bool> onValueChanged;
        public bool IsSelected { get; private set; }
        
        [SerializeField] [Required] private Text label;
        public RootModel rootModel { get; private set; }

        private Button _btn;
        private Button _buttons
        {
            get
            {
                if (_btn == null) _btn = GetComponent<Button>();
                return _btn;
            }
        }

        private bool _isInitialized;
        private Menu _menu;
        private Dashboard _dashboard;
        public void Initialize(Dashboard inDashboard, Menu inMenu)
        {
            if(_isInitialized) return;
            _isInitialized = true;
            IsSelected = false;
            _menu = inMenu;
            _dashboard = inDashboard;
        }
        
        public void BindData<T, TD>(string inPath, string inLabel, bool isSelected = false) where T : RootModel where TD : Model
        {
            void Evaluate()
            {
                _menu.OnClickButton(this);
                _dashboard.EvaluateRoot<TD>(rootModel);
            }

            label.text = inLabel;
            rootModel = LoadRoot<T>(inPath);
            _buttons.onClick.RemoveAllListeners();
            _buttons.onClick.AddListener(Evaluate);
            if (isSelected) Evaluate();
        }

        private T LoadRoot<T>(string filePath) where T : RootModel
        {
            Debug.Log($"Load Root ({typeof(T).Name}) From : {filePath}");
            var targetFile = Resources.Load<TextAsset>(filePath);
            Debug.Log($"targetFile : {targetFile} - value : {targetFile.text}");
            return JsonConvert.DeserializeObject<T>(targetFile.text);
        }

        public void Select()
        {
            Debug.Log($"Select");
            IsSelected = true;
            onValueChanged?.Invoke(IsSelected);
        }

        public void Unselect()
        {
            Debug.Log($"UnSelect");
            IsSelected = false;
            onValueChanged?.Invoke(IsSelected);
        }

       
        public void SetColors(Color selectedColor, Color unselectedColor)
        {
            
        }
    }
}
