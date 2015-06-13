//------------------------------------------------------------------------------
// <copyright file="Program.cs">
//     Copyright (c) gsksoft. All rights reserved.
// </copyright>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            TestCase("Test add and mul operators", "print 1 + 2 * 3;").ShouldOutput("7");

            TestCase("Test rel operators", "print 1 < 2;").ShouldOutput("True");

            TestCase("Test logical operators", "print not (1 < 2);").ShouldOutput("False");

            TestCase("Test logical operators", "print 1 < 2 and 2 > 1;").ShouldOutput("True");

            TestCase
                ("Test define statement", @"
                    define integer a: 2;
                    print a * 3;"
                ).ShouldOutput("6");

            TestCase
                ("Test assign(let) statement", @"
                    def int a: 1;
                    let a = 2;
                    print a;"
                ).ShouldOutput("2");

            TestCase
                ("Test if statement", @"
                    def int a: 3;
                    def int b: 4;
                    if (a > b) { print a; } else print b;"
                ).ShouldOutput("4");

            TestCase
                ("Test while statement", @"
                    def int sum: 0;
                    def int i: 1;
                    while (i <= 5) { let sum = sum + i; let i = i + 1; }
                    print sum;"
                ).ShouldOutput("15");

            Console.ReadLine();
        }

        private static InterpreterTestCase TestCase(string name, string code)
        {
            return new InterpreterTestCase(name, code);
        }
    }
}
