MyoSharp
========
```
  __  ____     ______   _____ _    _          _____  _____  
 |  \/  \ \   / / __ \ / ____| |  | |   /\   |  __ \|  __ \ 
 | \  / |\ \_/ / |  | | (___ | |__| |  /  \  | |__) | |__) |
 | |\/| | \   /| |  | |\___ \|  __  | / /\ \ |  _  /|  ___/ 
 | |  | |  | | | |__| |____) | |  | |/ ____ \| | \ \| |     
 |_|  |_|  |_|  \____/|_____/|_|  |_/_/    \_\_|  \_\_|     
                                                         
```

C# Wrapper for the Myo Armband

The goal of this project is to create a high-level object-oriented C# API for devs to easily interact with the Myo.

MyoSharp is compatible with .NET 2.0+

### Getting Started
* [`Setup`](#setup)
* [`Pose Sequences`](#poseseq)
* [`Holding Poses`](#posehold)
* [`Roll, Pitch and Yaw Data`](#rpy)
* [`License`](#license)


<a name='setup' />
### Sample Usage
``` C#
using System;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.ConsoleSample.Internal;

namespace MyoSharp.ConsoleSample
{
    /// <summary>
    /// This example will show you the basics for setting up and working with 
    /// a Myo using MyoSharp. Primary communication with the device happens 
    /// over Bluetooth, but this C# wrapper hooks into the unmanaged Myo SDK to
    /// listen on their "hub". The unmanaged hub feeds us information about 
    /// events, so a channel within MyoSharp is responsible for publishing 
    /// these events for other C# code to consume. A device listener uses a 
    /// channel to listen for pairing events. When a Myo pairs up, a device 
    /// listener publishes events for others to listen to. Once we have access 
    /// to a channel and a Myo handle (from something like a Pair event), we 
    /// can create our own Myo object. With a Myo object, we can do things like
    /// cause it to vibrate or monitor for poses changes.
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
    internal class BasicSetupExample
    {
        #region Methods
        private static void Main()
        {
            // create a hub that will manage Myo devices for us
            using (var channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create())))
            using (var hub = Hub.Create(channel))
            {
                // listen for when the Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
                    e.Myo.Vibrate(VibrationType.Short);
                    e.Myo.PoseChanged += Myo_PoseChanged;
                    e.Myo.Locked += Myo_Locked;
                    e.Myo.Unlocked += Myo_Unlocked;
                };

                // listen for when the Myo disconnects
                hub.MyoDisconnected += (sender, e) =>
                {
                    Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);
                    e.Myo.PoseChanged -= Myo_PoseChanged;
                    e.Myo.Locked -= Myo_Locked;
                    e.Myo.Unlocked -= Myo_Unlocked;
                };

                // start listening for Myo data
                channel.StartListening();

                // wait on user input
                ConsoleHelper.UserInputLoop(hub);
            }
        }
        #endregion

        #region Event Handlers
        private static void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            Console.WriteLine("{0} arm Myo detected {1} pose!", e.Myo.Arm, e.Myo.Pose);
        }

        private static void Myo_Unlocked(object sender, MyoEventArgs e)
        {
            Console.WriteLine("{0} arm Myo has unlocked!", e.Myo.Arm);
        }

        private static void Myo_Locked(object sender, MyoEventArgs e)
        {
            Console.WriteLine("{0} arm Myo has locked!", e.Myo.Arm);
        }
        #endregion
    }
}
```

<a name='poseseq' />
### Detecting Sequences of Poses
With this implementation, you can define your own creative sequences of poses. See the example below.
``` C#
using System;

using MyoSharp.Communication;
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
            using (var channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create())))
            using (var hub = Hub.Create(channel))
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

                // start listening for Myo data
                channel.StartListening();

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
```

<a name='posehold' />
### Detecting Poses Being Held
It's easy to be notified when a pose is being held by the user. You can even define an interval to adjust granularity. See the example below.
``` C#
using System;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.ConsoleSample.Internal;
using MyoSharp.Poses;

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
            using (var channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create())))
            using (var hub = Hub.Create(channel))
            {
                // listen for when a Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);

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
```
<a name='rpy' />
### Getting Roll, Pitch and Yaw data
Don't get lost in the orientation Quaternion vectors, use the <strong>OrientationDataEventArgs</strong> object to get the roll, pitch and yaw of the Myo
``` C#
using System;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.ConsoleSample.Internal;

namespace MyoSharp.ConsoleSample
{
    /// <summary>
    /// This example will show you how to hook onto the orientation events on
    /// the Myo and pull roll, pitch and yaw values from it. In this example the 
    /// raw vectors from the orientation event args are converted to roll, pitch and yaw
    /// on a scale from 0 to 9, depending on the position of the myo
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
    internal class OrientationExample
    {
        #region Methods
        private static void Main()
        {
            // create a hub that will manage Myo devices for us
            using (var channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create())))
            using (var hub = Hub.Create(channel))
            {
                // listen for when the Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
                    e.Myo.Vibrate(VibrationType.Short);
                    e.Myo.OrientationDataAcquired += Myo_OrientationDataAcquired;
                };

                // listen for when the Myo disconnects
                hub.MyoDisconnected += (sender, e) =>
                {
                    Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);
                    e.Myo.OrientationDataAcquired -= Myo_OrientationDataAcquired;
                };

                // start listening for Myo data
                channel.StartListening();

                // wait on user input
                ConsoleHelper.UserInputLoop(hub);
            }
        }
        #endregion

        #region Event Handlers
        private static void Myo_OrientationDataAcquired(object sender, OrientationDataEventArgs e)
        {
            const float PI = (float)System.Math.PI;

            // convert the values to a 0-9 scale (for easier digestion/understanding)
            var roll = (int)((e.Roll + PI) / (PI * 2.0f) * 10);
            var pitch = (int)((e.Pitch + PI) / (PI * 2.0f) * 10);
            var yaw = (int)((e.Yaw + PI) / (PI * 2.0f) * 10);

            Console.Clear();
            Console.WriteLine(@"Roll: {0}", roll);
            Console.WriteLine(@"Pitch: {0}", pitch);
            Console.WriteLine(@"Yaw: {0}", yaw);
        }
        #endregion
    }
}
```

<a name='license' />
### License
MyoSharp uses the MIT License.
Copyright (c) 2014 Nick Cosentino, Tayfun Uzun
