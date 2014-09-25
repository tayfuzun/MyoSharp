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
