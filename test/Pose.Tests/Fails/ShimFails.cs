using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pose.Tests.Fails
{
    [TestClass]
    public class ShimFails
    {
        public class MyClass
        {
            public int MyProperty { get; set; }
        }

        [TestMethod]
     //   [ExpectedException(typeof(NullReferenceException))]
        public void ShimConstructor()
        {
            Shim ctorShim = Shim.Replace(() => new MyClass())
                .With(() => new MyClass { MyProperty = 10 });

            PoseContext.Isolate(() =>
            {
                // this line breaks the Shim
                Assert.AreEqual(10, 10);

            }, ctorShim);
        }

    }
}