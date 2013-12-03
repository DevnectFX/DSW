using DSW.Responses;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DSW
{
    public static class FormatterExtensions
    {
        public static Response FromStream(this IResponseFormatter formatter, Stream stream, string filename, string contentType)
        {
            return new StreamResponse(() => stream, filename, contentType);
        }
    }
}
