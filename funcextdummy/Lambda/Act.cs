using System;

namespace FunctionalExtensions.Lambda
{
    public static class Act
    {
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
