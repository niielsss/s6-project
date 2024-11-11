using Microsoft.AspNetCore.Mvc;

namespace MWL.ContentService.Api.Helpers
{
    internal static class ControllerHelper
    {
        public static ProblemDetails CreateProblemDetails(string problem, string errorMessage)
        {
            var error = new KeyValuePair<string, object>("Errors", new Dictionary<string, List<string>>
            {
                { problem, new List<string> { errorMessage } }
            });

            return new()
            {
                Extensions = { error },
                Title = "An error occurred.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            };
        }
    }
}
