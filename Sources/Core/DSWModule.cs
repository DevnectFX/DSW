using System;
using Nancy;

namespace DSW
{
	public abstract class DSWModule : NancyModule
	{
		public DSWModule()
			: base()
		{
		}

		public DSWModule(string modulePath)
			: base(modulePath)
		{
		}
	}
}

