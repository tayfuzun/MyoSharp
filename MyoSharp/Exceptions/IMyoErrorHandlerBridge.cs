using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Exceptions
{
    [ContractClass(typeof(IMyoErrorHandlerBridgeContract))]
    public interface IMyoErrorHandlerBridge
    {
        #region Methods
        string LibmyoErrorCstring32(IntPtr errorHandle);

        string LibmyoErrorCstring64(IntPtr errorHandle);

        void LibmyoFreeErrorDetails32(IntPtr errorHandle);

        void LibmyoFreeErrorDetails64(IntPtr errorHandle);
        #endregion
    }

    [ContractClassFor(typeof(IMyoErrorHandlerBridge))]
    internal abstract class IMyoErrorHandlerBridgeContract : IMyoErrorHandlerBridge
    {
        #region Methods
        public string LibmyoErrorCstring32(IntPtr errorHandle)
        {
            Contract.Requires<ArgumentException>(errorHandle != IntPtr.Zero, "The pointer to the error must be set.");
            Contract.Ensures(Contract.Result<string>() != null);

            return default(string);
        }

        public string LibmyoErrorCstring64(IntPtr errorHandle)
        {
            Contract.Requires<ArgumentException>(errorHandle != IntPtr.Zero, "The pointer to the error must be set.");
            Contract.Ensures(Contract.Result<string>() != null);

            return default(string);
        }

        public void LibmyoFreeErrorDetails32(IntPtr errorHandle)
        {
        }

        public void LibmyoFreeErrorDetails64(IntPtr errorHandle)
        {
        }
        #endregion
    }
}
