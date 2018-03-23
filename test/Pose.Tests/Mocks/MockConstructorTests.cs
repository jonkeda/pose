using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pose;

namespace Pose.Tests.Mocks
{
    [TestClass]
    public class MockConstructorTests
    {
        public class MyClass
        {
            public MyClass()
            {
                MyProperty = 5;
            }
            public MyClass(int property)
            {
                MyProperty = property;
            }

            public int MyProperty { get; set; }
        }

        public  class ShimMyClass
        {
            public static MyClass MyClass()
            {
                return new MyClass()
                {
                    MyProperty = 10
                };
            }
            public static MyClass MyClass(int property)
            {
                return new MyClass()
                {
                    MyProperty = 30
                };
            }

        }

        [TestMethod]
        public void MockMethod()
        {
            Mock mock = Mock.It<MyClass, ShimMyClass>();

            MyClass myClass1 = new MyClass();
            Assert.AreEqual(5, myClass1.MyProperty);

            MyClass myClass2 = new MyClass(20);
            Assert.AreEqual(20, myClass2.MyProperty);

            PoseContext.Isolate(() => 
                {

                    MyClass myClassA = new MyClass();
                    Assert.AreEqual("10", myClassA.MyProperty.ToString());

                    MyClass myClassB = new MyClass(20);
                    Assert.AreEqual("30", myClassB.MyProperty.ToString());

                }, mock);

            Assert.AreEqual(5, myClass1.MyProperty);
        }
    }
}
