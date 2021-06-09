using System;
using System.Threading;
using PinePhoneCore.Devices;

namespace ppstepd
{
    class Program
    {
        static void Main(string[] args)
        {
            Accelerometer.CalibratePosition();
            Accelerometer.CalibrateAngVel();

            StepDetector.Start();
            while (true)
                Thread.Sleep(int.MaxValue);
        }
    }
}
