namespace VolumeInfo.Native
{
    using System;

    /// <summary>
    /// Utility class providing OS specific functionality.
    /// </summary>
    internal static class Platform
    {
        /// <summary>
        /// Determines whether the operating system is Windows NT or later.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the operating system is Windows NT or later; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsWinNT()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }

        /// <summary>
        /// Determines whether the operating system is a supported version of a Unix based system.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the operating system is a supported version of a Unix based system; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// The method is meant to be compatible with both the .NET CLR and the MONO framework.
        /// Details of how to detect the platform under MONO can be found at
        /// http://www.mono-project.com/docs/faq/technical/#how-to-detect-the-execution-platform
        /// </remarks>
        public static bool IsUnix()
        {
            int platform = (int)Environment.OSVersion.Platform;
            return ((platform == 4) || (platform == 6) || (platform == 128));
        }

        /// <summary>
        /// Determines if the software is running on top of the Mono Common Language Runtime.
        /// </summary>
        /// <returns><see langword="true"/> if running on the Mono CLR; otherwise, <see langword="false"/>.</returns>
        public static bool IsMonoClr()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
