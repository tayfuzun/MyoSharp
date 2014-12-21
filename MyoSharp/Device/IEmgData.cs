using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    public interface IEmgData
    {
        #region Methods
        int GetDataForSensor(int sensor);
        #endregion
    }
}
