using System;

namespace Theme
{
    public enum ThemeColor
    {
        Base, LightBase, HueBase , Light, HighLight, Color
    }

    [Serializable]
    public struct ThemeButtonColor
    {
        public ThemeColor selected;
        public ThemeColor unselected;
    }
}