using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.IoCs
{
    public class IoCException : Exception
    {
        public IoCException() : base() { }
        public IoCException(string massage) : base(massage)
        {

        }
    }
    public static class IoC<T>
    {
        delegate T MyFunc(params object[] args);

        static IDictionary<string, T> container = new Dictionary<string, T>();
        static IDictionary<string, MyFunc> methods = new Dictionary<string, MyFunc>
        {
            ["Registration"] = Registration,
            ["IoC.Registration"] = Registration
        };

        public static T Resolve(string key, params object[] obj)
        {
            if (methods.ContainsKey(key))
                return methods[key].Invoke(obj);

            return Get(key);
        }

        static T Registration(params object[] args)
        {
            return Add((string)args[0], (T)args[1]);
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
                throw new IoCException($"key {key} not found");
            return container[key];
        }

    }
}