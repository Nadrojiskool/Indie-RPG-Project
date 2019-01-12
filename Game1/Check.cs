using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Check
    {
        // Check value against a minimum, if below return min //
        public static int Min(int value, int min)
        {
            if (value < min)
            {
                value = min;
            }
            return (value);
        }
        // Check value against a maximum, if above return max //
        public static int Max(int value, int max)
        {
            if (value > max)
            {
                value = max;
            }
            return (value);
        }
        // Could also be called "CheckRange" //
        public static int Range(int value, int min, int max)
        {
            value = Min(value, min);
            value = Max(value, max);
            return (value);
        }
        // Expected one iteration on loop max - min to avoid while statement
        public static int LoopInt(int num, int loopMin, int loopMax)
        {
            int loopSize = loopMax - (loopMin - 1);

            if (num > loopMax)
            {
                num -= loopSize;
            }
            else if (num < loopMin)
            {
                num += loopSize;
            }

            return (num);
        }
    }
}
