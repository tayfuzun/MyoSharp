using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Poses
{
    public enum Pose : uint
    {
        /// <summary>
        /// When the user is at rest.
        /// </summary>
        Rest = 0,

        /// <summary>
        /// When the user makes a fist.
        /// </summary>
        Fist = 1,

        /// <summary>
        /// When the user has an open palm rotated towards the posterior of their wrist.
        /// </summary>
        WaveIn = 2,

        /// <summary>
        /// When the user has an open palm rotated towards the anterior of their wrist.
        /// </summary>
        WaveOut = 3,

        /// <summary>
        /// When the user has an open palm with their fingers spread away from each other.
        /// </summary>
        FingersSpread = 4,

        /// <summary>
        /// Reserved value; not a valid pose.
        /// </summary>
        Reserved1 = 5,

        /// <summary>
        /// When the user is touching the tip of their thumb to the tip of their pinky.
        /// </summary>
        ThumbToPinky = 6,

        /// <summary>
        /// Unknown pose.
        /// </summary>
        Unknown = 0xffff
    }
}