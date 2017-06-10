using System.Collections;

namespace com.webjema.PanelsMonster
{
    public class HashtableScreenArguments : IScreenArguments
    {
        private Hashtable _arguments;

        public HashtableScreenArguments(Hashtable arguments)
        {
            this._arguments = arguments;
        }

        public object GetScreenArguments()
        {
            return this._arguments;
        }

        public Hashtable GetTypedScreenArguments()
        {
            return this._arguments;
        }
    } // HashtableScreenArguments
}