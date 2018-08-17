using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pose.Helpers;

namespace Pose.Tests.Fails
{
    [TestClass]
    public class ShimExamples2
    {
        public void Rewrite(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType)
        {

            MethodInfo mi;
            int i;
            IntPtr ip;
            Type type;

            mi = MethodBase.GetMethodFromHandle(handle, declaringType) as MethodInfo;

            type = mi.GetType();

            mi = StubHelper.GetRuntimeMethodForVirtual(type, mi);

        }



 
    }
}