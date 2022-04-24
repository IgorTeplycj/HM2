using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.IoCs
{
    public class IoCException : Exception { }
    public static class IoC<T>
    {
        static IDictionary<string, T> container = new Dictionary<string, T>();

        public static T Resolve(string key, params object[] obj)
        {
            if (key.Contains("Registration"))
            {
                string _key = (string)obj[0];
                T val = (T)obj[1];

                if (container.ContainsKey(_key))
                    container.Remove(_key);

                container.Add(_key, val);
                return container[_key];
            }
            else
            {
                if (!container.ContainsKey(key))
                    throw new IoCException();
                return container[key];
            }
        }

    }
}