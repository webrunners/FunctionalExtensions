using System;

namespace FunctionalExtensions
{
    public static class Operators
    {
        public static Func<T, T> Id<T>()
        {
            return x => x;
        }

        public static T Id<T>(T value)
        {
            return value;
        }
    }
}
