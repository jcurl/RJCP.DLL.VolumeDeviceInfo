// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// Some APIs copyright from https://www.pinvoke.net.

// This is not a direct copy, as this project doesn't reconstruct the build system used in the
// https://github.com/dotnet/pinvoke.git project.

namespace VolumeInfo.Native.Win32
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    internal static partial class Kernel32
    {
        /// <summary>
        /// Constant for invalid handle value.
        /// </summary>
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hObject">A valid handle to an open object.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call <see cref="GetLastError"/>.
        /// </returns>
        [SuppressUnmanagedCodeSecurity]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeObjectHandle CreateFile(string fileName, ACCESS_MASK access, FileShare share,
            IntPtr securityAttributes, CreationDisposition creationDisposition, CreateFileFlags flagsAndAttributes,
            SafeObjectHandle templateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint QueryDosDevice(string deviceName, StringBuilder targetPath, int bufferLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FileAttributes GetFileAttributes(string fileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetVolumePathName(string fileName, StringBuilder volumePathName, int bufferLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetVolumeNameForVolumeMountPoint(string volumeMountPoint, StringBuilder volumeName, int bufferLength);
    }
}
