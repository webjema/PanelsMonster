using System;
using System.Collections.Generic;

namespace com.webjema.Functional {

    /// <summary>
    /// Simple Option type for functional programming. Usage:
    /// Option.Some(x) or Option.None or Option.Return(x)
    /// </summary>
    public static class Option {
        public static Option<T> Some<T>(T value) {
            return new Option<T>(value);
        }

        public static Option<T> None<T>() {
            return new Option<T>();
        }

        public static Option<T> Return<T>(T value) {
            return (value != null) ? Some(value) : None<T>();
        }

        public static Option<T> BindNone<T>(this Option<T> o, Func<Option<T>> none) {
            return o.Bind(r => o, () => none());
        }  

        public static T MatchNone<T>(this Option<T> o, Func<T> none) {
            return o.Match(r => r, () => none());
        }

    } // class Option

    /// <summary>
    /// Simple Option type for functional programming.
    /// Defaults to None if instantiated without a constructor    
    /// </summary>
    public struct Option<T> {

        readonly T tValue;
        readonly bool isSome;

        public bool IsNone
        {
            get
            {
                return !isSome;
            }
        }

        public bool IsSome
        {
            get
            {
                return isSome;
            }
        }

        public Option(T v)
        {
            if (v == null) {
                isSome = false;
                tValue = default(T);
            } else {
                this.isSome = true;
                this.tValue = v;
            }
        }

        public static Option<T> None
        {
            get
            {
                return new Option<T>();
            }
        }

        public Option<R> Map<R>(Func<T,R> f) {
            return Select(f);
        }

        public Option<T> BindNone(Func<Option<T>> f) {
            return isSome ? this : f();
        }

        public Option<R> Select<R>(Func<T,R> f) {
            return isSome ? new Option<R>(f(tValue)) : new Option<R>();
        }

        public Option<R> Bind<R>(Func<T,Option<R>> f) {
            return SelectMany(f);
        }

        public Option<R> Bind<R>(Func<T,Option<R>> f, Func<Option<R>> none) {
            return isSome ? f(tValue) : none();
        }

        public Option<R> SelectMany<R>(Func<T,Option<R>> f) {
            return isSome ? f(tValue) : new Option<R>();
        }

         public Option<RR> SelectMany<R,RR>(Func<T,Option<R>> binder, Func<T,R,RR> project) {
            var tv = tValue;
            return isSome ? binder(tValue).SelectMany(r => Option.Return(project(tv, r))) : new Option<RR>();
        }

        public R Match<R>(Func<T, R> some, Func<R> none) {
            return isSome ? some(tValue) : none();
        }

        public void MatchAction(Action<T> some, Action none) {
            if (isSome)
            {
                some(tValue);
            }
            else
            {
                none();
            }
        }

        public void MatchAction(Action<T> some) {
            this.MatchAction(some, () =>
                {
                });
        }

        public IEnumerable<T> ToEnumerable() {
            if (isSome) {
                yield return tValue;
            }
        }

        public override string ToString() {
            return this.Match(r => "Some " + r.ToString(), () => "None");
        }

    } // Struct Option
}