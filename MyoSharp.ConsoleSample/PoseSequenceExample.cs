using System;

using MyoSharp.Device;
using MyoSharp.ConsoleSample.Internal;
using MyoSharp.Poses;

namespace MyoSharp.ConsoleSample
{
    /// <summary>
    /// Myo devices can notify you every time the device detects that the user 
    /// is performing a different pose. However, sometimes it's useful to know
    /// when a user has performed a series of poses. A 
    /// <see cref="PoseSequence"/> can monitor a Myo for a series of poses and
    /// notify you when that sequence has completed.
    /// </summary>
    /// <remarks>
    /// Not sure how to use this example?
    /// - Open Visual Studio
    /// - Go to the solution explorer
    /// - Find the project that this file is contained within
    /// - Right click on the project in the solution explorer, go to "properties"
    /// - Go to the "Application" tab
    /// - Under "Startup object" pick this example from the list
    /// - Hit F5 and you should be good to go!
    /// </remarks>
    internal class PoseSequenceExample
    {
        #region Methods
        private static void Main()
        {
            // create a hub to manage Myos
            using (var hub = Hub.Create())
            {
                // listen for when a Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);

                    // for every Myo that connects, listen for special sequences
                    var sequence = PoseSequence.Create(
                        e.Myo, 
                        Pose.WaveOut, 
                        Pose.WaveIn);
                    sequence.PoseSequenceCompleted += Sequence_PoseSequenceCompleted;
                };

                ConsoleHelper.UserInputLoop(hub);
            }
        }
        #endregion

        #region Event Handlers
        private static void Sequence_PoseSequenceCompleted(object sender, PoseSequenceEventArgs e)
        {
            Console.WriteLine("{0} arm Myo has performed a pose sequence!", e.Myo.Arm);
            e.Myo.Vibrate(VibrationType.Medium);
        }
        #endregion
    }
}