using System;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.ConsoleSample.Internal;
using MyoSharp.Poses;
using MyoSharp.Exceptions;

namespace MyoSharp.ConsoleSample
{
    /// <summary>
    /// Myo devices can notify you every time the device detects that the user 
    /// is performing a different pose. However, sometimes it's useful to know
    /// when a user is still holding a pose and not just that they've 
    /// transitioned from one pose to another. The <see cref="HeldPose"/> class
    /// monitors a Myo and notifies you as long as a particular pose is held.
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
    internal class HeldPoseExample
    {
        #region Methods
        private static void Main()
        {
            // create a hub to manage Myos
            using (var channel = Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()))))
            using (var hub = Hub.Create(channel))
            {
                // listen for when a Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);

                    // unlock the Myo so that it doesn't keep locking between our poses
                    e.Myo.Unlock(UnlockType.Hold);

                    // setup for the pose we want to watch for
                    var pose = HeldPose.Create(e.Myo, Pose.Fist, Pose.FingersSpread);

                    // set the interval for the event to be fired as long as 
                    // the pose is held by the user
                    pose.Interval = TimeSpan.FromSeconds(0.5);

                    pose.Start();
                    pose.Triggered += Pose_Triggered;
                };

                // start listening for Myo data
                channel.StartListening();

                ConsoleHelper.UserInputLoop(hub);
            }
        }
        #endregion

        #region Event Handlers
        private static void Pose_Triggered(object sender, PoseEventArgs e)
        {
            Console.WriteLine("{0} arm Myo is holding pose {1}!", e.Myo.Arm, e.Pose);
        }
        #endregion
    }
}