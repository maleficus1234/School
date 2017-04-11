using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;


namespace Pie.Tests
{
    public static class Invocation
    {
        public static object InvokeShared(Assembly assembly, string className, string methodName, params object [] parameters)
        {
            return assembly.GetType(className).GetMethod(methodName).Invoke(null, parameters);
        }
    }
}
