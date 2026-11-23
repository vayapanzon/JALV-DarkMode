using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JALV.Common
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Menu
    {
        public MainMenu()
        {
            InitializeComponent();
            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
            UpdateThemeSelection();
            Unloaded += (sender, args) => ThemeManager.ThemeChanged -= ThemeManager_ThemeChanged;
        }

        private void ThemeManager_ThemeChanged(object sender, System.EventArgs e)
        {
            UpdateThemeSelection();
        }

        private void UpdateThemeSelection()
        {
            ThemeSystem.IsChecked = ThemeManager.SelectedTheme == ThemeMode.System;
            ThemeLight.IsChecked = ThemeManager.SelectedTheme == ThemeMode.Light;
            ThemeDark.IsChecked = ThemeManager.SelectedTheme == ThemeMode.Dark;
        }

        private void ThemeSystem_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.SetTheme(ThemeMode.System);
        }

        private void ThemeLight_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.SetTheme(ThemeMode.Light);
        }

        private void ThemeDark_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.SetTheme(ThemeMode.Dark);
        }

        public RecentFileList RecentFileList
        {
            get 
            {
                return this.RecentFileListMenu;
            }
        }
    }
}
