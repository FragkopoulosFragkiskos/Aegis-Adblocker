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
            //CheckWfpEnvironment();

            // Runs the dynamic WFP session test.
            //TestDynamicWfpSession();
        }


        // Tests opening and closing a dynamic WFP session.
        private void TestDynamicWfpSession()
        {
            try
            {
                // Opens the dynamic WFP session.
                string openResult = _wfpService.OpenDynamicSession();

                // Checks whether the session was opened successfully.
                if (openResult != "Dynamic WFP session opened successfully!")
                {
                    MessageBox.Show(
                        openResult,
                        "WFP Session Test",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                // Displays the successful opening result.
                MessageBox.Show(
                    openResult,
                    "WFP Session Test",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Attempts to open an already active session.
                string secondOpenResult = _wfpService.OpenDynamicSession();

                // Closes the dynamic WFP session.
                string closeResult = _wfpService.CloseDynamicSession();

                // Attempts to open a new session after closing the previous one.
                string reopenResult = _wfpService.OpenDynamicSession();

                // Closes the newly opened session.
                string secondCloseResult = _wfpService.CloseDynamicSession();

                // Displays all test results.
                string message =
                    $"First opening:\n{openResult}\n\n" +
                    $"Second opening:\n{secondOpenResult}\n\n" +
                    $"Session closing:\n{closeResult}\n\n" +
                    $"Reopening session:\n{reopenResult}\n\n" +
                    $"Second closing:\n{secondCloseResult}";

                MessageBox.Show(
                    message,
                    "WFP Dynamic Session Test",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Handles unexpected errors during the test.
                MessageBox.Show(
                    $"Dynamic WFP session test failed: {ex.Message}",
                    "WFP Session Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
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