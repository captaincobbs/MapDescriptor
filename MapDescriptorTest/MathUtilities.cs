using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest
{
    /// <summary>
    /// Class of common math functions I don't like doing
    /// </summary>
    public static class MathUtilities
    {
        /// <summary>
        /// Forces a float to stay within the range specified.
        /// </summary>
        /// <param name="value">float to check is within the range</param>
        /// <param name="min">Minimum wanted value</param>
        /// <param name="max">Maximum wanted value</param>
        /// <returns>Returns value if within max and min, otherwise returns min or max</returns>
        public static float ContainInRange(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }
    }
}
