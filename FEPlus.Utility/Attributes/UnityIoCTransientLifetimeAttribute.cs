using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Utility.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class UnityIoCTransientLifetimeAttribute : System.Attribute
    {
        public double version;

        public UnityIoCTransientLifetimeAttribute()
        {
            version = 1.0;
        }
    }
}
