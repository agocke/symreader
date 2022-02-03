// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

#pragma warning disable 436 // SuppressUnmanagedCodeSecurityAttribute defined in source and mscorlib

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.DiaSymReader;

#if !NET6_0_OR_GREATER
[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("B01FAFEB-C450-3A4D-BEEC-B4CEEC01E006"), SuppressUnmanagedCodeSecurity]
#endif
internal interface ISymUnmanagedDocumentWriter
{
#if NET6_0_OR_GREATER
    public static Guid IID = new Guid("B01FAFEB-C450-3A4D-BEEC-B4CEEC01E006");
#endif
    void SetSource(uint sourceSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] source);
    void SetCheckSum(Guid algorithmId, uint checkSumSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] checkSum);
}

#if NET6_0_OR_GREATER
internal sealed class DocumentWriterWrapperCache : ComWrappers
{
    public static readonly DocumentWriterWrapperCache Instance = new DocumentWriterWrapperCache();
    private DocumentWriterWrapperCache() { }

    private static readonly IntPtr s_vtable;
    private static readonly (IntPtr Entries, int Count) s_definition;

    unsafe static DocumentWriterWrapperCache()
    {
        GetIUnknownImpl(out IntPtr queryIfacePtr, out IntPtr addRefPtr, out IntPtr releasePtr);

        {
            int tableCount = 5;
            int i = 0;
            var vtable = (IntPtr*)RuntimeHelpers.AllocateTypeAssociatedMemory(
                typeof(DocumentWriterRcw),
                IntPtr.Size * tableCount);
            vtable[i++] = queryIfacePtr;
            vtable[i++] = addRefPtr;
            vtable[i++] = releasePtr;
            vtable[i++] = (IntPtr)(delegate* unmanaged<IntPtr, uint, byte*, int>)&DocumentWriterCcw.SetSource;
            vtable[i++] = (IntPtr)(delegate* unmanaged<IntPtr, Guid, uint, byte*, int>)&DocumentWriterCcw.SetCheckSum;
            Debug.Assert(tableCount == i);
            s_vtable = (IntPtr)vtable;
        }

        {
            int definitionLen = 1;
            int i = 0;
            var entries = (ComInterfaceEntry*)RuntimeHelpers.AllocateTypeAssociatedMemory(
                typeof(DocumentWriterRcw),
                sizeof(ComInterfaceEntry) * definitionLen);
            entries[i++] = new ComInterfaceEntry() { IID = IUnsafeComStream.IID, Vtable = s_vtable };
            Debug.Assert(i == definitionLen);
            s_definition = ((IntPtr)entries, definitionLen);
        }
    }

    protected override unsafe ComInterfaceEntry* ComputeVtables(object obj, CreateComInterfaceFlags flags, out int count)
    {
        Debug.Assert(flags == CreateComInterfaceFlags.None);

        if (obj is not DocumentWriterRcw)
        {
            throw new NotSupportedException();
        }
        count = s_definition.Count;
        return (ComInterfaceEntry*)s_definition.Entries;
    }

    protected override DocumentWriterRcw CreateObject(IntPtr externalComObject, CreateObjectFlags flags)
    {
        Debug.Assert(flags == CreateObjectFlags.UniqueInstance);

        return DocumentWriterRcw.Create(externalComObject);
    }

    protected override void ReleaseObjects(IEnumerable objects)
    {
        throw new NotImplementedException();
    }

    private unsafe static class DocumentWriterCcw
    {
        [UnmanagedCallersOnly]
        public static int SetSource(IntPtr @this, uint sourceSize, byte* source)
        {
            try
            {
                var copy = new byte[sourceSize];
                Marshal.Copy((IntPtr)source, copy, 0, (int)sourceSize);
                ComInterfaceDispatch.GetInstance<ISymUnmanagedDocumentWriter>((ComInterfaceDispatch*)@this).SetSource(sourceSize, copy);
            }
            catch (Exception e)
            {
                return e.HResult;
            }
            return HResult.S_OK;
        }

        [UnmanagedCallersOnly]
        public static int SetCheckSum(IntPtr @this, Guid algorithmId, uint checkSumSize, byte* checkSum)
        {
            try
            {
                var copy = new byte[checkSumSize];
                Marshal.Copy((IntPtr)checkSum, copy, 0, (int)checkSumSize);
                ComInterfaceDispatch.GetInstance<ISymUnmanagedDocumentWriter>((ComInterfaceDispatch*)@this).SetCheckSum(algorithmId, checkSumSize, copy);
            }
            catch (Exception e)
            {
                return e.HResult;
            }
            return HResult.S_OK;
        }
    }

    internal sealed class DocumentWriterRcw : ISymUnmanagedDocumentWriter, IDisposable
    {
        private bool _isDisposed = false;
        private readonly IntPtr _inst;
        private DocumentWriterRcw(IntPtr inst)
        {
            _inst = inst;
            Marshal.AddRef(_inst);
        }

        public static DocumentWriterRcw Create(IntPtr ptr)
        {
            var iid = ISymUnmanagedDocumentWriter.IID;
            int hr = Marshal.QueryInterface(ptr, ref iid, out IntPtr wr);
            if (hr != HResult.S_OK)
            {
                throw new NotSupportedException();
            }
            return new DocumentWriterRcw(wr);
        }

        unsafe void ISymUnmanagedDocumentWriter.SetSource(uint sourceSize, byte[] source)
        {
            var ptr = _inst;
            var func = (delegate* unmanaged<IntPtr, uint, byte*, int>)(*(*(void***)ptr + 3));
            fixed (byte* sourcePtr = source)
            {
                int hr = func(ptr, sourceSize, sourcePtr);
                if (hr != HResult.S_OK)
                {
                    Marshal.ThrowExceptionForHR(hr);
                }
            }
        }

        unsafe void ISymUnmanagedDocumentWriter.SetCheckSum(Guid algorithmId, uint checkSumSize, byte[] checkSum)
        {
            var ptr = _inst;
            var func = (delegate* unmanaged<IntPtr, Guid, uint, byte*, int>)(*(*(void***)ptr + 4));
            fixed (byte* checksumPtr = checkSum)
            {
                int hr = func(ptr, algorithmId, checkSumSize, checksumPtr);
                if (hr != HResult.S_OK)
                {
                    Marshal.ThrowExceptionForHR(hr);
                }
            }
        }

        ~DocumentWriterRcw()
        {
            DisposeInternal();
        }

        public void Dispose()
        {
            DisposeInternal();
            GC.SuppressFinalize(this);
        }

        private void DisposeInternal()
        {
            if (_isDisposed)
            {
                return;
            }
            Marshal.Release(_inst);
            _isDisposed = true;
        }
    }
}
#endif
