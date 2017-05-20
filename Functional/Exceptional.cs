using System;
using System.Reflection;
using System.Collections.Generic;

namespace com.webjema.Functional
{

    public static class Exceptional {

        public static Exceptional<T> From<T>(System.Func<T> f) {
            return Exceptional<T>.From(f);
        }

        public static Exceptional<T> From<T>(System.Func<T> f, System.Type exceptionType) {
            return Exceptional<T>.From(f, exceptionType);
        }

        public static Exceptional<T> With<T>(T v) {
            return new Exceptional<T>(v);
        }

        public static Exceptional<T> Raise<T>(string v) {
            return Exceptional<T>.Raise(new System.Exception(v));
        }

        #if NETCOREAPP1_0
        public static bool IsAssignableFrom(this System.Type o, System.Type t) {
            return o.GetTypeInfo().IsAssignableFrom(t);
        }
        #endif
    }

    /// <summary>
    /// Simple Exception type for functional programming.
    /// Defaults to null if initialized as a default (best to use Uninitialized instead)
    /// </summary>
    public struct Exceptional<T> {

        readonly T tValue;
        readonly Exception exception;

        public static Exceptional<T> Uninitialized
        {
            get
            {
                return Raise(new InvalidProgramException("Uninitialized Exceptional"));
            }
        }

        public bool IsException
        {
            get
            {
                return exception != null;
            }
        }

        /// <summary>
        /// Create Exceptional with no exceptions. Accepts null.
        /// </summary>
        /// <param name="v">V.</param>
        public Exceptional(T v) {
            this.tValue = v;
            this.exception = null;
        }

        private Exceptional(T v, Exception e) {
            this.tValue = v;
            this.exception = e;
        }

        /// <summary>
        /// Create Exceptional with an exception
        /// </summary>
        /// <param name="e">E.</param>
        public static Exceptional<T> Raise(Exception e) {
            return new Exceptional<T>(default(T), e);
        }

        public static Exceptional<T> From(System.Func<T> f)
        {
            try {
                return new Exceptional<T>(f());
            } catch (Exception e) {
                return Raise(e);
            }
        }

        // only catch the specific exception that we want
        public static Exceptional<T> From(System.Func<T> f, System.Type exceptionType)
        {
            try {
                return new Exceptional<T>(f());
            } catch (Exception e) {
                if (exceptionType.IsAssignableFrom(e.GetType())) {
                    return Raise(e);
                } else {
                    throw;
                }
            }
        }

        public Exceptional<R> Select<R>(Func<T,R> f) {
            return IsException ? Exceptional<R>.Raise(exception) : new Exceptional<R>(f(tValue));
        }

        public Exceptional<R> Map<R>(Func<T,R> f) {
            return Select(f);
        }

        public Exceptional<R> SelectMany<R>(Func<T,Exceptional<R>> f) {
            return IsException ? Exceptional<R>.Raise(exception) : f(tValue);
        }

        public Exceptional<R> Bind<R>(Func<T,Exceptional<R>> f) {
            return SelectMany(f);
        }

         public Exceptional<RR> SelectMany<R,RR>(Func<T,Exceptional<R>> binder, Func<T,R,RR> project) {
            var tv = tValue;
            return IsException ?  Exceptional<RR>.Raise(exception) :
                binder(tValue).SelectMany(r => new Exceptional<RR>(project(tv, r)));
        }

        // returns the result, throwing an exception on failure
        public T GetResultWithException() {
            if (!IsException) {
                return tValue;
            } else {                
                throw exception;
            }
        }

        public R Match<R>(Func<T, R> onSuccess, Func<Exception, R> onFailure) {
            return IsException ? onFailure(exception) : onSuccess(tValue);
        }

        public void MatchAction(Action<T> onSuccess, Action<Exception> onFailure) {
            if (IsException)
            {
                onFailure(exception);
            }
            else
            {
                onSuccess(tValue);
            }
        }

        public Option<T> ToOption() {
            return Match(t => Option.Some(t), ex => Option.None<T>());
        }

        public IEnumerable<T> ToEnumerable() {
            if (!this.IsException) {
                yield return tValue;
            }
        }

    }

}