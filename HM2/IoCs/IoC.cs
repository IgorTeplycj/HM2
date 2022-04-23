using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.IoCs
{
    public class IoC<T>
    {
        IDictionary<string, T> collection = new Dictionary<string, T>();

        public void Resolve<T>()
        {

        }
    }
}
