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
        
        // Stores the WFP Engine session handle.
        private IntPtr _engineHandle = IntPtr.Zero;

        // Stores whether the WFP Engine session is currently open.
        private bool _isSessionOpen = false;

        // Represents the display information of a WFP object.
        [StructLayout(LayoutKind.Sequential)]
        private struct FWPM_DISPLAY_DATA0
        {
            public IntPtr name;
            public IntPtr description;
        }

        // Represents a WFP Engine session.
        [StructLayout(LayoutKind.Sequential)]
        private struct FWPM_SESSION0
        {
            public Guid sessionKey;
            public FWPM_DISPLAY_DATA0 displayData;
            public uint flags;
            public uint txnWaitTimeoutInMSec;
            public uint processId;
            public IntPtr sid;
            public IntPtr username;
            public int kernelMode;
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


        // Opens a dynamic session with the WFP Engine.
        public string OpenDynamicSession()
        {
            // Checks whether a session is already open.
            if (_isSessionOpen)
            {
                return "WFP session is already open.";
            }

            // Stores the dynamic session configuration.
            FWPM_SESSION0 session = new FWPM_SESSION0();

            // Stores the allocated memory addresses.
            IntPtr sessionNamePointer = IntPtr.Zero;
            IntPtr sessionPointer = IntPtr.Zero;

            try
            {
                // Allocates memory for the session name.
                sessionNamePointer = Marshal.StringToHGlobalUni(
                    "Aegis Test Session");

                session.displayData.name = sessionNamePointer;

                // Enables the dynamic session flag.
                session.flags = 0x00000001;

                // Allocates memory for the native session structure.
                sessionPointer = Marshal.AllocHGlobal(
                    Marshal.SizeOf<FWPM_SESSION0>());

                // Copies the session structure into the allocated memory.
                Marshal.StructureToPtr(session, sessionPointer, false);

                // Opens the WFP Engine with the dynamic session.
                uint openResult = FwpmEngineOpen0(
                    IntPtr.Zero,
                    10,
                    IntPtr.Zero,
                    sessionPointer,
                    out _engineHandle);

                // Checks whether the session was opened successfully.
                if (openResult != 0)
                {
                    _engineHandle = IntPtr.Zero;

                    return $"Failed to open dynamic WFP session. Error: 0x{openResult:X8}";
                }

                _isSessionOpen = true;

                return "Dynamic WFP session opened successfully!";
            }
            finally
            {
                // Releases the allocated session name.
                if (sessionNamePointer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(sessionNamePointer);
                }

                // Releases the allocated session structure.
                if (sessionPointer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(sessionPointer);
                }
            }
        }


        // Closes the current dynamic WFP session.
        public string CloseDynamicSession()
        {
            // Checks whether a session is currently open.
            if (!_isSessionOpen || _engineHandle == IntPtr.Zero)
            {
                return "No active WFP session to close.";
            }

            // Closes the WFP Engine connection.
            uint closeResult = FwpmEngineClose0(_engineHandle);

            // Checks whether the session was closed successfully.
            if (closeResult != 0)
            {
                return $"Failed to close WFP session. Error: 0x{closeResult:X8}";
            }

            // Resets the session information.
            _engineHandle = IntPtr.Zero;
            _isSessionOpen = false;

            return "Dynamic WFP session closed successfully!";
        }

    }
}

