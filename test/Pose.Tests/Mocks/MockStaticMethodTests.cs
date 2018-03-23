using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pose;

namespace Pose.Tests.Mocks
{
    [TestClass]
    public class MockStaticMethodTests
    {
        public class MyClass
        {
            public static int MyProperty { get; set; }
            public static string ReturnSomething() => "something";
            public static string ReturnValue(string value) => value;
            public static int ReturnValue(int value) => value;
        }

        public  class ShimMyClass
        {
            public static int MyProperty
            {
                get { return 100; }
                set { }
            }
            public static string ReturnSomething() => "something else";
            public static string ReturnValue(string value) => "fixed value";
            public static int ReturnValue(int value) => 88;
        }

        [TestMethod]
        public void MockMethod()
        {
            Mock mock = Mock.It<MyClass, ShimMyClass>();

            Assert.AreEqual(MyClass.MyProperty, 0);
            Assert.AreEqual(MyClass.ReturnSomething(), "something");
            Assert.AreEqual(MyClass.ReturnValue("value"), "value");
            Assert.AreEqual(MyClass.ReturnValue(9), 9);

            PoseContext.Isolate(() => 
                {
                    Assert.AreEqual(MyClass.MyProperty.ToString(), "100");
                    Assert.AreEqual(MyClass.ReturnSomething(), "something else");
                    Assert.AreEqual(MyClass.ReturnValue("value"), "fixed value");
                    Assert.AreEqual(MyClass.ReturnValue(9).ToString(), "88");
                }, mock);

            Assert.AreEqual(MyClass.MyProperty, 0);
            Assert.AreEqual(MyClass.ReturnSomething(), "something");
            Assert.AreEqual(MyClass.ReturnValue("value"), "value");
            Assert.AreEqual(MyClass.ReturnValue(9), 9);

        }
    }
}
