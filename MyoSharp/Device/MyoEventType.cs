using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    public enum MyoEventType
    {
        Paired,
        Unpaired,
        Connected,
        Disconnected,
        ArmRecognized,
        ArmLost,
        Orientation,
        Pose,
        Rssi
    }
}
