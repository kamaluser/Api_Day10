using CourseApp.UI.Models;
using System.Net;

namespace CourseApp.UI.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode Status { get; set; }
        public ErrorResponse Error { get; set; }

        public HttpException(HttpStatusCode status)
        {
            this.Status = status;
        }

        public HttpException(HttpStatusCode status, ErrorResponse error)
        {
            this.Status = status;
            this.Error = error;
        }
    }
}
