using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pose.Tests.Fails
{
    public static class MyTestExtension
    {
        public static int Explicitly(this MyExtensions.MyTest myTest)
        {
            return myTest.ExplicitMethod();
        }
    }

    [TestClass]
    public class MyExtensions
    {
        public class MyTest
        {
            public int ExplicitMethod() => 1;
        }

        [TestMethod]
        public void TestExplicit()
        {
            int foo = 0;
            Shim shim = Shim.Replace(() => Is.A<MyTest>().ExplicitMethod()).With((MyTest t) => 42);
            var r = new MyTest();
            PoseContext.Isolate(() =>
            {
                foo = r.Explicitly();
            }, shim);

            Assert.AreEqual(42, foo);
        }
    }
}
