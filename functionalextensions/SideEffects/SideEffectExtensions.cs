using System;

namespace FunctionalExtensions.SideEffects
{
    public static class SideEffectExtensions
    {
        public static Option<T> Do<T>(this Option<T> source, Action sideEffect)
        {
            sideEffect();
            return source;
        }
    }
}
