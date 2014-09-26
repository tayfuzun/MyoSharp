MyoSharp
========

C# Wrapper for the Myo Armband

The goal of this project is to allow a high-level object-oriented C# API for devs to easily interact with the Myo.

MyoSharp is compatible with .NET 2.0+

<h3>Sample Usage</h3>
``` C#
using System;
using System.Collections.Generic;

using MyoSharp.Device;
using MyoSharp.Discovery;
using MyoSharp.Communication;
using MyoSharp.Poses;

namespace MyoSharp.ConsoleSample
{
    internal class Program
    {
        private static readonly Dictionary<IntPtr, IMyo> _myos = new Dictionary<IntPtr, IMyo>();

        private static void Main(string[] args)
        {
            using (var channel = Channel.Create("com.myosharp.consolesample"))
            {
                channel.StartListening();

                var deviceListener = DeviceListener.Create(channel);
                deviceListener.Paired += DeviceListener_Paired;

                UserInputLoop();
            }
        }

        private static void UserInputLoop()
        {
            string userInput;
            while (!string.IsNullOrEmpty((userInput = Console.ReadLine())))
            {
                if (userInput.Equals("pose", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var myo in _myos.Values)
                    {
                        Console.WriteLine("Myo {0} in pose {1}.", myo.Handle, myo.Pose);
                    }
                }
                else if (userInput.Equals("arm", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var myo in _myos.Values)
                    {
                        Console.WriteLine("Myo {0} is on {1} arm.", myo.Handle, myo.Arm.ToString().ToLower());
                    }
                }
            }
        }

        private static void DeviceListener_Paired(object sender, PairedEventArgs e)
        {
            Console.WriteLine("Myo {0} has been paired!", e.MyoHandle);

            // we already have a Myo from a previous pair attempt
            if (_myos.ContainsKey(e.MyoHandle))
            {
                return;
            }

            // create a new Myo and hook up some events to it!
            var myo = Myo.Create(
                e.MyoHandle, 
                ((IDeviceListener)sender).ChannelListener);
            myo.PoseChanged += Myo_PoseChange;
            myo.Connected += Myo_Connected;
            myo.Disconnected += Myo_Disconnected;

            _myos[e.MyoHandle] = myo;
        }
        
        private static void Myo_Connected(object sender, MyoEventArgs e)
        {
            Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
            e.Myo.Vibrate(VibrationType.Short);
        }

        private static void Myo_Disconnected(object sender, MyoEventArgs e)
        {
            Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);
        }

        private static void Myo_PoseChange(object sender, PoseEventArgs e)
        {
            Console.WriteLine("{0} arm Myo detected {1} pose!", e.Myo.Arm, e.Myo.Pose);
        }
    }
}
```

<h3>Detecting Sequences of Poses</h3>
With this implementation, you can define your own creative sequences of poses. See the example below.
``` C#
private static void Main(string[] args)
{
    // NOTE: Setup left out for brevity
    var channel = Channel.Create(/*...*/);
    var deviceListener = DeviceListener.Create(/*...*/);
    var myo = Myo.Create(/*...*/);
    
    // Create a new pose sequence that will listen to the Myo's pose change events
    var poseSequence = new PoseSequence(
	e.Myo, 
	Pose.WaveOut, 
	Pose.WaveIn, 
	Pose.WaveOut ); 
    
    // This event will fire when the Myo detects the 3 poses defined above 
    // (WaveOut, WaveIn, WaveOut) in a sequence
    var poseSequence = PoseSequence.Create(myo, Pose.WaveOut, Pose.WaveIn);
    poseSequence.PoseSequenceCompleted += (_, poseArgs) =>
    {
	Console.WriteLine("{0} arm Myo did a fancy pose!", poseArgs.Myo.Arm);
	myo.Vibrate(VibrationType.Long);
    };
}

```

<h3>Detecting Poses Being Held</h3>
It's easy to be notified when a pose is being held by the user. You can even define an interval to adjust granularity. See the example below.
``` C#
private static void Main(string[] args)
{
    // NOTE: Setup left out for brevity
    var channel = Channel.Create(/*...*/);
    var deviceListener = DeviceListener.Create(/*...*/);
    var myo = Myo.Create(/*...*/);
    
    // This event will fire at a regular interval as long as the specified pose is being held
    var held = HeldPose.Create(myo, Pose.Fist);
    held.Interval = TimeSpan.FromSeconds(0.5);
    held.Triggered += (_, poseArgs) =>
    {
	Console.WriteLine("{0} arm Myo is holding the {1} pose!", poseArgs.Myo.Arm, poseArgs.Pose);
    };
    held.Start();
}

```




            