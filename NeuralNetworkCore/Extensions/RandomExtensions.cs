using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.MathF;

namespace NeuralNetwork.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        ///   Generates normally distributed numbers.
        /// </summary>
        /// <param name="random"></param>
        /// <param name = "mean">Mean of the distribution</param>
        /// <param name = "stdDev">Standard deviation</param>
        /// <returns></returns>
        public static float SampleGaussian(this Random random, float mean = 0, float stdDev = 1)
        {
            float x1 = 1 - random.NextSingle();
            float x2 = 1 - random.NextSingle();

            float y1 = Sqrt(-2.0f * Log(x1)) * Cos(2.0f * PI * x2);
            return y1 * stdDev + mean;
        }

        public static float NextSingle(this Random random, float min, float max)
        {
            return random.NextSingle() * (max - min) + min;
        }

        /// <summary>
        ///   Shuffles a array in O(n) time by using the Fisher-Yates/Knuth algorithm.
        /// </summary>
        /// <param name="random"></param>
        /// <param name = "array"></param>
        public static void Shuffle<T>(this Random random, T[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var j = random.Next(0, i + 1);

                var temp = array[j];
                array[j] = array[i];
                array[i] = temp;
            }
        }
    }
}
