using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pose;

namespace Pose.Tests.Mocks
{
    //https://www.codeproject.com/Articles/414195/Generating-Assemblies-at-runtime-using-IL-emit


    [TestClass]
    public class MockTests
    {
        public class MyClass
        {
            public int MyProperty { get; set; }
            public void DoSomething() => Console.WriteLine("doing someting");
            public void DoNothing() => Console.WriteLine("doing nothing");
            public string ReturnSomething() => "something";
            public string ReturnValue(string value) => value;
            public int ReturnValue(int value) => value;

            public static string StaticReturnValue(string value) => value;
        }

        public class ShimMyClass
        {
            public static int MyProperty
            {
                get { return 100; }
                set { }
            }

            public static void DoSomething() => Console.WriteLine("doing someting else");
            public static void DoMore() => Console.WriteLine("doing more");
            public static string ReturnSomething() => "something else";
            public static string ReturnValue(string value) => "fixed value";
            public static int ReturnValue(int value) => 88;
            public static string StaticReturnValue(string value) => "static fixed value";
        }

        [TestMethod]
        public void MockMethods()
        {
            Mock mock = Mock.It<MyClass, ShimMyClass>();

            new MyClass().DoSomething();
            Console.WriteLine(new MyClass().MyProperty);

            PoseContext.Isolate(() =>
            {
                new MyClass().DoNothing();
                new MyClass().DoSomething();
                Console.WriteLine(new MyClass().MyProperty);
                Console.WriteLine(new MyClass().ReturnSomething());
                Console.WriteLine(new MyClass().ReturnValue("a value"));
                Console.WriteLine(new MyClass().ReturnValue(22));
                Console.WriteLine(MyClass.StaticReturnValue("a static value"));

            }, mock);

            new MyClass().DoSomething();
            Console.WriteLine(new MyClass().MyProperty);
        }
    }
}
