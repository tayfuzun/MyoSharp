using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
