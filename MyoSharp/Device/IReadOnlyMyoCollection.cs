using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines a collection of Myos.
    /// </summary>
    [ContractClass(typeof(IReadOnlyMyoCollectionContract))]
    public interface IReadOnlyMyoCollection : IEnumerable<IMyo>
    {
        #region Properties
        /// <summary>
        /// Gets the number of Myos in the collection.
        /// </summary>
        int Count { get; }
        #endregion
    }

    [ContractClassFor(typeof(IReadOnlyMyoCollection))]
    internal abstract class IReadOnlyMyoCollectionContract : IReadOnlyMyoCollection
    {
        #region Properties
        /// <summary>
        /// Gets the number of Myos in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return default(int);
            }
        }
        #endregion

        #region Methods
        public abstract IEnumerator<IMyo> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return default(System.Collections.IEnumerator);
        }
        #endregion
    }
}
