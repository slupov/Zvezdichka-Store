using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Web.Controllers;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    [Area(WebConstants.Areas.AdminArea)]
    [Authorize(Roles = WebConstants.RoleNames.AdminRole)]
    public abstract class AdminBaseController : BaseController
    {
    }
}
