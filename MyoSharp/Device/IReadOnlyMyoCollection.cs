using System.Collections.Generic;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines a collection of Myos.
    /// </summary>
    public interface IReadOnlyMyoCollection : IEnumerable<IMyo>
    {
        #region Properties
        /// <summary>
        /// Gets the number of Myos in the collection.
        /// </summary>
        int Count { get; }
        #endregion
    }
}
