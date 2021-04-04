using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Theme
{
    public enum ThemeTarget
    {
        Image, Text, Button
    }
    public class ThemeElement : MonoBehaviour
    {
        [BoxGroup("Target")][EnumToggleButtons][SerializeField] private ThemeTarget target;
        [BoxGroup("Color")][EnumToggleButtons][HideIf("target", ThemeTarget.Button)] [SerializeField] 
        private ThemeColor color;
        [BoxGroup("Color")][EnumToggleButtons][ShowIf("target", ThemeTarget.Button)] [SerializeField] 
        private ThemeButtonColor colors;

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
            ThemeManager.OnThemeChanged += ApplyTheme;
        }

        private void OnEnable()
        {
            if(ThemeManager.currentTheme == null) return;
            ApplyTheme(ThemeManager.currentTheme);
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
                case ThemeTarget.Button:
                    _button.colors = new ColorBlock()
                    {
                        normalColor = inTheme.GetColor(colors.normal),
                        highlightedColor = inTheme.GetColor(colors.highlighted),
                        pressedColor = inTheme.GetColor(colors.pressed),
                        disabledColor = inTheme.GetColor(colors.disabled),
                    };
                    break;
            }
        }

    }
}