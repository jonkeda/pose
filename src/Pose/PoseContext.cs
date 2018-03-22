using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Pose.Helpers;
using Pose.IL;

namespace Pose
{
    public static class PoseContext
    {
        internal static Shim[] Shims { private set; get; }
        internal static Dictionary<MethodBase, DynamicMethod> StubCache { private set; get; }

        public static void Isolate(Action entryPoint, params IShims[] shims)
        {
            if (shims == null || shims.Length == 0)
            {
                entryPoint.Invoke();
                return;
            }

            ShimCollection shms = new ShimCollection();

            foreach (IShims shim in shims)
            {
                foreach (var s in shim.GetShims())
                {
                    shms.Add(s);
                }
            }

            Shims = shms.ToArray();
            StubCache = new Dictionary<MethodBase, DynamicMethod>();

            Type delegateType = typeof(Action<>).MakeGenericType(entryPoint.Target.GetType());
            MethodRewriter rewriter = MethodRewriter.CreateRewriter(entryPoint.Method);
            ((MethodInfo)(rewriter.Rewrite())).CreateDelegate(delegateType).DynamicInvoke(entryPoint.Target);
        }
    }
}