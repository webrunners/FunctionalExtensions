using System;

namespace FunctionalExtensions.FluentOption
{
    public static class OptionMonad
    {
        public static IIntermediate1<TResult> From<TResult>(Func<Option<TResult>> f)
        {
            return new Intermediate1<TResult>(f);
        }
    }
}