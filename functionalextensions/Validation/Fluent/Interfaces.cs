//using System;

//namespace FunctionalExtensions.Validation.Fluent
//{
//    public interface IChain<T>
//    {
//        IValidateThat<TResult> And<TResult>(Func<T, TResult> selector);
//        Choice<T, Errors> Result { get; }
//    }

//    public interface IValidateThat<T>
//    {
//        IChain<T> IsNotNull(string err);
//        IChain<T> Equals(T pattern, string err);
//        IChain<T> Fulfills(Func<T, bool> pred, string err);
//    }

//    public static class Validate2
//    {
//        public static IValidateThat<T> That<T>(T instance)
//        {
//            return new Dummy<T>(instance);
//        }
//    }

//    public class Dummy<T> : IValidateThat<T>
//    {
//        public Dummy(T instance)
//        {
//        }

//        public IChain<T> IsNotNull(string err)
//        {
//            throw new NotImplementedException();
//        }

//        public IChain<T> Equals(T pattern, string err)
//        {
//            throw new NotImplementedException();
//        }

//        public IChain<T> Fulfills(Func<T, bool> pred, string err)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
