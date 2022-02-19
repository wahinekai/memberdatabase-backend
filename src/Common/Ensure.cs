// -----------------------------------------------------------------------
// <copyright file="Ensure.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

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
        /// <param name="name">Name of the parameter to be checked</param>
        /// <returns>The object, which is now not null</returns>
        public static T IsNotNull<T>(Func<T?> input, string name = "")
        {
            var invoked = input.Invoke();

            if (invoked is null)
            {
                throw new ArgumentNullException($"{name} cannot be null");
            }

            return invoked;
        }

        /// <summary>
        /// Ensures two objects are equal
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="object1">Left-hand-side of the equals</param>
        /// <param name="object2">Right-hand-side of the equals</param>
        /// <returns>The first object, which is equal to the second</returns>
        public static T AreEqual<T>(Func<T?> object1, Func<T?> object2)
        {
            var leftHandSide = Ensure.IsNotNull(object1);
            var rightHandSide = Ensure.IsNotNull(object2);

            var areEqual = leftHandSide?.Equals(rightHandSide);

            if (!(areEqual ?? false))
            {
                throw new ArgumentNullException($"{leftHandSide} isnot equal to {rightHandSide}");
            }

            return leftHandSide;
        }

        /// <summary>
        /// Ensures a boolean is true
        /// </summary>
        /// <param name="input">Nullable input boolean</param>
        /// <param name="name">Name of the parameter to be checked</param>
        /// <returns>The boolean, which is true</returns>
        public static bool IsTrue(Func<bool?> input, string name = "")
        {
            var invokedMaybeNull = Ensure.IsNotNull(input, name);

            var invoked = invokedMaybeNull ?? false;

            if (invoked == false)
            {
                throw new ArgumentNullException($"{name} is false");
            }

            return invoked;
        }

        /// <summary>
        /// Ensures a boolean is false
        /// </summary>
        /// <param name="input">Nullable input boolean</param>
        /// <param name="name">Name of the parameter to be checked</param>
        /// <returns>The boolean, which is false</returns>
        public static bool IsFalse(Func<bool?> input, string name = "")
        {
            var invokedMaybeNull = Ensure.IsNotNull(input, name);

            var invoked = invokedMaybeNull ?? true;

            if (invoked == true)
            {
                throw new ArgumentNullException($"{name} is true");
            }

            return invoked;
        }

        /// <summary>
        /// Makes sure the string input is not null or entirely whitespace
        /// </summary>
        /// <param name="input">A function returning the input string</param>
        /// <param name="name">Name of the parameter to be checked</param>
        /// <returns>A non-nullable string</returns>
        public static string IsNotNullOrWhitespace(Func<string?> input, string name = "")
        {
            var invokedMaybeNull = Ensure.IsNotNull<string>(input, name);

            var invoked = invokedMaybeNull ?? string.Empty;

            if (invoked.Trim().Length == 0)
            {
                throw new ArgumentException($"{name} is whitespace");
            }

            return invoked;
        }

        /// <summary>
        /// Ensures that an array of any type is not null or empty
        /// </summary>
        /// <typeparam name="T">The array element type</typeparam>
        /// <param name="input">A function which, when invoked, gives the array</param>
        /// <param name="name">Name of the parameter to be checked</param>
        /// <returns>A non-null and non-empty array</returns>
        public static IEnumerable<T> IsNotNullOrEmpty<T>(Func<IEnumerable<T>?> input, string name = "")
        {
            var invokedMaybeNull = Ensure.IsNotNull<IEnumerable<T>>(input, name);

            var invoked = invokedMaybeNull ?? new Collection<T>();

            if (!invoked.Any())
            {
                throw new ArgumentException($"{name} is empty");
            }

            return invoked;
        }
    }
}
