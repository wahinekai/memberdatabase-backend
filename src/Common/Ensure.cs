// -----------------------------------------------------------------------
// <copyright file="Ensure.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Common
{
    using System;

    /// <summary>
    /// Utility Methods to sanity check objects and arguments
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Ensures an object is not null
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="input">Nullable input object</param>
        /// <returns>The object, which is now not null</returns>
        public static T IsNotNull<T>(Func<T?> input)
        {
            var invoked = input.Invoke();

            if (invoked == null)
            {
                throw new ArgumentNullException($"{invoked} is null");
            }

            return invoked;
        }

        /// <summary>
        /// Makes sure the string input is not null or entirely whitespace
        /// </summary>
        /// <param name="input">A function returning the input string</param>
        /// <returns>A non-nullable string</returns>
        public static string IsNotNullOrWhitespace(Func<string?> input)
        {
            var invoked = Ensure.IsNotNull<string>(input);

            if (invoked.Trim().Length == 0)
            {
                throw new ArgumentException($"{invoked} is whitespace");
            }

            return invoked;
        }

        /// <summary>
        /// Ensures that an array of any type is not null or empty
        /// </summary>
        /// <typeparam name="T">The array element type</typeparam>
        /// <param name="input">A function which, when invoked, gives the array</param>
        /// <returns>A non-null and non-empty array</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1011:Closing square brackets should be spaced correctly", Justification = "Nullable array")]
        public static T[] IsNotNullOrEmpty<T>(Func<T[]?> input)
        {
            var invoked = Ensure.IsNotNull<T[]>(input);

            if (invoked.Length == 0)
            {
                throw new ArgumentException($"{invoked} is empty");
            }

            return invoked;
        }
    }
}
