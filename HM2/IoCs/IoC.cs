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
                return Add((string)obj[0], (T)obj[1]);
            }
            else
            {
                return Get(key);
            }
        }

        static T Add(string key, T val)
        {
            if (container.ContainsKey(key))
                container.Remove(key);

            container.Add(key, val);
            return container[key];
        }

        static T Get(string key)
        {
            if (!container.ContainsKey(key))
                throw new IoCException();
            return container[key];
        }

    }
}