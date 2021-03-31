using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrueLayer.Pokemon.Api.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected ActionResult NotFoundResponse(int errorCode, string errorMessage)
        {
            var response = new ApiErrorResponse(errorCode, errorMessage);
            return NotFound(response);
        }
    }
}
