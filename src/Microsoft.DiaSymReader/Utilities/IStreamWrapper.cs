// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

#if NET6_0_OR_GREATER

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader;

internal class IStreamWrapperCache : ComWrappers
{
    protected override unsafe ComInterfaceEntry* ComputeVtables(object obj, CreateComInterfaceFlags flags, out int count)
    {
        throw new NotImplementedException();
    }

    protected override object CreateObject(IntPtr externalComObject, CreateObjectFlags flags)
    {
        throw new NotImplementedException();
    }

    protected override void ReleaseObjects(IEnumerable objects)
    {
        throw new NotImplementedException();
    }
}

#endif
