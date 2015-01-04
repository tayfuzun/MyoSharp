using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Commands
{
    /// <summary>
    /// A delegate signature for executing Myo commands.
    /// </summary>
    /// <returns>Returns a <see cref="IMyoCommandResult"/> instance with the result of the command. Cannot be null.</returns>
    /// <remarks>
    /// TODO: Need to wait for CodeContract improvements to be able to handle this:
    /// CodeContracts: Suggested ensures for member Invoke: 
    /// The caller expects the postcondition Contract.Ensures(Contract.Result<MyoSharp.Commands.IMyoCommandResult>() != null); 
    /// to hold for the external member Invoke. Consider adding a postcondition or an assume to document it
    /// </remarks>
    public delegate IMyoCommandResult MyoCommandDelegate();
}
