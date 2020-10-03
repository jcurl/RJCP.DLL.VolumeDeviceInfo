﻿// Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT license. See LICENSE file
// in the project root for full license information.

namespace RJCP.Native.Win32
{
    using System;

    /// <content>Contains the <see cref="FileShare"/> nested type.</content>
    internal partial class Kernel32
    {
        [Flags]
        public enum FileShare : uint
        {
            /// <summary>
            /// Prevents other processes from opening a file or device if they request delete, read, or write access.
            /// </summary>
            None = 0x00000000,

            /// <summary>
            /// Enables subsequent open operations on an object to request read access. Otherwise, other processes
            /// cannot open the object if they request read access. If this flag is not specified, but the object has
            /// been opened for read access, the function fails.
            /// </summary>
            FILE_SHARE_READ = 0x00000001,

            /// <summary>
            /// Enables subsequent open operations on an object to request write access. Otherwise, other processes
            /// cannot open the object if they request write access. If this flag is not specified, but the object has
            /// been opened for write access, the function fails.
            /// </summary>
            FILE_SHARE_WRITE = 0x00000002,

            /// <summary>
            /// Enables subsequent open operations on an object to request delete access. Otherwise, other processes
            /// cannot open the object if they request delete access. If this flag is not specified, but the object has
            /// been opened for delete access, the function fails.
            /// </summary>
            FILE_SHARE_DELETE = 0x00000004,
        }
    }
}
