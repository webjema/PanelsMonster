using System;

namespace com.webjema.Functional
{
    public static class Lazy
    {
        public static T Init<T>(T v, Func<T> f) where T : class
        {
            if (v != null) {
                return v;
            }

            return f();
        }
    }
}