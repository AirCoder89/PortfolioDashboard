using System.Collections.Generic;
using Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Theme
{
    public enum ThemeTarget
    {
        Image, Text, Toggle
    }
    
    public class ThemeElement : MonoBehaviour
    {
        [BoxGroup("Main")] public bool isChild;
        [BoxGroup("Target")][EnumToggleButtons][SerializeField] private ThemeTarget target;
        [BoxGroup("Color")][EnumToggleButtons][SerializeField] [HideIf("target", ThemeTarget.Toggle)]
        private ThemeColor color;
        [BoxGroup("Color")][SerializeField] [ShowIf("target", ThemeTarget.Toggle)]
        private ThemeButtonColor colors;

        [BoxGroup("Color")] [SerializeField] [ShowIf("target", ThemeTarget.Toggle)]
        private List<ThemeElement> children;

        private Text _txt;
        private Text _text
        {
            get
            {
                if (_txt == null) _txt = GetComponent<Text>();
                return _txt;
            }
        }

        private Image _img;
        private Image _image
        {
            get
            {
                if (_img == null) _img = GetComponent<Image>();
                return _img;
            }
        }
        
        private Button _btn;
        private Button _button
        {
            get
            {
                if (_btn == null) _btn = GetComponent<Button>();
                return _btn;
            }
        }
        
        private void Awake()
        {
            if (isChild) return;
                ThemeManager.OnThemeChanged += ApplyTheme;
                var toggle = GetComponent<IToggle>();
                if (toggle != null)
                {
                    Debug.Log($"OOOOOOObject is toggle parent");
                    toggle.onValueChanged += isSelected =>
                    {
                        Debug.Log($"Update after value changed");
                        ApplyTheme(ThemeManager.currentTheme);
                    };
                }
        }

        private void OnEnable()
        {
            if(ThemeManager.currentTheme == null) return;
            if(!isChild) ApplyTheme(ThemeManager.currentTheme);
        }

        private void ApplyTheme(ThemeInfo inTheme)
        {
            switch (target)
            {
                case ThemeTarget.Text:
                    this._text.color = inTheme.GetColor(this.color);
                    break;
                case ThemeTarget.Image:
                    this._image.color = inTheme.GetColor(this.color);
                    break;
                case ThemeTarget.Toggle:
                    var isSelected = false;
                    var toggle = GetComponent<IToggle>();
                    if(toggle != null) isSelected = toggle.IsSelected;
                       
                   ApplyTheme(inTheme, isSelected);
                   if(children != null && children.Count > 0)
                       children.ForEach(child =>
                       {
                           child.ApplyTheme(inTheme, isSelected);
                       });
                    break;
            }
        }

        private void ApplyTheme(ThemeInfo inTheme, bool isSelected)
        {
            switch (target)
            {
                case ThemeTarget.Text:
                    this._text.color = inTheme.GetColor(GetColor(isSelected));
                    break;
                case ThemeTarget.Image:
                    this._image.color = inTheme.GetColor(GetColor(isSelected));
                    break;
                case ThemeTarget.Toggle:
                    if (_image != null)
                        this._image.color = inTheme.GetColor(GetColor(isSelected));
                    if(_text != null)
                        this._text.color = inTheme.GetColor(GetColor(isSelected));
                    break;
            }
        }

        private ThemeColor GetColor(bool isSelected) 
            => isSelected ? colors.selected : colors.unselected;

    }
}