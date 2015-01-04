using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Communication;

namespace MyoSharp.Commands
{
    public class MyoCommandResult : IMyoCommandResult
    {
        #region Fields
        private readonly MyoResult _result;
        private readonly IntPtr _errorHandle;
        #endregion

        #region Constructors
        private MyoCommandResult(MyoResult result, IntPtr errorHandle)
        {
            Contract.Requires<ArgumentException>(
                (result == MyoResult.Success && errorHandle == IntPtr.Zero) || errorHandle != IntPtr.Zero,
                "The result cannot be successful and have an error handle set.");

            _result = result;
            _errorHandle = errorHandle;
        }
        #endregion

        #region Properties
        /// <inheritdoc />
        public MyoResult Result 
        {
            get
            {
                return _result;
            }
        }

        /// <inheritdoc />
        public IntPtr ErrorHandle
        {
            get
            {
                return _errorHandle;
            }
        }
        #endregion

        #region Methods
        public static IMyoCommandResult Create(MyoResult result, IntPtr errorHandle)
        {
            Contract.Requires<ArgumentException>(
                (result == MyoResult.Success && errorHandle == IntPtr.Zero) || errorHandle != IntPtr.Zero,
                "The result cannot be successful and have an error handle set.");
            Contract.Ensures(Contract.Result<IMyoCommandResult>() != null);

            return new MyoCommandResult(result, errorHandle);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant((_result == MyoResult.Success && _errorHandle == IntPtr.Zero) || _errorHandle != IntPtr.Zero);
            Contract.Invariant((Result == MyoResult.Success && ErrorHandle == IntPtr.Zero) || ErrorHandle != IntPtr.Zero);
        }
        #endregion
    }
}
