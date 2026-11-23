using System.Windows;
using JALV.Common;

namespace JALV
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Apply the saved theme before creating any windows to avoid a bright flash.
            ThemeManager.Initialize();

            int? framerate = FrameRateHelper.DesiredFrameRate;
            BusyIndicatorBehavior.FRAMERATE = framerate;
            FrameRateHelper.SetTimelineDefaultFramerate(framerate);

            (new MainWindow(e.Args)).Show();
        }
    }
}
