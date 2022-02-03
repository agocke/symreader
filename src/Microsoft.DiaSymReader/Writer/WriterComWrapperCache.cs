
#if NET6_0_OR_GREATER

#nullable enable

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader;

internal sealed class WriterComWrapperCache : ComWrappers
{
    protected override unsafe ComInterfaceEntry* ComputeVtables(object obj, CreateComInterfaceFlags flags, out int count)
    {
        throw new NotImplementedException();
    }

    protected override object CreateObject(IntPtr externalComObject, CreateObjectFlags flags)
    {
        Debug.Assert(flags == CreateObjectFlags.UniqueInstance);

        return WriterWrapper.CreateIfSupported(externalComObject) ?? throw new NotSupportedException();
    }

    protected override void ReleaseObjects(IEnumerable objects)
    {
        throw new NotImplementedException();
    }
}

internal class WriterWrapper : IDynamicInterfaceCastable, IDisposable
{
    private bool _isDisposed = false;

    public readonly IntPtr WriterInst;
    public readonly IntPtr CompilerInfoWriterInst;
    public readonly IntPtr PdbWriterInst;

    private WriterWrapper(IntPtr writerInst, IntPtr compilerInfoWriterInst, IntPtr pdbWriterInst)
    {
        WriterInst = writerInst;
        CompilerInfoWriterInst = compilerInfoWriterInst;
        PdbWriterInst = pdbWriterInst;
    }

    public static WriterWrapper? CreateIfSupported(IntPtr ptr)
    {
        var iid = ICompilerInfoWriterImpl.IID;
        int hr = Marshal.QueryInterface(ptr, ref iid, out IntPtr compilerInfoWriterPtr);
        if (hr != HResult.S_OK)
        {
            return null;
        }

        iid = IPdbWriterImpl.IID;
        hr = Marshal.QueryInterface(ptr, ref iid, out IntPtr pdbWriterPtr);
        if (hr != HResult.S_OK)
        {
            Marshal.Release(compilerInfoWriterPtr);
            return null;
        }

        // Try to get ISymUnmanagedWriter8 first, ISymUnmanagedWriter5 only if it's not found
        iid = ISymWriter8Impl.IID;
        hr = Marshal.QueryInterface(ptr, ref iid, out IntPtr unmanagedWriterPtr);
        if (hr != HResult.S_OK)
        {
            iid = ISymWriter5Impl.IID;
            hr = Marshal.QueryInterface(ptr, ref iid, out unmanagedWriterPtr);
        }

        if (hr == HResult.S_OK)
        {
            return new WriterWrapper(unmanagedWriterPtr, compilerInfoWriterPtr, pdbWriterPtr);
        }
        Marshal.Release(compilerInfoWriterPtr);
        Marshal.Release(pdbWriterPtr);
        return null;
    }

    public RuntimeTypeHandle GetInterfaceImplementation(RuntimeTypeHandle interfaceType)
    {
        if (interfaceType.Equals(typeof(ISymUnmanagedWriter5).TypeHandle))
        {
            return typeof(ISymWriter5Impl).TypeHandle;
        }
        if (interfaceType.Equals(typeof(ISymUnmanagedWriter8).TypeHandle))
        {
            return typeof(ISymWriter8Impl).TypeHandle;
        }
        if (interfaceType.Equals(typeof(ISymUnmanagedCompilerInfoWriter).TypeHandle))
        {
            return typeof(ICompilerInfoWriterImpl).TypeHandle;
        }
        if (interfaceType.Equals(typeof(IPdbWriter).TypeHandle))
        {
            return typeof(IPdbWriterImpl).TypeHandle;
        }
        return default;
    }

    public bool IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented)
    {
        if (interfaceType.Equals(typeof(ISymUnmanagedWriter8).TypeHandle) ||
            interfaceType.Equals(typeof(ISymUnmanagedWriter5).TypeHandle) ||
            interfaceType.Equals(typeof(ISymUnmanagedCompilerInfoWriter).TypeHandle) ||
            interfaceType.Equals(typeof(IPdbWriter).TypeHandle))
        {
            return true;
        }

        if (throwIfNotImplemented)
        {
            throw new InvalidCastException($"{nameof(WriterWrapper)} does not implement {interfaceType}");
        }
        return false;
    }

    public void Dispose()
    {
        DisposeInternal();
        GC.SuppressFinalize(this);
    }

    ~WriterWrapper()
    {
        DisposeInternal();
    }

    private void DisposeInternal()
    {
        if (_isDisposed)
            return;

        //Marshal.Release(CompilerInfoWriterInst);
        //Marshal.Release(PdbWriterInst);
        Marshal.Release(WriterInst);
        _isDisposed = true;
    }
}

#endif