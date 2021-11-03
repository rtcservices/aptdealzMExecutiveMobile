using System;

namespace aptdealzMExecutiveMobile.Theme.Styles
{
    public class ThemeManager
    {
        public static void ChangeTheme(string themeName)
        {
            //App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Clear();
            App.Current.Resources.MergedWith = GetTheme(themeName);

            //bool isLightThemeActivated = false;
            //if (themeName.Equals(Constraints.THEME_LIGHTMODE))
            //    isLightThemeActivated = true;

            // DependencyService.Get<IChangeSatusColor>().ChangeStatusTheme(isLightThemeActivated);
        }

        private static Type GetTheme(string themeName)
        {
            Type theme;
            var type = typeof(ThemeManager);
            if (type != null)
            {
                var uri = $"{type.Assembly.GetName().Name}.Resources.Styles.{themeName}";
                if (!string.IsNullOrEmpty(uri))
                {
                    theme = Type.GetType(uri);
                    if (theme != null)
                        return theme;
                    else
                        return GetDefaultTheme();
                }
                else
                    return GetDefaultTheme();

            }
            else
                return GetDefaultTheme();

        }

        private static Type GetDefaultTheme()
        {
            return new LightModeResources().GetType();
        }
    }
}
