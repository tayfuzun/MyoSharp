using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Exceptions
{
    [ContractClass(typeof(IMyoErrorHandlerDriverContract))]
    public interface IMyoErrorHandlerDriver
    {
        #region Methods
        string GetErrorString(IntPtr errorHandle);

        void FreeMyoError(IntPtr errorHandle);
        #endregion
    }

    [ContractClassFor(typeof(IMyoErrorHandlerDriver))]
    internal abstract class IMyoErrorHandlerDriverContract : IMyoErrorHandlerDriver
    {
        #region Methods
        public string GetErrorString(IntPtr errorHandle)
        {
            Contract.Requires<ArgumentException>(errorHandle != IntPtr.Zero, "The error handle must be set.");

            return default(string);
        }

        public void FreeMyoError(IntPtr errorHandle)
        {
        }
        #endregion
    }
}
