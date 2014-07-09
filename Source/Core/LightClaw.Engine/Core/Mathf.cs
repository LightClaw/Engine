﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace LightClaw.Engine.Core
{
    /// <summary>
    /// Contains extensions to the math class.
    /// </summary>
    public static class Mathf
    {
        /// <summary>
        /// Backing field.
        /// </summary>
        private static readonly string[] _HexTable = Enumerable.Range(0, 256).Select(i => i.ToString("X2")).ToArray();

        /// <summary>
        /// Contains the hexadecimal representation of all byte values.
        /// </summary>
        public static string[] HexTable
        {
            get
            {
                Contract.Ensures(Contract.Result<string[]>() != null);
                Contract.Ensures(Contract.Result<string[]>().Length == 256);

                return _HexTable.ToArray();
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private static readonly double _RootTwo = Math.Sqrt(2);

        /// <summary>
        /// The square root of two.
        /// </summary>
        public static double RootTwo
        {
            get
            {
                Contract.Ensures(Contract.Result<double>() > 1 && Contract.Result<double>() < 2);

                return _RootTwo;
            }
        }

        /// <summary>
        /// Makes sure the value stays in the given area.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value the value may have.</param>
        /// <param name="max">The maximum value the value may have.</param>
        /// <returns>The value cut off at the boundaries.</returns>
        public static int Clamp(int value, int min, int max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Makes sure the value stays in the given area.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value the value may have.</param>
        /// <param name="max">The maximum value the value may have.</param>
        /// <returns>The value cut off at the boundaries.</returns>
        public static float Clamp(float value, float min, float max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Makes sure the value stays in the given area.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value the value may have.</param>
        /// <param name="max">The maximum value the value may have.</param>
        /// <returns>The value cut off at the boundaries.</returns>
        public static double Clamp(double value, double min, double max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Gets the biggest common divisor of two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The greatest common divisor of the two numbers.</returns>
        public static int GreatestCommonDivisor(int a, int b)
        {
            while (b > 0)
            {
                int rem = a % b;
                a = b;
                b = rem;
            }

            return a;
        }

        /// <summary>
        /// Gets the biggest common divisor of a range of numbers.
        /// </summary>
        /// <param name="values">The numbers.</param>
        /// <returns>The greatest common divisor of the two numbers.</returns>
        public static int GreatestCommonDivisor(IEnumerable<int> values)
        {
            return values.Aggregate((gcd, arg) => GreatestCommonDivisor(gcd, arg));
        }

        /// <summary>
        /// Interpolates between two values using a linear function by a given amount.
        /// </summary>
        /// <remarks>
        /// See http://www.encyclopediaofmath.org/index.php/Linear_interpolation and
        /// http://fgiesen.wordpress.com/2012/08/15/linear-interpolation-past-present-and-future/
        /// </remarks>
        /// <param name="from">Value to interpolate from.</param>
        /// <param name="to">Value to interpolate to.</param>
        /// <param name="amount">Interpolation amount.</param>
        /// <returns>The result of linear interpolation of values based on the amount.</returns>
        public static double Lerp(double from, double to, double amount)
        {
            return (1 - amount) * from + amount * to;
        }

        /// <summary>
        /// Interpolates between two values using a linear function by a given amount.
        /// </summary>
        /// <remarks>
        /// See http://www.encyclopediaofmath.org/index.php/Linear_interpolation and
        /// http://fgiesen.wordpress.com/2012/08/15/linear-interpolation-past-present-and-future/
        /// </remarks>
        /// <param name="from">Value to interpolate from.</param>
        /// <param name="to">Value to interpolate to.</param>
        /// <param name="amount">Interpolation amount.</param>
        /// <returns>The result of linear interpolation of values based on the amount.</returns>
        public static float Lerp(float from, float to, float amount)
        {
            return (1 - amount) * from + amount * to;
        }

        /// <summary>
        /// Interpolates between two values using a linear function by a given amount.
        /// </summary>
        /// <remarks>
        /// See http://www.encyclopediaofmath.org/index.php/Linear_interpolation and
        /// http://fgiesen.wordpress.com/2012/08/15/linear-interpolation-past-present-and-future/
        /// </remarks>
        /// <param name="from">Value to interpolate from.</param>
        /// <param name="to">Value to interpolate to.</param>
        /// <param name="amount">Interpolation amount.</param>
        /// <returns>The result of linear interpolation of values based on the amount.</returns>
        public static byte Lerp(byte from, byte to, float amount)
        {
            return (byte)Lerp((float)from, (float)to, amount);
        }

        /// <summary>
        /// Rounds the value up to the next power of two.
        /// </summary>
        /// <param name="x">The value to round up.</param>
        /// <returns>The value's next power of two.</returns>
        [CLSCompliant(false)]
        public static uint NextPowerOfTwo(uint x)
        {
            // Applying bitwise operations causes the number to give us the right value in the end. See http://acius2.blogspot.de/2007/11/calculating-next-power-of-2.html

            x--;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x++;

            return x;
        }

        /// <summary>
        /// Rounds the value up to the next power of two.
        /// </summary>
        /// <param name="x">The value to round up.</param>
        /// <returns>The value's next power of two.</returns>
        [CLSCompliant(false)]
        public static ulong NextPowerOfTwo(ulong x)
        {
            // Applying bitwise operations causes the number to give us the right value in the end. See http://acius2.blogspot.de/2007/11/calculating-next-power-of-2.html

            x--;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x |= x >> 32;
            x++;

            return x;
        }

        /// <summary>
        /// Checks whether a given value is almost zero using the given accuracy.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="decimalPlaceCount">The amount of accuracy in decimal places.</param>
        /// <returns>Whether the input number is almost zero or not.</returns>
        public static bool IsAlmostZero(float value, int decimalPlaceCount)
        {
            decimalPlaceCount = Mathf.Clamp(decimalPlaceCount, 1, 15);
            double zero = 0.1;
            for (int i = 1; i < decimalPlaceCount; i++)
            {
                zero *= 0.1;
            }
            return ((double)value < zero && (double)value > zero);
        }

        /// <summary>
        /// Checks whether a given value is almost zero using the given accuracy.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="decimalPlaceCount">The amount of accuracy in decimal places.</param>
        /// <returns>Whether the input number is almost zero or not.</returns>
        public static bool IsAlmostZero(double value, int decimalPlaceCount)
        {
            decimalPlaceCount = Mathf.Clamp(decimalPlaceCount, 1, 15);
            double zero = 0.1;
            for (int i = 1; i < decimalPlaceCount; i++)
            {
                zero *= 0.1;
            }
            return (value < zero && value > zero);
        }

        /// <summary>
        /// Checks whether a number is a divisor of another number.
        /// </summary>
        /// <param name="n">The number to be divided.</param>
        /// <param name="divisor">The numbers divisor.</param>
        /// <returns>Whether n is dividable by the divisor.</returns>
        public static bool IsDivisorOf(int n, int divisor)
        {
            return (n % divisor == 0);
        }

        /// <summary>
        /// Checks whether a number is a divisor of another number.
        /// </summary>
        /// <param name="n">The number to be divided.</param>
        /// <param name="divisor">The numbers divisor.</param>
        /// <returns>Whether n is dividable by the divisor.</returns>
        public static bool IsDivisorOf(double n, double divisor)
        {
            return (n % divisor == 0.0d);
        }

        /// <summary>
        /// Determines the least common multiple of two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The least common multiple of the two numbers.</returns>
        public static int LeastCommonMultiple(int a, int b)
        {
            return (a * b) / GreatestCommonDivisor(a, b);
        }

        /// <summary>
        /// Determines the least common multiple of a collection of numbers.
        /// </summary>
        /// <param name="values">The numbers.</param>
        /// <returns>The least common multiple of all the numbers.</returns>
        public static int LeastCommonMultiple(IEnumerable<int> values)
        {
            return values.Aggregate((lcm, arg) => LeastCommonMultiple(lcm, arg));
        }

        /// <summary>
        /// Gets the power of two of the given exponent.
        /// </summary>
        /// <param name="n">The exponent.</param>
        /// <returns>Two to the power of the exponent.</returns>
        public static double PowerOfTwo(double n)
        {
            return Math.Pow(2, n);
        }

        /// <summary>
        /// Performs smooth (cubic Hermite) interpolation between 0 and 1.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Smoothstep
        /// </remarks>
        /// <param name="amount">Value between 0 and 1 indicating interpolation amount.</param>
        public static float SmoothStep(float amount)
        {
            return (amount <= 0) ?
                0 :
                (amount >= 1) ?
                    1 :
                    amount * amount * (3 - (2 * amount));
        }

        /// <summary>
        /// Performs smooth (cubic Hermite) interpolation between 0 and 1.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Smoothstep
        /// </remarks>
        /// <param name="amount">Value between 0 and 1 indicating interpolation amount.</param>
        public static double SmoothStep(double amount)
        {
            return (amount <= 0) ?
                0 :
                (amount >= 1) ?
                    1 :
                    amount * amount * (3 - (2 * amount));
        }

        /// <summary>
        /// Performs a smooth(er) interpolation between 0 and 1 with 1st and 2nd order derivatives of zero at endpoints.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Smoothstep
        /// </remarks>
        /// <param name="amount">Value between 0 and 1 indicating interpolation amount.</param>
        public static float SmootherStep(float amount)
        {
            return (amount <= 0) ?
                0 :
                (amount >= 1) ?
                    1 :
                    amount * amount * amount * (amount * ((amount * 6) - 15) + 10);
        }

        /// <summary>
        /// Performs a smooth(er) interpolation between 0 and 1 with 1st and 2nd order derivatives of zero at endpoints.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Smoothstep
        /// </remarks>
        /// <param name="amount">Value between 0 and 1 indicating interpolation amount.</param>
        public static double SmootherStep(double amount)
        {
            return (amount <= 0) ?
                0 :
                (amount >= 1) ?
                    1 :
                    amount * amount * amount * (amount * ((amount * 6) - 15) + 10);
        }
    }
}