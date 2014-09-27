using System;

using MyoSharp.Device;

namespace MyoSharp.ConsoleSample.Internal
{
    internal static class ConsoleHelper
    {
        #region Methods
        internal static void UserInputLoop(IHub hub)
        {
            string userInput;
            while (!string.IsNullOrEmpty((userInput = Console.ReadLine())))
            {
                if (userInput.Equals("pose", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var myo in hub.Myos)
                    {
                        Console.WriteLine("Myo {0} in pose {1}.", myo.Handle, myo.Pose);
                    }
                }
                else if (userInput.Equals("arm", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var myo in hub.Myos)
                    {
                        Console.WriteLine("Myo {0} is on {1} arm.", myo.Handle, myo.Arm.ToString().ToLower());
                    }
                }
                else if (userInput.Equals("count", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("There are {0} Myo(s) connected.", hub.Myos.Count);
                }
            }
        }
        #endregion
    }
}