using System;
using DSW.Core.Context;


namespace DSW.Context
{
	/// <summary>
	/// Description of MenuContext.
	/// </summary>
	public class MenuContext : IMenuContext
	{
        private int id;

		public MenuContext()
		{
            Console.WriteLine("makeit!");
            id = new Random().Next();
		}

        public int GetId()
        {
            Console.WriteLine(this.GetHashCode());
            return id;
        }
    }
}
