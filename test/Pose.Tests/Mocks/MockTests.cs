using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pose;

namespace Pose.Tests.Classes
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

        public static class ShimMyClass
        {
            //public static int MyProperty
            //{
            //    get { return 100; }
            //    set { }
            //}

            public static void DoSomething() => Console.WriteLine("doing someting else");
            public static void DoMore() => Console.WriteLine("doing more");

        }

        [TestMethod]
        public void MockMethod()
        {
            Mock mock = new Mock(typeof(MyClass), typeof(ShimMyClass));

            //Mock mock = new Mock().Replace<MyClass, ShimMyClass>();
            new MyClass().DoSomething();

            PoseContext.Isolate(() =>
                {
                    // test it
                    new MyClass().DoSomething();

                }, mock.Shims);

            new MyClass().DoSomething();
        }

        [TestMethod]
        public void Example1()
        {

            // This block executes immediately
            PoseContext.Isolate(() =>
            {
                // All code that executes within this block
                // is isolated and shimmed methods are replaced

                // Outputs "Hijacked: Hello World!"
                //Console.WriteLine("Hello World!");

                //// Outputs "4/4/04 12:00:00 AM"
                //Console.WriteLine(DateTime.Now);

                //// Outputs "doing someting else"
                new MyClass().DoSomething();

                //// Outputs "doing someting else with myClass"
                //myClass.DoSomething();

            }, classShim); //, dateTimeShim, classPropShim, classShim, myClassShim, structShim);
        }

        [TestMethod]
        public void Example()
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
