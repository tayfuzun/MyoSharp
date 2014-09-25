using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    public enum Pose : uint
    {
        Rest = 0, ///< Rest pose.
        Fist = 1, ///< User is making a fist.
        WaveIn = 2, ///< User has an open palm rotated towards the posterior of their wrist.
        WaveOut = 3, ///< User has an open palm rotated towards the anterior of their wrist.
        FingersSpread = 4, ///< User has an open palm with their fingers spread away from each other.
        Reserved1 = 5, ///< Reserved value; not a valid pose.
        ThumbToPinky = 6, ///< User is touching the tip of their thumb to the tip of their pinky.
        Unknown = 0xffff    ///< Unknown pose.
    }
}