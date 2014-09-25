using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Internal
{
    internal static class PlatformInvocation
    {
        #region Constants
        internal const string MyoDllPath64 = @"x64\myo.dll";

        internal const string MyoDllPath32 = @"x86\myo.dll";

        private static readonly bool IS_RUNNING_32 = IntPtr.Size == 4;
        #endregion

        #region Properties
        internal static bool Running32Bit
        {
            get { return IS_RUNNING_32; }
        }
        #endregion
    }
}