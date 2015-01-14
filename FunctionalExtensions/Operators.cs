using System;

namespace FunctionalExtensions
{
    public static class Operators
    {
        /// <summary>
        /// Returns a function that always returns the same value that was used as its argument
        /// </summary>
        /// <typeparam name="T">Type of the identity function</typeparam>
        /// <returns>The identity function</returns>
        public static Func<T, T> Id<T>()
        {
            return Id;
        }

        /// <summary>
        /// Always returns the same value that was used as its argument
        /// </summary>
        /// <typeparam name="T">Type of the argument</typeparam>
        /// <param name="value">The argument</param>
        /// <returns>The value that was used as its argument</returns>
        public static T Id<T>(T value)
        {
            return value;
        }
    }
}
