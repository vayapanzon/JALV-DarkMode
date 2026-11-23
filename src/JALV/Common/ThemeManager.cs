using System;
using System.Windows;
using Microsoft.Win32;

namespace JALV.Common
{
    public enum ThemeMode
    {
        System,
        Light,
        Dark
    }

    /// <summary>
    /// Changes only WPF resource dictionaries; log parsing and window data are untouched.
    /// Compatible with .NET Framework 4.0 and Windows 7 through Windows 11.
    /// </summary>
    public static class ThemeManager
    {
        private static ResourceDictionary darkResources;
        private static ThemeMode selectedTheme = ThemeMode.System;
        private static bool isInitialized;

        public static event EventHandler ThemeChanged;

        public static ThemeMode SelectedTheme
        {
            get { return selectedTheme; }
        }

        public static bool IsDark
        {
            get { return selectedTheme == ThemeMode.Dark ||
                (selectedTheme == ThemeMode.System && IsWindowsDarkMode()); }
        }

        public static void Initialize()
        {
            if (isInitialized)
                return;
            isInitialized = true;
            ThemeMode stored;
            if (!Enum.TryParse<JALV.Common.ThemeMode>(Properties.Settings.Default.ThemeMode, true, out stored) ||
                !Enum.IsDefined(typeof(ThemeMode), stored))
                stored = ThemeMode.System;
            selectedTheme = stored;
            ApplyTheme();

            // Windows theme settings may change while JALV is running.
            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
            if (Application.Current != null)
                Application.Current.Exit += OnApplicationExit;
        }

        public static void SetTheme(ThemeMode theme)
        {
            if (!Enum.IsDefined(typeof(ThemeMode), theme))
                return;
            selectedTheme = theme;
            Properties.Settings.Default.ThemeMode = theme.ToString();
            try
            {
                Properties.Settings.Default.Save();
            }
            catch (System.Configuration.ConfigurationErrorsException)
            {
                // The chosen theme still works for this session if user.config is read-only.
            }
            ApplyTheme();
        }

        private static void ApplyTheme()
        {
            ResourceDictionary resources = Application.Current.Resources;
            if (darkResources != null)
                resources.MergedDictionaries.Remove(darkResources);

            if (IsDark)
            {
                if (darkResources == null)
                    darkResources = new ResourceDictionary
                    {
                        Source = new Uri("/JALV;component/Resources/DarkTheme.xaml", UriKind.Relative)
                    };
                resources.MergedDictionaries.Add(darkResources);
            }

            EventHandler changed = ThemeChanged;
            if (changed != null)
                changed(null, EventArgs.Empty);
        }

        private static bool IsWindowsDarkMode()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("AppsUseLightTheme");
                        if (value is int)
                            return (int)value == 0;
                    }
                }
            }
            catch (Exception)
            {
                // Earlier Windows versions have no system application-theme preference.
            }
            return false;
        }

        private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (selectedTheme != ThemeMode.System)
                return;
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(ApplyTheme));
        }

        private static void OnApplicationExit(object sender, ExitEventArgs e)
        {
            SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
            if (Application.Current != null)
                Application.Current.Exit -= OnApplicationExit;
        }
    }
}
