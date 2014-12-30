using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines functionality for working with EMG data.
    /// </summary>
    [ContractClass(typeof(IEmgDataContract))]
    public interface IEmgData
    {
        #region Methods
        /// <summary>
        /// Gets the EMG data for specified sensor.
        /// </summary>
        /// <param name="sensor">The index of the sensor. Must be greater than or equal to zero.</param>
        /// <returns>The data for the specified sensor or zero if the sensor does not exist.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="sensor"/> is less than zero.
        /// </exception>
        int GetDataForSensor(int sensor);
        #endregion
    }

    [ContractClassFor(typeof(IEmgData))]
    internal abstract class IEmgDataContract : IEmgData
    {
        #region Methods
        public int GetDataForSensor(int sensor)
        {
            Contract.Requires<ArgumentOutOfRangeException>(sensor >= 0, "The sensor value must be greater than or equal to zero.");

            return default(int);
        }
        #endregion
    }
}
