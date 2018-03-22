using System;
using System.Linq;
using System.Reflection;

namespace Pose.Helpers
{
    internal class ShimDelegate
    {
        public ShimDelegate(Delegate replacement)
        {
            Target = replacement.Target;
            Method = replacement.Method;
        }

        public ShimDelegate(object target, MethodInfo method)
        {
            Target = target;
            Method = method;
        }

        public object Target { get; }

        public MethodInfo Method { get; }
    }
}