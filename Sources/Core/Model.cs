using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSW.Core
{
    public abstract class Model
    {
        public override string ToString()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(base.ToString()).Append(" {").AppendLine();
                var list = GetType().GetProperties();
                foreach (var p in list)
                {
                    sb.Append("    ").Append(p.Name).Append(" : ").Append(p.GetValue(this, null)).AppendLine();
                }
                sb.AppendLine("}");

                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + ":" + e.StackTrace);
                return base.ToString();
            }
        }
    }
}
