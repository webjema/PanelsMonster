using com.webjema.Functional;

namespace com.webjema.PanelsMonster
{
    class ArgumentsStackItem<T>
    {
        public string ScreenName { get; set; }
        public Option<T> Arguments { get; set; }

        public ArgumentsStackItem(string screenName, Option<T> arg)
        {
            this.ScreenName = screenName;
            this.Arguments = arg;
        }
    } // ArgumentsStackItem
}