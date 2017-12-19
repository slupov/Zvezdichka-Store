using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers.Html;

namespace Zvezdichka.Web.Controllers.Helpers
{
    public class HelperController : BaseController
    {
        public async Task<IActionResult> ShowBootstrapAlert(string alertStyle, string message)
        {
            //sets the temp data according to the passed in alertStyle and message
            alertStyle = alertStyle.ToLower();

            switch (alertStyle)
            {
                case AlertStyles.Success:
                    Success(message);
                    break;
                case AlertStyles.Danger:
                    Danger(message);
                    break;
                case AlertStyles.Warning:
                    Warning(message);
                    break;
                case AlertStyles.Information:
                default:
                    Information(message);
                    break;
            }

            //returns rendered with temp data partial view 
            return PartialView("BootlstrapAlerts");
        }
    }
}