MyoSharp
========

C# Wrapper for the Myo Armband

The goal of this project is to allow a high-level object-oriented C# API for devs to easily interact with the Myo.

MyoSharp is compatible with .NET 2.0+

Sample Usage
```
using MyoSharp.Device;
using MyoSharp.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyoSharp
{
    class Program
    {

        static void Main(string[] args)
        {
            var hub = Hub.Create("com.myosharp.myoapp"); // creates the hub and initializes it
            hub.StartListening(); // starts searching for nearby Myos

            hub.Paired += hub_Paired;

            Console.ReadLine();
        }

        // This event is fired when the Myo has paired
        static void hub_Paired(object sender, MyoEventArgs e)
        {
            e.Myo.PoseChange += Myo_PoseChange;
            e.Myo.Connected += Myo_Connected;
        }

        // This event fires when the Myo has successfully connected
        static void Myo_Connected(object sender, MyoEventArgs e)
        {
            Console.WriteLine("Myo Connected");
        }

        // This event is fired when the Myo detects a pose change
        static void Myo_PoseChange(object sender, PoseEventArgs e)
        {
            Console.WriteLine("Pose Detected: " + e.Pose);
        }
    }
}
```
