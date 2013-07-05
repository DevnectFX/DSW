using System;

namespace DSW.Extention
{
	public static class StringExtention
	{
		public static string ToFirstUpper(this string me)
		{
			if (string.IsNullOrEmpty(me) == true)
				return me;

			return me.Substring(0, 1).ToUpper() + me.Substring(1);
		}
	}
}

