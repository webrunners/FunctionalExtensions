using System;

namespace FunctionalExtensions
{
    public static class Lambda
    {
        public static Func<T> Create<T>(Func<T> del)
        {
            return del;
        }

        public static Action<T> Create<T>(Action<T> del)
        {
            return del;
        }

        public static Action Create(Action del)
        {
            return del;
        }
    }
}
