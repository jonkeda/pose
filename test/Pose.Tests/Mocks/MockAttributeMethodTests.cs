using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pose;

namespace Pose.Tests.Mocks
{
    [TestClass]
    public class MockAttributeMethodTests
    {
        public class MyClass
        {
            public int MyProperty { get; set; }
            public string ReturnSomething() => "something";
            public string ReturnValue(string value) => value;
            public int ReturnValue(int value) => value;
        }

        [Mock(typeof(MyClass))]
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
            Mock mock = Mock.It<ShimMyClass>();

            MyClass myClass = new MyClass();

            Assert.AreEqual(myClass.MyProperty, 0);
            Assert.AreEqual(myClass.ReturnSomething(), "something");
            Assert.AreEqual(myClass.ReturnValue("value"), "value");
            Assert.AreEqual(myClass.ReturnValue(9), 9);

            PoseContext.Isolate(() => 
                {
                    var myClass2 = new MyClass();

                    Assert.AreEqual(myClass2.MyProperty.ToString(), "100");
                    Assert.AreEqual(myClass2.ReturnSomething(), "something else");
                    Assert.AreEqual(myClass2.ReturnValue("value"), "fixed value");
                    Assert.AreEqual(myClass2.ReturnValue(9).ToString(), "88");
                }, mock);

            Assert.AreEqual(myClass.MyProperty, 0);
            Assert.AreEqual(myClass.ReturnSomething(), "something");
            Assert.AreEqual(myClass.ReturnValue("value"), "value");
            Assert.AreEqual(myClass.ReturnValue(9), 9);

        }
    }
}
