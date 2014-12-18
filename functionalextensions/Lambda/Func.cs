using System;

namespace FunctionalExtensions.Lambda
{
    public static class Fun
    {
        public static Func<T> Create<T>(Func<T> del)
        {
            return del;
        }
    }
}
