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

        private static void Hub_Paired(object sender, PairedEventArgs e)
        {
            // create a new Myo and hook up some events to it!
            var myo = Myo.Create((IHub)sender, e.MyoHandle);
            myo.PoseChanged += Myo_PoseChange;
            myo.Connected += Myo_Connected;
            myo.Disconnected += Myo_Disconnected;
            _myos[e.MyoHandle] = myo;
        }

        private static void PoseSeq_PoseSequenceComplete(object sender, PoseEventArgs e)
        {
            e.Myo.Vibrate(VibrationType.Long);
        }

        private static void Myo_Connected(object sender, MyoEventArgs e)
        {
            Console.WriteLine("Myo {0} Connected", e.Myo.Handle);
            var poseSeq = new PoseSequence(e.Myo, Pose.WaveOut, Pose.WaveIn );
            poseSeq.PoseSequenceComplete += PoseSeq_PoseSequenceComplete;
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
