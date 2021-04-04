using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Menu
{
    public struct  MenuEntry<T,TD> where T : RootModel where TD : Model
    {
        public string fileName;
        
    }
    public class Menu : MonoBehaviour
    {
        [SerializeField] private string menuButtonName;
        [SerializeField] private RectTransform buttonsHolder;

        private List<MenuButton> _buttons;
        private MenuButton _selectedBtn;
        private Dashboard _dashboard;
        
        public void Initialize(Dashboard inDashboard)
        {
            _dashboard = inDashboard;
            _selectedBtn = null;
            _buttons = null;
        }

        public void AddEntry<T, TD>(string inFileName, string inLabel) where T : RootModel where TD : Model
        {
            var isFirstEntry = false;
            if (_buttons == null)
            {
                isFirstEntry = true;
                _buttons = new List<MenuButton>();
            }
            var btn = GetMenuButton();
            btn.Initialize(_dashboard, this);
            var fullPath = $"{_dashboard.Settings.jsonPath}/{inFileName}";
            btn.BindData<T,TD>(fullPath, inLabel, isFirstEntry);
            _buttons.Add(btn);
        }

        private MenuButton GetMenuButton()
        {
            return _dashboard.Pool.Spawn(this.menuButtonName, this.buttonsHolder).gameObject
                .GetComponent<MenuButton>();
        }

        public void OnClickButton(MenuButton inButton)
        {
            if(_selectedBtn != null) _selectedBtn.Unselect();
            _selectedBtn = inButton;
            _selectedBtn.Select();
        }
    }
}