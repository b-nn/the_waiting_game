using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Global;
using BreakInfinity;

namespace Player
{
    /// <summary>
    /// 
    /// </summary>
    public static class player {
        public static double BaseTime = 1;
        public static double WaitCurrency;
        public static BigDouble Time = 0; // Expressed in seconds
        public static double Scale = 0;
        public static double Deltons = 0;
        public static int DeltonCap = 100;
        public static double DeltonCapModifier = 0;
        public static BigDouble BaseTimePerSeconds = 1;
        public static BigDouble TotalTimePerSeconds;
        public static List<double> Multipliers = new List<double>();
        public static BigDouble Flips = 30;
        public static Double[] Generators = {0, 0, 0, 0};
        public static double[] GeneratorEfficiencies = {0.2,0.2,0.2,0.2};
        public static bool[] FlipUpgrades = {false, false, false, false, false, false, false, false};
        public static double TimeSinceFlip;
    }
}
