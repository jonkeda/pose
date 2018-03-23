using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pose;

namespace Pose.Tests.Mocks
{
    [TestClass]
    public class MockVirtualMethodTests
    {
        public class MyClass
        {
            public virtual int MyProperty { get; set; }
            public virtual string ReturnSomething() => "something";
            public virtual string ReturnValue(string value) => value;
            public virtual int ReturnValue(int value) => value;
        }

        public class MyClassOverride : MyClass
        {
            public override int MyProperty { get { return 20;} set {} }
            public override string ReturnSomething() => "override something";
            public override string ReturnValue(string value) => "override value";
            public override int ReturnValue(int value) => value;
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
        public void MockMethodBase()
        {
            Mock mock = Mock.It<MyClass, ShimMyClass>();

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

        [TestMethod]
        public void MockMethodOverride()
        {
            Mock mock = Mock.It<MyClassOverride, ShimMyClass>();

            MyClass myClass = new MyClassOverride();

            Assert.AreEqual(myClass.MyProperty, 20);
            Assert.AreEqual(myClass.ReturnSomething(), "override something");
            Assert.AreEqual(myClass.ReturnValue("value"), "override value");
            Assert.AreEqual(myClass.ReturnValue(9), 9);

            PoseContext.Isolate(() =>
            {
                var myClass2 = new MyClassOverride();

                Assert.AreEqual(myClass2.MyProperty.ToString(), "100");
                Assert.AreEqual(myClass2.ReturnSomething(), "something else");
                Assert.AreEqual(myClass2.ReturnValue("value"), "fixed value");
                Assert.AreEqual(myClass2.ReturnValue(9).ToString(), "88");
            }, mock);

            Assert.AreEqual(myClass.MyProperty, 20);
            Assert.AreEqual(myClass.ReturnSomething(), "override something");
            Assert.AreEqual(myClass.ReturnValue("value"), "override value");
            Assert.AreEqual(myClass.ReturnValue(9), 9);

        }
    }
}
