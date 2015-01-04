using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Communication;
using MyoSharp.Exceptions;
using MyoSharp.Internal;

namespace MyoSharp.Commands
{
    public class MyoCommand : IMyoCommand
    {
        #region Fields
        private readonly MyoCommandDelegate _command;
        private readonly IMyoErrorHandlerDriver _myoErrorHandlerDriver;
        #endregion

        #region Constructors
        private MyoCommand(IMyoErrorHandlerDriver myoErrorHandlerDriver, MyoCommandDelegate command)
        {
            Contract.Requires<ArgumentNullException>(myoErrorHandlerDriver != null, "myoErrorHandlerDriver");
            Contract.Requires<ArgumentNullException>(command != null, "command");

            _myoErrorHandlerDriver = myoErrorHandlerDriver;
            _command = command;
        }
        #endregion

        #region Methods
        public static IMyoCommand Create(IMyoErrorHandlerDriver myoErrorHandlerDriver, MyoCommandDelegate command)
        {
            Contract.Requires<ArgumentNullException>(myoErrorHandlerDriver != null, "myoErrorHandlerDriver");
            Contract.Requires<ArgumentNullException>(command != null, "command");
            Contract.Ensures(Contract.Result<IMyoCommand>() != null);

            return new MyoCommand(myoErrorHandlerDriver, command);
        }

        public void Execute()
        {
            IMyoCommandResult result = null;
            try
            {
                result = _command();
                if (result == null)
                {
                    // TODO: replace all of this by contracts once supported.
                    throw new NullReferenceException("The result of a MyoCommandDelegate cannot be null.");
                }

                if (result.Result == MyoResult.Success)
                {
                    return;
                }

                throw CreateMyoException(result);
            }
            finally
            {
                if (result != null)
                {
                    _myoErrorHandlerDriver.FreeMyoError(result.ErrorHandle);
                }
            }
        }

        private Exception CreateMyoException(IMyoCommandResult myoCommandResult)
        {
            Contract.Requires<ArgumentNullException>(myoCommandResult != null, "myoCommandResult");
            Contract.Requires<ArgumentException>(myoCommandResult.Result != MyoResult.Success, "The result code must not be MyoResult.Success.");

            var errorMessage = _myoErrorHandlerDriver.GetErrorString(myoCommandResult.ErrorHandle);
            
            return myoCommandResult.Result == MyoResult.ErrorInvalidArgument
                ? (Exception)new ArgumentException(errorMessage)
                : (Exception)new InvalidOperationException(errorMessage);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_command != null);
            Contract.Invariant(_myoErrorHandlerDriver != null);
        }
        #endregion
    }
}
