using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pose;

namespace Pose.Tests.Mocks
{
    [TestClass]
    public class MockTests
    {
        public class MyClass
        {
            public int MyProperty { get; set; }

            public void DoSomething() => Console.WriteLine("doing someting");
            public void DoNothing() => Console.WriteLine("doing nothing");
        }

        public  class ShimMyClass
        {
            public static int MyProperty
            {
                get { return 100; }
                set { }
            }

            public static void DoSomething() => Console.WriteLine("doing someting else");

            public static void DoMore() => Console.WriteLine("doing more");

        }

        [TestMethod]
        public void MockMethod()
        {
            Mock mock = new Mock<MyClass, ShimMyClass>();

            new MyClass().DoSomething();
            Console.WriteLine(new MyClass().MyProperty);

            PoseContext.Isolate(() =>
                {
                    // test it
                    new MyClass().DoNothing();

                    // test it
                    new MyClass().DoSomething();

                    Console.WriteLine(new MyClass().MyProperty);

                }, mock);

            new MyClass().DoSomething();
            Console.WriteLine(new MyClass().MyProperty);
        }

        [TestMethod]
        public void ShimExample()
        {
            Shim classShim = Shim.Replace(() => Is.A<MyClass>().DoSomething()).With(
                delegate (MyClass @this) { Console.WriteLine("doing someting else"); });

            Shim consoleShim = Shim.Replace(() => Console.WriteLine(Is.A<string>())).With(
                delegate (string s) { Console.WriteLine("Hijacked: {0}", s); });

            // This block executes immediately
            PoseContext.Isolate(() =>
            {
                // All code that executes within this block
                // is isolated and shimmed methods are replaced

                // Outputs "Hijacked: Hello World!"
                Console.WriteLine("Hello World!");

                //// Outputs "4/4/04 12:00:00 AM"
                //Console.WriteLine(DateTime.Now);

                //// Outputs "doing someting else"
                new MyClass().DoSomething();

                //// Outputs "doing someting else with myClass"
                //myClass.DoSomething();

            }, consoleShim, classShim); //, dateTimeShim, classPropShim, classShim, myClassShim, structShim);
        }

    }
}
