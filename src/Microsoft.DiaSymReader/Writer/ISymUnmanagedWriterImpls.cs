
#if NET6_0_OR_GREATER

#nullable enable

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader;

[DynamicInterfaceCastableImplementation]
internal interface IPdbWriterImpl : IPdbWriter
{
    public static readonly Guid IID = new Guid("98ECEE1E-752D-11d3-8D56-00C04F680B2B");

    int IPdbWriter.__SetPath(/*[in] const WCHAR* szFullPathName, [in] IStream* pIStream, [in] BOOL fFullBuild*/)
        => throw new NotImplementedException();
    int IPdbWriter.__OpenMod(/*[in] const WCHAR* szModuleName, [in] const WCHAR* szFileName*/)
        => throw new NotImplementedException();
    int IPdbWriter.__CloseMod()
        => throw new NotImplementedException();
    int IPdbWriter.__GetPath(/*[in] DWORD ccData,[out] DWORD* pccData,[out, size_is(ccData),length_is(*pccData)] WCHAR szPath[]*/)
        => throw new NotImplementedException();

    unsafe void IPdbWriter.GetSignatureAge(out uint sig, out int age)
    {
        var inst = ((WriterWrapper)this).PdbWriterInst;
        var func = (delegate* unmanaged<IntPtr, uint*, int*, int>)(*(*(void***)inst + 7));
        fixed (uint* sigPtr = &sig)
        fixed (int* agePtr = &age)
        {
            int hr = func(inst, sigPtr, agePtr);
            if (hr != HResult.S_OK)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }
    }
}

[DynamicInterfaceCastableImplementation]
internal unsafe interface ISymWriter5Impl : ISymUnmanagedWriter5
{
    public static readonly Guid IID = new Guid("DCF7780D-BDE9-45DF-ACFE-21731A32000C");

    protected static IntPtr GetInst(ISymWriter5Impl self) => ((WriterWrapper)self).WriterInst;

    ISymUnmanagedDocumentWriter ISymUnmanagedWriter5.DefineDocument(string url, ref Guid language, ref Guid languageVendor, ref Guid documentType)
    {
        var inst = GetInst(this);
        IntPtr docWriterPtr;
        var func = (delegate* unmanaged<IntPtr, char*, Guid*, Guid*, Guid*, IntPtr*, int>)(*(*(void***)inst + 3));
        fixed (char* urlPtr = url)
        fixed (Guid* languagePtr = &language)
        fixed (Guid* languageVendorPtr = &languageVendor)
        fixed (Guid* documentTypePtr = &documentType)
        {
            int hr = func(inst, urlPtr, languagePtr, languageVendorPtr, documentTypePtr, &docWriterPtr);
            Marshal.AddRef(docWriterPtr);
            if (hr != HResult.S_OK)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
            var cw = DocumentWriterWrapperCache.Instance;
            return (ISymUnmanagedDocumentWriter)cw.GetOrCreateObjectForComInstance(docWriterPtr, CreateObjectFlags.UniqueInstance);
        }
    }
    void ISymUnmanagedWriter5.SetUserEntryPoint(int entryMethodToken)
    {
        var inst = GetInst(this);
        var func = (delegate* unmanaged<IntPtr, int, int>)(*(*(void***)inst + 4));
        int hr = func(inst, entryMethodToken);
        if (hr != HResult.S_OK)
        {
            Marshal.ThrowExceptionForHR(hr);
        }
    }
    void ISymUnmanagedWriter5.OpenMethod(uint methodToken)
    {
        var inst = GetInst(this);
        var func = (delegate* unmanaged<IntPtr, uint, int>)(*(*(void***)inst + 5));
        int hr = func(inst, methodToken);
        if (hr != HResult.S_OK)
        {
            Marshal.ThrowExceptionForHR(hr);
        }
    }
    void ISymUnmanagedWriter5.CloseMethod()
    {
        var inst = GetInst(this);
        var func = (delegate* unmanaged<IntPtr, int>)(*(*(void***)inst + 6));
        int hr = func(inst);
        if (hr != HResult.S_OK)
        {
            Marshal.ThrowExceptionForHR(hr);
        }
    }
    uint ISymUnmanagedWriter5.OpenScope(int startOffset)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.CloseScope(int endOffset)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.SetScopeRange(uint scopeID, uint startOffset, uint endOffset)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.DefineLocalVariable(string name, uint attributes, uint sig, byte* signature, uint addrKind, uint addr1, uint addr2, uint startOffset, uint endOffset)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.DefineParameter(string name, uint attributes, uint sequence, uint addrKind, uint addr1, uint addr2, uint addr3)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.DefineField(uint parent, string name, uint attributes, uint sig, byte* signature, uint addrKind, uint addr1, uint addr2, uint addr3)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.DefineGlobalVariable(string name, uint attributes, uint sig, byte* signature, uint addrKind, uint addr1, uint addr2, uint addr3)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.Close()
    {
        var inst = GetInst(this);
        var func = (delegate* unmanaged<IntPtr, int>)(*(*(void***)inst + 14));
        int hr = func(inst);
        if (hr != HResult.S_OK)
        {
            Marshal.ThrowExceptionForHR(hr);
        }
    }
    void ISymUnmanagedWriter5.SetSymAttribute(uint parent, string name, int length, byte* data)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.OpenNamespace(string name)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.CloseNamespace()
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.UsingNamespace(string fullName)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.SetMethodSourceRange(ISymUnmanagedDocumentWriter startDoc, uint startLine, uint startColumn, object endDoc, uint endLine, uint endColumn)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.Initialize([MarshalAs(UnmanagedType.IUnknown)] object emitter, string filename, [MarshalAs(UnmanagedType.IUnknown)] object ptrIStream, bool fullBuild)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.GetDebugInfo(ref ImageDebugDirectory debugDirectory, uint dataCount, out uint dataCountPtr, byte* data)
    {
        var inst = GetInst(this);
        var func = (delegate* unmanaged<IntPtr, ImageDebugDirectory*, uint, uint*, byte*, int>)(*(*(void***)inst + 21));
        fixed (ImageDebugDirectory* debugDir = &debugDirectory)
        fixed (uint* dataCountPtrPtr = &dataCountPtr)
        {
            int hr = func(inst, debugDir, dataCount, dataCountPtrPtr, data);
            if (hr != HResult.S_OK)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }
    }
    void ISymUnmanagedWriter5.DefineSequencePoints(ISymUnmanagedDocumentWriter document, int count, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
    {
        var inst = GetInst(this);
        var func = (delegate* unmanaged<IntPtr, IntPtr, int, int*, int*, int*, int*, int*, int>)(*(*(void***)inst + 22));
        var docPtr = DocumentWriterWrapperCache.Instance.GetOrCreateComInterfaceForObject(document, CreateComInterfaceFlags.None);
        Marshal.AddRef(docPtr);
        fixed (int* offsetsPtr = offsets)
        fixed (int* linesPtr = lines)
        fixed (int* columnsPtr = columns)
        fixed (int* endLinesPtr = endLines)
        fixed (int* endColumnsPtr = endColumns)
        {
            int hr = func(inst, docPtr, count, offsetsPtr, linesPtr, columnsPtr, endLinesPtr, endColumnsPtr);
            if (hr != HResult.S_OK)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }
    }
    void ISymUnmanagedWriter5.RemapToken(uint oldToken, uint newToken)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.Initialize2([MarshalAs(UnmanagedType.IUnknown)] object emitter, string tempfilename, [MarshalAs(UnmanagedType.IUnknown)] object ptrIStream, bool fullBuild, string finalfilename)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.DefineConstant(string name, object value, uint sig, byte* signature)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.Abort()
        => throw new NotImplementedException();

    void ISymUnmanagedWriter5.DefineLocalVariable2(string name, int attributes, int localSignatureToken, uint addrKind, int index, uint addr2, uint addr3, uint startOffset, uint endOffset)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.DefineGlobalVariable2(string name, int attributes, int sigToken, uint addrKind, uint addr1, uint addr2, uint addr3)
        => throw new NotImplementedException();

    void ISymUnmanagedWriter5.DefineConstant2([MarshalAs(UnmanagedType.LPWStr)] string name, VariantStructure value, int constantSignatureToken)
        => throw new NotImplementedException();

    void ISymUnmanagedWriter5.OpenMethod2(uint methodToken, int sectionIndex, int offsetRelativeOffset)
        => throw new NotImplementedException();
    void ISymUnmanagedWriter5.Commit()
        => throw new NotImplementedException();

    void ISymUnmanagedWriter5.GetDebugInfoWithPadding(ref ImageDebugDirectory debugDirectory, uint dataCount, out uint dataCountPtr, byte* data)
        => throw new NotImplementedException();

    void ISymUnmanagedWriter5.OpenMapTokensToSourceSpans()
        => throw new NotImplementedException();

    void ISymUnmanagedWriter5.CloseMapTokensToSourceSpans()
        => throw new NotImplementedException();

    void ISymUnmanagedWriter5.MapTokenToSourceSpan(int token, ISymUnmanagedDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
        => throw new NotImplementedException();
}

[DynamicInterfaceCastableImplementation]
#pragma warning disable CA2256 // Ignore warning about not re-implementing ISymWriter5Impl
internal unsafe interface ISymWriter8Impl : ISymWriter5Impl, ISymUnmanagedWriter8
#pragma warning restore CA2256
{
    public static new readonly Guid IID = new Guid("5ba52f3b-6bf8-40fc-b476-d39c529b331e");

    // ISymUnmanagedWriter6
    void ISymUnmanagedWriter8.InitializeDeterministic(object emitter, object stream)
    {
        var inst = GetInst(this);
        var func = (delegate* unmanaged<IntPtr, IntPtr, IntPtr, int>)(*(*(void***)inst + 36));
        var emitterPtr = Marshal.GetIUnknownForObject(emitter);
        var streamAsIUkPtr = ComMemoryStreamWrapperCache.Instance.GetOrCreateComInterfaceForObject(stream, CreateComInterfaceFlags.None);
        Marshal.AddRef(streamAsIUkPtr);
        var iid = IUnsafeComStream.IID;
        if (Marshal.QueryInterface(streamAsIUkPtr, ref iid, out IntPtr streamPtr) != HResult.S_OK)
        {
            throw new ArgumentException("Stream parameter must implement IStream", nameof(stream));
        }
        int hr = func(inst, emitterPtr, streamPtr);
        Marshal.Release(emitterPtr);
        if (hr != HResult.S_OK)
        {
            Marshal.ThrowExceptionForHR(hr);
        }
    }

    // ISymUnmanagedWriter7
    unsafe void ISymUnmanagedWriter8.UpdateSignatureByHashingContent([In] byte* buffer, int size)
        => throw new NotImplementedException();

    // ISymUnmanagedWriter8
    void ISymUnmanagedWriter8.UpdateSignature(Guid pdbId, uint stamp, int age)
    {
        var inst = GetInst(this);
        var func = (delegate* unmanaged<IntPtr, Guid, uint, int, int>)(*(*(void***)inst + 38));
        int hr = func(inst, pdbId, stamp, age);
        if (hr != HResult.S_OK)
        {
            Marshal.ThrowExceptionForHR(hr);
        }
    }
    unsafe void ISymUnmanagedWriter8.SetSourceServerData([In] byte* data, int size)
        => throw new NotImplementedException();
    unsafe void ISymUnmanagedWriter8.SetSourceLinkData([In] byte* data, int size)
        => throw new NotImplementedException();
}


#endif