using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

using MahApps.Metro;

namespace Common
{
    public static class ThemeUtil
    {
        private static bool _isDarkTheme;
        public static bool IsDarkTheme => _isDarkTheme;
        public static void LoadTheme()
        {
            List<string> resources = new List<string>();
            _isDarkTheme = false;
            
            //add common metro resources
            resources.Add("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml");
            resources.Add("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml");
            resources.Add("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml");
            // <!--Accent and AppTheme setting-->
            resources.Add("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml");
            //check theme mentioned in command line
            if (Environment.GetCommandLineArgs().Contains("-dark"))
            {
                //if want to load "-dark" specify in command line
                _isDarkTheme = true;
                resources.Add("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseDark.xaml");
                resources.Add(@"resources\SharedStylesMetro.xaml");
                resources.Add(@"resources\SharedImagesDark.xaml");
            }
            else
            { // load "-light" theme by default
                resources.Add("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml");
                resources.Add(@"resources\SharedStylesMetro.xaml");

            }
            //add above resources in application merged dictionary
            resources.ForEach((x) =>
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(x, UriKind.RelativeOrAbsolute) });
            });
            if (_isDarkTheme)
            {
                LoadGrayTheme();
            }
            else
            {
                LoadBlueTheme();
            }
        }

        private static void LoadGrayTheme()
        {
            string name = "SharedAccentGray";
            // add custom accent and theme resource dictionaries to the ThemeManager
            // you should replace MahAppsMetroThemesSample with your application name
            // and correct place where your custom accent lives
            ThemeManager.AddAccent(name, new Uri(@"resources/SharedAccentGray.xaml", UriKind.Relative));

            // get the current app style (theme and accent) from the application
            Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);
            if (theme != null)
            {
                // now change app style to the custom accent and current theme
                ThemeManager.ChangeAppStyle(Application.Current,
                                            ThemeManager.GetAccent(name),
                                            theme.Item1);
            }
        }

        private static void LoadBlueTheme()
        {
            string name = "SharedAccentBlue";
            // add custom accent and theme resource dictionaries to the ThemeManager
            // you should replace MahAppsMetroThemesSample with your application name
            // and correct place where your custom accent lives
            ThemeManager.AddAccent(name, new Uri(@"resources/SharedAccentBlue.xaml", UriKind.Relative));

            // get the current app style (theme and accent) from the application
            Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);
            if (theme != null)
            {
                // now change app style to the custom accent and current theme
                ThemeManager.ChangeAppStyle(Application.Current,
                                            ThemeManager.GetAccent(name),
                                            theme.Item1);
            }
        }
    }
}
