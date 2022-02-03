// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
#if !NET6_0_OR_GREATER
    [ComImport]
    [Guid("2ae6a06a-92ba-4c2d-a64e-7e9fa421a330")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
#endif
    public interface ISymUnmanagedCompilerInfoWriter
    {
        /// <summary>
        /// Adds compiler version number and name.
        /// </summary>
        [PreserveSig]
        int AddCompilerInfo(ushort major, ushort minor, ushort build, ushort revision, [MarshalAs(UnmanagedType.LPWStr)] string name);
    }

#if NET6_0_OR_GREATER
    [DynamicInterfaceCastableImplementation]
    interface ICompilerInfoWriterImpl : ISymUnmanagedCompilerInfoWriter
    {
        public static readonly Guid IID = new Guid("2ae6a06a-92ba-4c2d-a64e-7e9fa421a330");

        /// <summary>
        /// Adds compiler version number and name.
        /// </summary>
        unsafe int ISymUnmanagedCompilerInfoWriter.AddCompilerInfo(ushort major, ushort minor, ushort build, ushort revision, string name)
        {
            var inst = ((WriterWrapper)this).CompilerInfoWriterInst;
            var func = (delegate* unmanaged<IntPtr, ushort, ushort, ushort, ushort, char*, int>)(*(*(void***)inst + 3));
            fixed (char* namePtr = name)
            {
                int hr = func(inst, major, minor, build, revision, namePtr);
                return hr;
            }
        }
    }
#endif
}