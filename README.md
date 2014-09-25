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

namespace MyoSharp.ConsoleSample
{
    internal class Program
    {
        private static readonly Dictionary<IntPtr, IMyo> _myos = new Dictionary<IntPtr, IMyo>();

        private static void Main(string[] args)
        {
            var hub = Hub.Create("com.myosharp.consolesample");
            hub.StartListening();

            hub.Paired += Hub_Paired;

            Console.ReadLine();
        }

        private static void Hub_Paired(object sender, PairedEventArgs e)
        {
            var myo = Myo.Create((IHub)sender, e.MyoHandle);
            myo.PoseChanged += Myo_PoseChange;
            myo.Connected += Myo_Connected;
            _myos[e.MyoHandle] = myo;
        }

        private static void Myo_Connected(object sender, MyoEventArgs e)
        {
            e.Myo.Disconnected += Myo_Disconnected;
            Console.WriteLine("Myo Connected");
        }

        private static void Myo_Disconnected(object sender, MyoEventArgs e)
        {
            Console.WriteLine("Oh no, looks like the Myo disconnected!");
        }

        private static void Myo_PoseChange(object sender, PoseEventArgs e)
        {
            Console.WriteLine("Pose Detected: " + e.Pose);
        }
    }
}
```

<h3>Detecting Sequences of Poses</h3>
With this implementation you can define your own creative sequences of poses, see example below.
``` C#
private static void Main(string[] args)
{
    var hub = hub;// Hub setup is ommitted
    var myo = Myo.Create((IHub)hub, e.MyoHandle);
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
