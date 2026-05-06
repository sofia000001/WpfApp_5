using System.Collections.Generic;

namespace WpfApp_5
{
    public class SymbolTable
    {
        private Dictionary<string, string> symbols = new Dictionary<string, string>();

        public bool Declare(string name, string type)
        {
            if (symbols.ContainsKey(name))
                return false;

            symbols[name] = type;
            return true;
        }

        public bool Exists(string name)
        {
            return symbols.ContainsKey(name);
        }
    }
}