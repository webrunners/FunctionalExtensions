using System;

namespace FunctionalExtensions.Validation.Fluent
{
    //public interface IChain<T>
    //{
    //    IValidate<T, TResult> And<TResult>(Func<T, TResult> selector);
    //    Choice<T, Errors> Result { get; }
    //}

    //public interface IValidate<T, TMember> 
    //{
    //    IChain<T> IsNotNull(string err);
    //    IChain<T> Equals(T pattern, string err);
    //    IChain<T> Fulfills(Func<T, bool> pred, string err);
    //}

    //public static class Validate2
    //{
    //    public static IValidate<T> That<T>(T instance)
    //    {
    //        return new Dummy<T>(instance);
    //    }
    //}

    //public class Dummy<T> : IValidate<T>
    //{
    //    private T _instance;

    //    public Dummy(T instance)
    //    {
    //        _instance = instance;
    //    }

    //    public IChain<T> IsNotNull(string err)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IChain<T> Equals(T pattern, string err)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IChain<T> Fulfills(Func<T, bool> pred, string err)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
