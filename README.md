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
* [`Environment`](#environment)
* [`Setup`](#setup)
* [`Visualization in WinForms`](#winformvis)
* [`Pose Sequences`](#poseseq)
* [`Holding Poses`](#posehold)
* [`Roll, Pitch and Yaw Data`](#rpy)
* [`Acquiring EMG Data`](#emg)
* [`License`](#license)

<a name='environment' />
### Environment
* We suggest that you clone down the MyoSharp repository to a directory relative to your project. This will allow you to safely pull down future changes into this repository.
  * **NOTE**: You aren't required to do it this way. You can copy the MyoSharp project and files into your solution if you want, but in my opinion that would make updates a little bit trickier. It's up to you though.
* You should create your new solution and reference the MyoSharp project called "MyoSharp.csproj". This is the project file for the MyoSharp assembly. The other assemblies are examples, and your final project will **NOT** be required to reference these other projects.
  * **NOTE**: MyoSharp is targeted at the .NET 2.0 framework. If you're building projects targeting later frameworks, you should still have no issue referencing MyoSharp assemblies. If you're targeting prior to .NET 2.0... I can't help you.
* MyoSharp uses [Code Contracts](https://msdn.microsoft.com/en-us/library/dd264808(v=vs.100).aspx). It was our opinion that providing code contracts would allow for a more clear API for our consumers. Please install code contracts to use this project.
  * [Microsoft Research: Code Contracts](http://research.microsoft.com/en-us/projects/contracts/)
  * [Visual Studio Gallery: Code Contracts for .NET](https://visualstudiogallery.msdn.microsoft.com/1ec7db13-3363-46c9-851f-1ce455f66970)

<a name='setup' />
### Sample Usage
Here's a simple example of a console program that shows MyoSharp in action:
``` C#
using System;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.ConsoleSample.Internal;
using MyoSharp.Exceptions;

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
            using (var channel = Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()))))
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

<a name='winformvis' />
### Visualizations in WinForms
Running the MyoSHarp.EmgVisualization project will pop up a form that allows you to view the EMG data that comes in from the device. It uses a ZedGraph component to do all of the drawing, and has some simple features like toggling sensors and zooming in to regions. It's primitive, but it should give you an idea what the data looks like.


<a name='poseseq' />
### Detecting Sequences of Poses
With this implementation, you can define your own creative sequences of poses. See the example below.
``` C#
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
            using (var channel = Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()))))
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
```
<a name='rpy' />
### Getting Roll, Pitch and Yaw data
Don't get lost in the orientation Quaternion vectors, use the <strong>OrientationDataEventArgs</strong> object to get the roll, pitch and yaw of the Myo
``` C#
using System;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.ConsoleSample.Internal;
using MyoSharp.Exceptions;

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
            using (var channel = Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()))))
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
<a name='emg' />
### Acquiring EMG Data
Thalmic has now opened up access to the raw EMG data that comes off of the Myo device. Simply enable EMG data streaming on your Myo and listen to the <strong>EmgDataAcquired</strong> event.
``` C#
using System;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.ConsoleSample.Internal;
using MyoSharp.Exceptions;

namespace MyoSharp.ConsoleSample
{
    /// <summary>
    /// This example will show you how to get up and running with streaming 
    /// EMG data from your Myo device.
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
    internal class EmgStreamingExample
    {
        #region Methods
        private static void Main()
        {
            // create a hub that will manage Myo devices for us
            using (var channel = Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(), 
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()))))
            using (var hub = Hub.Create(channel))
            {
                // listen for when the Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
                    e.Myo.Vibrate(VibrationType.Short);
                    e.Myo.EmgDataAcquired += Myo_EmgDataAcquired;
                    e.Myo.SetEmgStreaming(true);
                };

                // listen for when the Myo disconnects
                hub.MyoDisconnected += (sender, e) =>
                {
                    Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);
                    e.Myo.SetEmgStreaming(false);
                    e.Myo.EmgDataAcquired -= Myo_EmgDataAcquired;
                };

                // start listening for Myo data
                channel.StartListening();

                // wait on user input
                ConsoleHelper.UserInputLoop(hub);
            }
        }
        #endregion

        #region Event Handlers
        private static void Myo_EmgDataAcquired(object sender, EmgDataEventArgs e)
        {
            // TODO: write your code to interpret EMG data!
        }
        #endregion
    }
}
```

<a name='license' />
### License
MyoSharp uses the MIT License.
Copyright (c) 2014 Nick Cosentino, Tayfun Uzun
