using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable IDE0060 // Remove unused parameter
public class NoNamespaceClass
{
    public void OverloadedMethod()
    {
        Console.WriteLine("NoNamespaceClass::MethodOne()");
    }

    public void OverloadedMethod(int _, double __)
    {
        Console.WriteLine("NoNamespaceClass::MethodOne(i, d)");
    }

    public void OverloadedMethod(object _)
    {
        Console.WriteLine("NoNamespaceClass::MethodOne(o)");
    }

    public void OverloadedMethod(string _)
    {
        Console.WriteLine("NoNamespaceClass::MethodOne(s)");
    }
}

namespace ConsoleApp
{
    public class Qwerty
    {
        public class Zxcvbn
        {
            public int PublicMethod() { return PrivateMethod(); }
            private int PrivateMethod() { return 0; }
        }
    }

    namespace Namespace1
    {
        public class Asdfgh
        {
        }

        namespace Namespace2.Namespace3
        {
            public class NestedClass
            {
                public void FirstMethod()
                {
                    Console.WriteLine("First method");
                }

                public void ThirdMethod(int _)
                {
                    Console.WriteLine("Third method (int)");
                }

                public void SecondMethod()
                {
                    Console.WriteLine("Second method");
                }

                public void ThirdMethod(double _)
                {
                    Console.WriteLine("Third method (double)");
                }

                public void ThirdMethod(string _)
                {
                    Console.WriteLine("Third method (string)");
                }
            }
        }
    }
}
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CA1050 // Declare types in namespaces
#pragma warning restore CA1822 // Mark members as static
