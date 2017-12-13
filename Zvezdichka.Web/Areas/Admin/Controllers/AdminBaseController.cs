using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Web.Controllers;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public abstract class AdminBaseController : BaseController
    {
    }
}
