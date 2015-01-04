using System.Diagnostics.Contracts;

namespace MyoSharp.Commands
{
    public interface IMyoCommand
    {
        #region Methods
        void Execute();
        #endregion
    }
}
