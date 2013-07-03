using System;

namespace DSW.Core
{
	public static class StringExtention
	{
		public static string ToFirstUpper(this string me)
		{
			if (string.IsNullOrEmpty(me) == true)
				return me;

			return me.Substring(0, 1).ToString().ToUpper() + me.Substring(1);
		}
	}
}

