MyoSharp
========

C# Wrapper for the Myo Armband

The goal of this project is to allow a high-level object-oriented C# API for devs to easily interact with the Myo.

MyoSharp is compatible with .NET 2.0+

<h3>Sample Usage</h3>
``` C#
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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
                        Console.WriteLine("Myo ({0}) in pose {1}.", myo.Handle, myo.Pose);
                    }
                }
                else if (userInput.Equals("arm", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var myo in _myos.Values)
                    {
                        Console.WriteLine("Myo ({0}) is on {1} arm.", myo.Handle, myo.Arm.ToString().ToLower());
                    }
                }
            }
        }

        private static void DeviceListener_Paired(object sender, PairedEventArgs e)
        {
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
            Console.WriteLine("Myo {0} Connected", e.Myo.Handle);
            e.Myo.Vibrate(VibrationType.Short);
        }

        private static void Myo_Disconnected(object sender, MyoEventArgs e)
        {
            Console.WriteLine("Oh no, looks like {0} arm Myo disconnected!", e.Myo.Arm);
            e.Myo.Dispose();
            _myos.Remove(e.Myo.Handle);
        }

        private static void Myo_PoseChange(object sender, PoseEventArgs e)
        {
            Console.WriteLine("{0} arm Myo detected {1} pose.", e.Myo.Arm, e.Myo.Pose);
        }
    }
}
```

<h3>Detecting Sequences of Poses</h3>
With this implementation you can define your own creative sequences of poses, see example below.
``` C#
private static void Main(string[] args)
{
    // NOTE: Setup left out for brevity
    var channel = Channel.Create(/*...*/);
    var deviceListener = DeviceListener.Create(*...*/);
    var myo = Myo.Create(*...*/);
    
    var poseSeq = new PoseSequence(e.Myo, Pose.WaveOut, Pose.WaveIn, Pose.WaveOut ); 
    poseSeq.PoseSequenceComplete += PoseSeq_PoseSequenceComplete;
}

// This event will fire when the Myo detects the 3 poses defined above 
// (WaveOut, WaveIn, WaveOut) in a sequence
private static void PoseSeq_PoseSequenceComplete(object sender, PoseEventArgs e)
{
    Console.WriteLine("Pose Sequence Detected!");
    e.Myo.Vibrate(VibrationType.Long);
}
```
