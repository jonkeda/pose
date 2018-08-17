using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pose.Tests.Fails
{
    [TestClass]
    public class MyTestFail
    {
        interface IMyTest
        {
            int ExplicitMethod();
        }

        class MyTest : IMyTest
        {
            int IMyTest.ExplicitMethod() => 1;
        }

        [TestMethod]
        public void TestExplicit()
        {
            int foo = 0;
            Shim shim = Shim.Replace(() => Is.A<IMyTest>().ExplicitMethod()).With((IMyTest t) => 42);
            var r = new MyTest();
            PoseContext.Isolate(() =>
            {
                foo = ((IMyTest)r).ExplicitMethod();

                Assert.AreEqual(42, foo);

            }, shim);

            
        }
    }
}
