using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tr3Line.Assessment.Api.Entities;
using Tr3Line.Assessment.Entities;

namespace Tr3Line.Assessment.Api.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)
        public Account Account => (Account)HttpContext.Items["Account"];
    }
}
