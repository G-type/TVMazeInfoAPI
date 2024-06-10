using System.Net;

namespace TVMazeInfoAPI.Exceptions
{
    public class ShowNotFoundException : Exception
    {
        public ShowNotFoundException(int showId) : base($"Show with ID {showId} not found.")
        {
        }
    }

    public class TVMazeApiException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public TVMazeApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
