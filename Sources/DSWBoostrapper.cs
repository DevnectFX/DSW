using System;
using Nancy;
using Nancy.Conventions;

namespace DSW.Sources
{
	public class DSWBoostrapper : DefaultNancyBootstrapper
	{
		protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
		{
			base.ConfigureConventions(nancyConventions);

			Conventions.StaticContentsConventions.Add(
				StaticContentConventionBuilder.AddDirectory("", "WebContents", new string[] { "css", "js", "png", "gif", "jpg" })
			);
		}
	}
}