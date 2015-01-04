using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Communication;

namespace MyoSharp.Commands
{
    [ContractClass(typeof(IMyoCommandResultContract))]
    public interface IMyoCommandResult
    {
        #region Properties
        MyoResult Result { get; }

        IntPtr ErrorHandle { get; }
        #endregion
    }

    [ContractClassFor(typeof(IMyoCommandResult))]
    internal abstract class IMyoCommandResultContract : IMyoCommandResult
    {
        #region Properties
        public MyoResult Result
        {
            get
            {
                Contract.Ensures((Contract.Result<MyoResult>() == MyoResult.Success && ErrorHandle == IntPtr.Zero) || ErrorHandle != IntPtr.Zero);

                return default(MyoResult);
            }
        }

        public IntPtr ErrorHandle
        {
            get
            {
                Contract.Ensures((Result == MyoResult.Success && Contract.Result<IntPtr>() == IntPtr.Zero) || Contract.Result<IntPtr>() != IntPtr.Zero);

                return default(IntPtr);
            }
        }
        #endregion
    }
}
