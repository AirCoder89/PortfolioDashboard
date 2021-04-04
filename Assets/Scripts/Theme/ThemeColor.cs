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
        public ThemeColor normal;
        public ThemeColor highlighted;
        public ThemeColor pressed;
        public ThemeColor disabled;
    }
}