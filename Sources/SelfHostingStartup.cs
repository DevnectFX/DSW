using System;
using Nancy.Hosting.Self;

namespace DSW
{
	public class SelfHostingStartup
	{
		public SelfHostingStartup()
		{
		}

		public static void Main(String[] args)
		{
			var host = new NancyHost(new Uri("http://localhost"));

			Console.WriteLine("Starting Nancy Self Hosting... Service Port : 80");
			host.Start();
			Console.ReadLine();
			host.Stop();
			Console.WriteLine("Stopped Nancy Self Hosting.");
		}
	}
}
