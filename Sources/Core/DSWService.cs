using System;

namespace DSW
{
	public class DSWService
	{
		private dynamic db;
			
		
		public DSWService()
		{
		}
		
		public dynamic DB
		{
			get
			{
				if (db == null)
					db = Simple.Data.Database.Open();
				
				return db;
			}
		}
	}
}

