using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace Aegis_Adblocker.Services
{
    // This class will handle Windows Filtering Platform (WFP) operations.
    public class WfpService
    {
        // Constructor of the WfpService class.
        public WfpService()
        {
        }

        // Windows Filtering Platform native functions.
        [DllImport("fwpuclnt.dll", ExactSpelling = true)]
        private static extern uint FwpmEngineOpen0(
            IntPtr serverName,
            uint authnService,
            IntPtr authIdentity,
            IntPtr session,
            out IntPtr engineHandle);

        // Closes the connection to the WFP Engine.
        [DllImport("fwpuclnt.dll", ExactSpelling = true)]
        private static extern uint FwpmEngineClose0(
            IntPtr engineHandle);


        // Checks whether the application is running on Windows.
        public bool IsWindowsSupported()
        {
            return OperatingSystem.IsWindows();
        }


        // Returns the version of the operating system.
        public string GetOperatingSystemVersion()
        {
            return Environment.OSVersion.VersionString;
        }


        // Checks whether the application is running with administrator privileges.
        public bool IsRunningAsAdministrator()
        {
            using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
            {
                var principal = new System.Security.Principal.WindowsPrincipal(identity);

                return principal.IsInRole(
                    System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
        }

        // Tests the connection to the Windows Filtering Platform Engine.
        public string TestWfpEngineConnection()
        {
            // Stores the handle of the WFP Engine session.
            IntPtr engineHandle = IntPtr.Zero;

            // Opens a session with the WFP Engine.
            uint openResult = FwpmEngineOpen0(
                IntPtr.Zero,
                10, // RPC_C_AUTHN_WINNT
                IntPtr.Zero,
                IntPtr.Zero,
                out engineHandle);

            // Checks whether the session was opened successfully.
            if (openResult != 0)
            {
                return $"Failed to open WFP Engine. Error: 0x{openResult:X8}";
            }

            // Closes the WFP Engine session.
            uint closeResult = FwpmEngineClose0(engineHandle);

            // Checks whether the session was closed successfully.
            if (closeResult != 0)
            {
                return $"WFP Engine opened, but closing failed. Error: 0x{closeResult:X8}";
            }

            return "WFP Engine connection successful!";
        }

    }
}

