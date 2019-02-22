using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RELOCBS.Utility
{
    interface ISession
    {
        T Get<T>(string key);
        void Set<T>(string key, T entry);
    }
}
