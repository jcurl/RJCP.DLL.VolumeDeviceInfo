﻿namespace RJCP.Native.Win32
{
    internal partial class WinIoCtl
    {
        public enum STORAGE_QUERY_TYPE
        {
            /// <summary>
            /// Instructs the driver to return an appropriate descriptor.
            /// </summary>
            PropertyStandardQuery = 0,

            /// <summary>
            /// Instructs the driver to report whether the descriptor is supported.
            /// </summary>
            PropertyExistsQuery = 1,

            /// <summary>
            /// Used to retrieve a mask of writable fields in the descriptor. Not currently supported. Do not use.
            /// </summary>
            PropertyMaskQuery = 2,

            /// <summary>
            /// Specifies the upper limit of the list of query types. This is used to validate the query type.
            /// </summary>
            PropertyQueryMaxDefined = 3
        }
    }
}
