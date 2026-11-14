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

using Aegis_Adblocker.Services;


namespace Aegis_Adblocker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Creates an instance of the WfpService class.
        private readonly WfpService _wfpService = new WfpService();

        public MainWindow()
        {
            InitializeComponent();

            //MessageBox.Show("MainWindow constructor executed!");

            // Run the environment check when the application starts.
            CheckWfpEnvironment();
        }

        // Checks the Windows environment and application permissions.
        private void CheckWfpEnvironment()
        {
            bool isWindowsSupported = _wfpService.IsWindowsSupported();

            string operatingSystemVersion = _wfpService.GetOperatingSystemVersion();

            bool isAdministrator = _wfpService.IsRunningAsAdministrator();

            // Stores the result of the WFP Engine connection test.
            string wfpConnectionResult;

            try
            {
                // Tests the connection to the WFP Engine.
                wfpConnectionResult = _wfpService.TestWfpEngineConnection();
            }
            catch (Exception ex)
            {
                // Handles unexpected errors during the WFP connection test.
                wfpConnectionResult = $"WFP connection error: {ex.Message}";
            }

            // Create a message with the environment information.
            string message =
                $"Windows supported: {isWindowsSupported}\n" +
                $"Operating System: {operatingSystemVersion}\n" +
                $"Administrator privileges: {isAdministrator}\n\n" +
                $"WFP Engine Test:\n{wfpConnectionResult}";

            // Display the results.
            MessageBox.Show(
                message,
                "Aegis - Environment Check",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}