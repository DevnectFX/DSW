using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy.Hosting.Self;


namespace SimpleDataTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new NancyHost(new Uri("http://localhost:7777"));
            host.Start();

            Console.ReadLine();

            host.Stop();
        }
    }
}
