using System;

namespace Theme
{
    public class ThemeManager
    {
        public static event Action<ThemeInfo> OnThemeChanged;
        
        public static ThemeInfo currentTheme { get; private set; }
        public ThemeManager(ThemeInfo inTheme)
        {
            ChangeTheme(inTheme);
        }

        public void ChangeTheme(ThemeInfo inTheme)
        {
            currentTheme = inTheme;
            OnThemeChanged?.Invoke(currentTheme);
        }
    }
}