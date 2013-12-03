using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace DSW.Responses
{
    public class StreamResponse : Response
    {
        private Stream source;


        public StreamResponse(Func<Stream> source, string filename, string contentType = null)
        {
            this.Contents = GetResponseBodyDelegate(source);
            this.ContentType = contentType == null ? MimeTypes.GetMimeType(filename) : contentType;
            this.StatusCode = HttpStatusCode.OK;
            this.Headers["Content-Disposition"] = "attachment; filename=\"" + filename + "\"";
        }

        private Action<Stream> GetResponseBodyDelegate(Func<Stream> sourceDelegate)
        {
            return stream =>
            {
                using (this.source = sourceDelegate.Invoke())
                {
                    this.source.CopyTo(stream);
                }
            };
        }
    }
}
