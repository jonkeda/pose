using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pose.Tests.Mocks
{
    [TestClass]
    public class ShimExamples
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

        [TestMethod]
        public void ShimConstructor()
        {
            Shim ctorShim = Shim.Replace(() => new MyClass())
                .With(() => new MyClass { MyProperty = 10 });
            

            PoseContext.Isolate(() =>
            {
                MyClass myClass = new  MyClass();

                Assert.AreEqual("10", myClass.MyProperty.ToString());

            }, ctorShim);
        }

    }
}